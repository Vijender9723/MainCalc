using System;
using System.Collections.Generic;

namespace CalculatorLibrary
{
    public class Calculator
    {
        public static double EvaluateExpression(string expression)
        {
            expression = expression.Replace(" ", ""); // Remove spaces
            List<string> tokens = TokenizeExpression(expression);
            double result = EvaluateTokens(tokens);
            return result;
        }

        private static List<string> TokenizeExpression(string expression)
        {
            List<string> tokens = new List<string>();
            string currentToken = "";

            for (int i = 0; i < expression.Length; i++)
            {
                char c = expression[i];

                if (char.IsDigit(c) || c == '.')
                {
                    currentToken += c;
                }
                else if (IsOperator(c))
                {
                    if (!string.IsNullOrEmpty(currentToken))
                    {
                        tokens.Add(currentToken);
                        currentToken = "";
                    }

                    tokens.Add(c.ToString());
                }
                else if (c == '(' || c == ')')
                {
                    if (!string.IsNullOrEmpty(currentToken))
                    {
                        tokens.Add(currentToken);
                        currentToken = "";
                    }

                    tokens.Add(c.ToString());
                }
                else
                {
                    throw new ArgumentException($"Invalid character: '{c}' in the expression.");
                }
            }

            if (!string.IsNullOrEmpty(currentToken))
            {
                tokens.Add(currentToken);
            }

            // Handle implicit multiplication
            for (int i = 0; i < tokens.Count - 1; i++)
            {
                char currentLastChar = tokens[i][tokens[i].Length - 1];
                char nextFirstChar = tokens[i + 1][0];

                if ((char.IsDigit(currentLastChar) || currentLastChar == ')') && (char.IsDigit(nextFirstChar) || nextFirstChar == '('))
                {
                    tokens.Insert(i + 1, "*");
                }
            }


            return tokens;
        }




        private static double EvaluateTokens(List<string> tokens)
        {
            Stack<double> values = new Stack<double>();
            Stack<string> ops = new Stack<string>();
            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i] == "(")
                    ops.Push(tokens[i]);
                else if (tokens[i] == ")")
                {
                    while (ops.Peek() != "(")
                        values.Push(ApplyOp(ops.Pop(), values.Pop(), values.Pop()));
                    ops.Pop();
                }
                else if (tokens[i] == "+" || tokens[i] == "-" || tokens[i] == "*" || tokens[i] == "/")
                {
                    while (ops.Count > 0 && HasPrecedence(tokens[i], ops.Peek()))
                        values.Push(ApplyOp(ops.Pop(), values.Pop(), values.Pop()));
                    ops.Push(tokens[i]);
                }
                else
                    values.Push(double.Parse(tokens[i]));
            }
            while (ops.Count > 0)
                values.Push(ApplyOp(ops.Pop(), values.Pop(), values.Pop()));
            return values.Pop();
        }

        private static bool HasPrecedence(string op1, string op2)
        {
            if (op2 == "(" || op2 == ")")
                return false;
            if ((op1 == "*" || op1 == "/") && (op2 == "+" || op2 == "-"))
                return false;
            else
                return true;
        }

        private static double ApplyOp(string op, double b, double a)
        {
            switch (op)
            {
                case "+":
                    return a + b;
                case "-":
                    return a - b;
                case "*":
                    return a * b;
                case "/":
                    if (b == 0)
                        throw new NotSupportedException("Cannot divide by zero");
                    return a / b;
            }
            return 0;
        }



        private static bool IsOperator(char c)
        {
            return c == '+' || c == '-' || c == '*' || c == '/' || c == '%';
        }
    }
}
