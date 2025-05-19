using system;

class program
{
    static void max(int num1, int num2)
    {
        if (num1 > num2)
        {
            Console.WriteLine($"The maximum number is {num1}");
        }
        else
        {
            Console.WriteLine($"The maximum number is {num2}");
        }
    }
    static void main(string[] args)
    {
        Console.WriteLine("enter the numbers ");
        int num1 = int.parse(Console.ReadLine());
        int num2 = int.parse(Console.ReadLine());

        max(num1, num2);
    }


}

