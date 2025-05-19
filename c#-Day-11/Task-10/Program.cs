using System;
using System.Linq;

public class Task10
{
    public static void Main()
    {
        Console.WriteLine("Enter 9 numbers (1-9) separated by spaces for a Sudoku row:");

        string[] input = Console.ReadLine()?.Split(' ');
        if (input == null || input.Length != 9)
        {
            Console.WriteLine("Invalid input. Please enter exactly 9 numbers.");
            return;
        }

        int[] row = new int[9];
        for (int i = 0; i < 9; i++)
        {
            if (!int.TryParse(input[i], out row[i]) || row[i] < 1 || row[i] > 9)
            {
                Console.WriteLine("Invalid input. All numbers must be between 1 and 9.");
                return;
            }
        }

        if (row.Distinct().Count() == 9)
            Console.WriteLine("Valid Sudoku row.");
        else
            Console.WriteLine("Invalid Sudoku row. Contains duplicates.");
    }
}

