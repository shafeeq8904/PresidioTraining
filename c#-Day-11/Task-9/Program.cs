using System;

public class Task9
{
    public static void Main()
    {
        const string secret = "GAME";
        int attempts = 0;

        Console.WriteLine("Guess the 4-letter secret word (only letters)");

        while (true)
        {
            Console.Write("Enter your guess: ");
            string? guess = Console.ReadLine()?.ToUpper();

            if (string.IsNullOrWhiteSpace(guess) || guess.Length != 4 || !IsOnlyLetters(guess))
            {
                Console.WriteLine("Invalid input. Please enter exactly 4 letters.");
                continue;
            }

            attempts++;

            int bulls = 0;
            int cows = 0;
            bool[] secretUsed = new bool[4];
            bool[] guessUsed = new bool[4];

            // Count bulls
            for (int i = 0; i < 4; i++)
            {
                if (guess[i] == secret[i])
                {
                    bulls++;
                    secretUsed[i] = true;
                    guessUsed[i] = true;
                }
            }

            // Count cows
            for (int i = 0; i < 4; i++)
            {
                if (!guessUsed[i])
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (!secretUsed[j] && guess[i] == secret[j])
                        {
                            cows++;
                            secretUsed[j] = true;
                            break;
                        }
                    }
                }
            }

            Console.WriteLine($"{bulls} Bulls, {cows} Cows");

            if (bulls == 4)
            {
                Console.WriteLine($"Congratulations! You guessed the word in {attempts} attempts.");
                break;
            }
        }
    }

    public static bool IsOnlyLetters(string input)
    {
        foreach (char c in input)
        {
            if (!char.IsLetter(c))
                return false;
        }
        return true;
    }
}
