using System;

public class Task8
{
    public static void Main()
    {
        Console.WriteLine("Task 8: Merge Two Arrays");

        Console.Write("Enter the first array (space-separated numbers): ");
        string? input1 = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input1))
        {
            Console.WriteLine("First array is empty. Exiting.");
            return;
        }

        string[] parts1 = input1.Split(' ');
        int[] arr1 = new int[parts1.Length];
        for (int i = 0; i < parts1.Length; i++)
        {
            if (!int.TryParse(parts1[i], out arr1[i]))
            {
                Console.WriteLine($"Invalid number in first array: {parts1[i]}");
                return;
            }
        }

        
        Console.Write("Enter the second array (space-separated numbers): ");
        string? input2 = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input2))
        {
            Console.WriteLine("Second array is empty. Exiting.");
            return;
        }

        string[] parts2 = input2.Split(' ');
        int[] arr2 = new int[parts2.Length];
        for (int i = 0; i < parts2.Length; i++)
        {
            if (!int.TryParse(parts2[i], out arr2[i]))
            {
                Console.WriteLine($"Invalid number in second array: {parts2[i]}");
                return;
            }
        }

        int[] merged = new int[arr1.Length + arr2.Length];
        int index = 0;

        foreach (int num in arr1)
            merged[index++] = num;

        foreach (int num in arr2)
            merged[index++] = num;

       
        Console.WriteLine("Merged Array:");
        Console.WriteLine(string.Join(", ", merged));
        Console.WriteLine();
    }
}
