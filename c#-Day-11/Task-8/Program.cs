using System;

public class Task8
{
    public static void Main()
    {
        int[] arr1 = {1, 3, 5};
        int[] arr2 = {2, 4, 6};

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
