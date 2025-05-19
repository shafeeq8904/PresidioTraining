using System;

class ArithmeticOperation
{
    static void Main()
    {
        Console.Write("Enter first number: ");
        double num1 = Convert.ToDouble(Console.ReadLine());

        Console.Write("Enter second number: ");
        double num2 = Convert.ToDouble(Console.ReadLine());

        Console.Write("Enter operation (+, -, *, /): ");
        string operation = Console.ReadLine();

        double result;

        switch (operation)
        {
            case "+":
                result = num1 + num2;
                Console.WriteLine($"Result: {result}");
                break;
            case "-":
                result = num1 - num2;
                Console.WriteLine($"Result: {result}");
                break;
            case "*":
                result = num1 * num2;
                Console.WriteLine($"Result: {result}");
                break;
            case "/":
                if (num2 != 0)
                {
                    result = num1 / num2;
                    Console.WriteLine($"Result: {result}");
                }
                else
                {
                    Console.WriteLine("Error: Cannot divide by zero.");
                }
                break;
            default:
                Console.WriteLine("Invalid operation.");
                break;
        }
    }
}
