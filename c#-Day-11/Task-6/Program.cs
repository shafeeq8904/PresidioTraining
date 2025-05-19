using System;
using System.Collections.Generic;

public class Task6
{
    public static void Main()
    {
        Console.WriteLine("Task 6: Frequency Count");

        int[] numbers = {1, 2, 2, 3, 4, 4, 4};
        if (numbers.Length == 0)
        {
            Console.WriteLine("The array is empty. No frequency to count.");
            return;
        }
        
        Dictionary<int, int> frequency = new Dictionary<int, int>();

        foreach (int num in numbers)
        {
            if (frequency.ContainsKey(num))
                frequency[num]++;
            else
                frequency[num] = 1;
        }

        foreach (var kvp in frequency)
        {
            Console.WriteLine($"{kvp.Key} occurs {kvp.Value} times");
        }

        Console.WriteLine();
    }
}
