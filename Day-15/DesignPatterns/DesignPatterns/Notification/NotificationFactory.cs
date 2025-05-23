using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns.Notification
{
    public class NotificationFactory
    {
        public INotification Create(string type)
        {
            return type.ToLower() switch
            {
                "email" => new EmailNotification(),
                "sms" => new SMSNotification(),
                _ => throw new ArgumentException("Invalid notification type")
            };
        }
    }
}
