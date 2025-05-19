using System;

public class Task7
{
    public static void Main()
    {
        Console.WriteLine("Task 7: Rotate Left by One");

        Console.Write("Enter numbers separated by spaces: ");
        string? input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("No input provided.");
            return;
        }

        string[] parts = input.Split(' ');
        int[] arr = new int[parts.Length];

        for (int i = 0; i < parts.Length; i++)
        {
            if (!int.TryParse(parts[i], out arr[i]))
            {
                Console.WriteLine($"Invalid number: {parts[i]}");
                return;
            }
        }

        if (arr.Length <= 1)
        {
            Console.WriteLine("Array is too short to rotate.");
            return;
        }

        int first = arr[0];
        for (int i = 0; i < arr.Length - 1; i++)
        {
            arr[i] = arr[i + 1];
        }
        arr[arr.Length - 1] = first;

        Console.WriteLine("Rotated Array:");
        Console.WriteLine(string.Join(", ", arr));
        Console.WriteLine();
    }
}
