/*
Design a C# console app that uses a jagged array to store data for Instagram posts by multiple users. Each user can have a different number of posts, 
and each post stores a caption and number of likes.
You have N users, and each user can have M posts (varies per user).
*/

using System;

public class InstagramPost
{
    public string Caption { get; set; }
    public int Likes { get; set; }
}

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Enter the number of users:");
        if (!int.TryParse(Console.ReadLine(), out int userCount) || userCount <= 0)
        {
            Console.WriteLine("Invalid number of users.");
            return;
        }

        //
        InstagramPost[][] userPosts = new InstagramPost[userCount][];

        for (int i = 0; i < userCount; i++)
        {
            Console.WriteLine($"Enter the number of posts for user {i + 1}:");
            if (!int.TryParse(Console.ReadLine(), out int postCount) || postCount <= 0)
            {
                Console.WriteLine("Invalid number of posts.");
                return;
            }

            userPosts[i] = new InstagramPost[postCount];

            for (int j = 0; j < postCount; j++)
            {
                Console.Write($"Enter caption for post {j + 1}: ");
                string caption = Console.ReadLine() ?? "";

                Console.Write("Enter likes: ");
                if (!int.TryParse(Console.ReadLine(), out int likes) || likes < 0)
                {
                    Console.WriteLine("Invalid number of likes.");
                    return;
                }

                userPosts[i][j] = new InstagramPost { Caption = caption, Likes = likes };
            }
        }
        
        Console.WriteLine("\n--- Displaying Instagram Posts ---");
        for (int i = 0; i < userPosts.Length; i++)
        {
            Console.WriteLine($"User {i + 1}:");
            for (int j = 0; j < userPosts[i].Length; j++)
            {
                var post = userPosts[i][j];
                Console.WriteLine($"Post {j + 1} - Caption: {post.Caption} | Likes: {post.Likes}");
            }
            Console.WriteLine();
        }
    }
}

