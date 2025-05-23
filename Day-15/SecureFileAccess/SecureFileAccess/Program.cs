using SecureFileAccess.Models;
using SecureFileAccess.Services;

class Program
{
    static void Main(string[] args)
    {
        bool exit = false;

        while (!exit)
        {
            Console.Clear();

            Console.Write("Enter your name: ");
            string name = Console.ReadLine();

            Console.Write("Enter your role (Admin/User/Guest): ");
            string role = Console.ReadLine();

            User user = new User(name, role);
            ProxyFile proxy = new ProxyFile(user);

            Console.WriteLine($"\nUser: {user.Username} | Role: {user.Role}");
            proxy.Read();

            Console.WriteLine("\nPress Q to quit or any other key to continue...");
            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Q)
            {
                exit = true;
            }
        }
    }
}