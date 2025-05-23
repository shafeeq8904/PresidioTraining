using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns.Notification
{
    public interface INotificationFactory
    {
        INotification CreateNotification(string type);
    }
}
