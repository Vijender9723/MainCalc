using System;
using System.Linq;
using CalculatorLibrary;
using PowerOperations;

class Program
{
    static void Main()
    {
        string expression = "";
        string originalExpression = ""; // Stores the original expression before evaluation
        string result = "";
        ConsoleKeyInfo key;
        bool scientificMode = false;
        bool awaitingNumberForSquareRoot = false;
        bool awaitingNumberForScientific = false;
        double numberForSquareRoot = 0;
        double numberForScientific = 0;
        string message = ""; // For displaying square root prompt

        while (true)
        {
            Console.WriteLine($"Welcome to the {(scientificMode ? "Scientific" : "Simple")} Calculator. Hit Escape to exit");

            if (scientificMode)
            {
                Console.WriteLine(message);
                Console.WriteLine("Scientific Mode: Press 's' for sin, 'c' for cos, 't' for tan, 'r' for sqrt, 'p' for power, 'P' for PI, '%' for percentage, and 'x' to switch modes.");
            }

            Console.WriteLine("Live Expression");

            // Check if original expression exists for continuous editing
            if (!string.IsNullOrEmpty(originalExpression))
            {
                expression = originalExpression;
            }

            // Print the expression on the first line
            Console.WriteLine(expression.PadRight(Console.WindowWidth));

            // Print "Result" and the actual result on the next line
            if (!string.IsNullOrEmpty(result))
            {
                Console.WriteLine($"Result: {result}");
            }

            // Read key press
            key = Console.ReadKey(true);

            // Handle key presses
            switch (key.Key)
            {
                case ConsoleKey.Backspace:
                    if (expression.Length > 0)
                    {
                        expression = expression.Substring(0, expression.Length - 1);
                        originalExpression = ""; // Clear original expression when editing
                        result = ""; // Clear the result when editing
                    }
                    break;

                case ConsoleKey.Enter:
                    if (expression.ToLower() == "exit")
                    {
                        Console.WriteLine("Goodbye!");
                        return; // Exit the program
                    }

                    originalExpression = expression; // Store the original expression for continuous editing

                    // Remove trailing operator if present
                    if (IsOperator(expression.LastOrDefault()))
                    {
                        expression = expression.Substring(0, expression.Length - 1);
                    }

                    try
                    {
                        double calculatedResult = Calculator.EvaluateExpression(expression);
                        result = calculatedResult.ToString();
                        expression = ""; // Clear the expression for new input
                    }
                    catch (Exception ex)
                    {
                        result = $"Error: {ex.Message}";
                    }
                    break;

                case ConsoleKey.Escape:
                    // Exit the program
                    Console.WriteLine("Goodbye!");
                    return;

                case ConsoleKey.X:
                    // Toggle between scientific and simple modes
                    scientificMode = !scientificMode;
                    break;

                case ConsoleKey.R:
                    if (scientificMode)
                    {
                        message = "Enter number to get square root: ";
                        awaitingNumberForSquareRoot = true;
                    }
                    break;

                // Add cases for other scientific modes here
                case ConsoleKey.S:
                case ConsoleKey.C:
                case ConsoleKey.T:
                case ConsoleKey.P:
                case ConsoleKey.Q: // Placeholder for PI
                case ConsoleKey.D5: // ConsoleKey.D5 corresponds to the '%' character
                    if (scientificMode)
                    {
                        char scientificFunctionKey = char.ToLower(key.KeyChar);
                        message = $"Enter number to get {scientificFunctionKey}: ";
                        awaitingNumberForScientific = true;
                    }
                    break;

                default:
                    if (IsAllowedCharacter(key.KeyChar))
                    {
                        if (IsOperator(expression.LastOrDefault()) && IsOperator(key.KeyChar))
                        {
                            // Replace the last operator if another operator is typed
                            expression = expression.Substring(0, expression.Length - 1);
                        }

                        expression += key.KeyChar;

                        if (awaitingNumberForSquareRoot)
                        {
                            // If awaiting a number for square root, handle it here
                            if (char.IsDigit(key.KeyChar))
                            {
                                numberForSquareRoot = double.Parse(expression);
                                try
                                {
                                    double sqrtResult = MyPower.SquareRoot(numberForSquareRoot);
                                    result = $"√{numberForSquareRoot} = {sqrtResult}";
                                    expression = ""; // Clear the expression for new input
                                    awaitingNumberForSquareRoot = false;
                                }
                                catch (Exception ex)
                                {
                                    result = $"Error: {ex.Message}";
                                }
                            }
                        }
                        else if (awaitingNumberForScientific)
                        {
                            if (IsAllowedCharacter(key.KeyChar))
                            {
                                // Extract the number before the decimal point
                                if (key.KeyChar == '.')
                                {
                                    numberForScientific = double.Parse(expression.Substring(0, expression.Length - 1));
                                }
                                else
                                {
                                    expression += key.KeyChar;
                                }
                            }

                            // If awaiting a number for scientific function, handle it here
                            if (char.IsDigit(key.KeyChar))
                            {
                                numberForScientific = double.Parse(expression);
                                try
                                {
                                    double scientificResult = EvaluateScientificFunction(key.KeyChar, numberForScientific);
                                    result = $"{key.KeyChar}({numberForScientific}) = {scientificResult}";
                                    expression = ""; // Clear the expression for new input
                                    awaitingNumberForScientific = false;
                                }
                                catch (Exception ex)
                                {
                                    result = $"Error: {ex.Message}";
                                }
                            }
                        }
                        else
                        {
                            originalExpression = ""; // Clear original expression when adding new characters
                            result = ""; // Clear the result when editing
                        }
                    }
                    break;
            }

            // Clear the console and start over
            Console.Clear();
        }
    }

    private static double EvaluateScientificFunction(char functionKey, double value)
    {
        switch (char.ToLower(functionKey))
        {
            case 's':
                return MyPower.Sin(value, "DEG");
            case 'c':
                return MyPower.Cos(value, "DEG");
            case 't':
                return MyPower.Tan(value, "DEG");
            case 'p':
                return MyPower.CustomPower(value, 2); // Placeholder for power function, replace with your logic
            case 'q':
                return MyPower.PI();
            case '%':
                return MyPower.Percentage(value, 100);
            default:
                throw new ArgumentException($"Unsupported scientific function: {functionKey}");
        }
    }

    private static bool IsAllowedCharacter(char c)
    {
        return char.IsDigit(c) || IsOperator(c) || c == '(' || c == ')' || c == '.' || c == '-';
    }

    private static bool IsOperator(char c)
    {
        return c == '+' || c == '-' || c == '*' || c == '/' || c == '%';
    }
}
