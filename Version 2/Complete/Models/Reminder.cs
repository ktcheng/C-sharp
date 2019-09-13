using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompleteList.Models
{
    public class Reminder
    {
        public string ReminderName { get; set; }
        public string ReminderLocation { get; set; }
        public string ReminderNotes { get; set; }
        public string ReminderDate { get; set; }
        public string ReminderTime { get; set; }
        public DateTime ReminderNotification { get; set; }
    }
}