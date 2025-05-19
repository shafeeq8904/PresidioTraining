using System;
using System.Linq;

public class Task12
{
    public static void Main()
    {
        Console.Write("Enter a lowercase message (letters only): ");
        string input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Message cannot be empty or null.");
            return;
        }

        if (!input.All(char.IsLower))
        {
            Console.WriteLine("Invalid input. Please use lowercase letters only (no spaces or symbols).");
            return;
        }

        Console.Write("Enter shift amount: ");
        string shiftInput = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(shiftInput) || !int.TryParse(shiftInput, out int shift))
        {
            Console.WriteLine("Invalid shift value. Please enter a valid integer.");
            return;
        }

        string encrypted = Encrypt(input, shift);
        string decrypted = Decrypt(encrypted, shift);

        Console.WriteLine($"Encrypted: {encrypted}");
        Console.WriteLine($"Decrypted: {decrypted}");
    }

    private static string Encrypt(string text, int shift)
    {
        char ShiftChar(char c)
        {
            return (char)('a' + (c - 'a' + shift) % 26);
        }

        return string.Concat(text.Select(c => ShiftChar(c)));
    }

    private static string Decrypt(string text, int shift)
    {
        char UnshiftChar(char c)
        {
            return (char)('a' + (c - 'a' - shift + 26) % 26);
        }

        return string.Concat(text.Select(c => UnshiftChar(c)));
    }
}
