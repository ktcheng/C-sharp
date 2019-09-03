using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Notifications;

namespace Reminders.Models
{
    public class Reminder
    {
        public string ReminderName { get; set; }
        public string LocationName { get; set; }
        public string EmailName { get; set; }
        public string ReminderNotes { get; set; }
        public string ReminderLinks { get; set; }
        public string ReminderDate { get; set; }
        public string ReminderTime { get; set; }
        public int ReminderNotification { get; set; }
        public DateTime NotificationTime { get; set; }
    }

}
