using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignPatterns.Services;

namespace DesignPatterns.Notification
{
    public class SMSNotification : INotification
    {
        public void Notify(string message)
        {
            string notifyMessage = $"[SMS] Notification sent: {message}";
            Console.WriteLine(notifyMessage);
            FileService.Instance.Write(notifyMessage);
        }
    }
}
