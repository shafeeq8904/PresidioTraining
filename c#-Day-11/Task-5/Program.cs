using System;

class DivisibleBy7Counter
{
    static void Main()
    {
        int count = 0;

        for (int i = 1; i <= 10; i++)
        {
            Console.Write($"Enter number {i}: ");
            int num = Convert.ToInt32(Console.ReadLine());

            if (num % 7 == 0)
            {
                count++;
            }
        }

        Console.WriteLine($"Total numbers divisible by 7: {count}");
    }
}
