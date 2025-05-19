using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("enter the name");

        string name = Console.ReadLine();

        Greeting(name);
    }

    static void Greeting(string name)
    {
        Console.WriteLine($"Hello {name}");
    }
}