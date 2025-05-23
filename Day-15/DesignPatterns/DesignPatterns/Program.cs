using DesignPatterns.Notification;
using DesignPatterns.Services;

class Program
{
    static void Main()
    {
        var fileService = FileService.Instance;
        INotificationFactory notificationFactory = new ConcreteNotificationFactory();

        bool keepRunning = true;

        while (keepRunning)
        {
            Console.WriteLine("Choose notification method:");
            Console.WriteLine("1. Email");
            Console.WriteLine("2. SMS");
            Console.WriteLine("3. Exit");
            Console.Write("Enter choice (1, 2 or 3): ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("\nEnter your message:");
                    string emailMessage = Console.ReadLine();

                    var emailNotifier = notificationFactory.CreateNotification("email");
                    emailNotifier.Notify(emailMessage);
                    break;

                case "2":
                    Console.WriteLine("\nEnter your message:");
                    string smsMessage = Console.ReadLine();

                    var smsNotifier = notificationFactory.CreateNotification("sms");
                    smsNotifier.Notify(smsMessage);
                    break;
                case "3":
                    Console.WriteLine("Exiting program...");
                    keepRunning = false;
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please enter 1, 2, or 3.");
                    break;
            }
        }

        fileService.Dispose(); 
    }
}
