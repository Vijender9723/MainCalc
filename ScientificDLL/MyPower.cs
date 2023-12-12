// PowerOperations.dll

using System;

namespace PowerOperations
{
    public class MyPower
    {
        public static double SquareRoot(double number)
        {
            if (number < 0)
            {
                throw new ArgumentException("Invalid input");
            }
            return Math.Sqrt(number);
        }

        public static double Cube(double number)
        {
            return Math.Pow(number, 3);
        }

        public static double Power(double baseValue, double exponent)
        {
            return Math.Pow(baseValue, exponent);
        }

        public static double CustomPower(double baseValue, double exponent)
        {
            // Implement your custom power calculation logic here
            // For example, you can use a loop to calculate power
            double result = 1;
            for (int i = 0; i < exponent; i++)
            {
                result *= baseValue;
            }
            return result;
        }

        public static double PI()
        {
            return Math.PI;
        }

        public static double Percentage(double value, double percentage)
        {
            return (percentage / 100) * value;
        }

        public static string ScientificNotation(double number)
        {
            return number.ToString("E");
        }

        // Additional trigonometric functions
        public static double Sin(double angle, string status)
        {
            angle = AngleChanger(angle, status);
            return Math.Sin(angle);
        }

        public static double Cos(double angle, string status)
        {
            angle = AngleChanger(angle, status);
            return Math.Cos(angle);
        }

        public static double Tan(double angle, string status)
        {
            angle = AngleChanger(angle, status);
            return Math.Tan(angle);
        }

        private static double AngleChanger(double angle, string status)
        {
            if (status == "DEG")
            {
                angle = (angle * Math.PI) / 180;
            }
            else if (status == "GRAD")
            {
                angle = (angle * Math.PI) / 200;
            }
            return angle;
        }
    }
}
