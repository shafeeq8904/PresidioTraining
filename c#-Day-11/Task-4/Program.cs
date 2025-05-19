using System;

class Validation
{
    static void Main(string[] args)
    {
        int count = 0;
        while (count < 3)
        {
            Console.Write("Enter username: ");
            string username = Console.ReadLine();

            Console.Write("Enter password: ");
            string password = Console.ReadLine();

            if (username == "admin" && password == "pass")
            {
                Console.WriteLine("Login successful!");
                break;
            }

            count++;
            Console.WriteLine($"Invalid credentials. You have {3 - count} attempts left.");

        }
        
        Console.WriteLine("Invalid attempts for 3 times. Exiting....");
    }
}