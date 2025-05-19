using System;
using System.Linq;

public class Task11
{
    public static void Main()
    {
        int[,] board = new int[9, 9];

        Console.WriteLine("Enter each row of the Sudoku board (9 numbers from 1-9 separated by space):");

        for (int i = 0; i < 9; i++)
        {
            string[] input = Console.ReadLine()?.Split(' ');

            if (input == null || input.Length != 9)
            {
                Console.WriteLine($"Row {i + 1} is invalid (must contain exactly 9 numbers).");
                return;
            }

            for (int j = 0; j < 9; j++)
            {
                if (!int.TryParse(input[j], out board[i, j]) || board[i, j] < 1 || board[i, j] > 9)
                {
                    Console.WriteLine($"Row {i + 1} contains invalid number '{input[j]}'. Numbers must be from 1 to 9.");
                    return;
                }
            }

            var row = Enumerable.Range(0, 9).Select(j => board[i, j]);
            if (row.Distinct().Count() != 9)
            {
                Console.WriteLine($"Row {i + 1} is invalid (contains duplicates).");
                return;
            }
        }

        for (int j = 0; j < 9; j++)
        {
            var column = Enumerable.Range(0, 9).Select(i => board[i, j]);
            if (column.Distinct().Count() != 9)
            {
                Console.WriteLine($"Column {j + 1} is invalid (contains duplicates).");
                return;
            }
        }

        Console.WriteLine("All rows and columns are valid.");
    }
}
