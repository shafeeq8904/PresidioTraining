using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns.Notification
{
    public class ConcreteNotificationFactory : INotificationFactory
    {
        private readonly NotificationFactory _factory = new NotificationFactory();

        public INotification CreateNotification(string type)
        {
            return _factory.Create(type);
        }
    }
}
