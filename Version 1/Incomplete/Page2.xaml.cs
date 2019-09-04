using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Reminders.Models;
using Microsoft.Toolkit.Uwp.Notifications;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Reminders
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    
    public sealed partial class BlankPage2 : Page
    {
        public BlankPage2()
        {
            this.InitializeComponent();
        }
        
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage), null);
        }
        
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
        
        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            LinkBox.Text = "";
            ReminderNameBox.Text = "";
            LocationBox.Text = "";
            EmailBox.Text = "";
            NotesBox.Text = "";
            
            ReminderDatePickerStart.Date = DateTime.Now;
            ReminderDatePickerEnd.Date = DateTime.Now;
            ReminderTimePicker.Time = TimeSpan.Zero;
            NotificationComboBox.SelectedValue = null;

            SundayCheckBox.IsChecked = false;
            MondayCheckBox.IsChecked = false;
            TuesdayCheckBox.IsChecked = false;
            WednesdayCheckBox.IsChecked = false;
            ThursdayCheckBox.IsChecked = false;
            FridayCheckBox.IsChecked = false;
            SaturdayCheckBox.IsChecked = false;
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            var parameter1 = new Reminder();

            if(String.IsNullOrWhiteSpace(ReminderNameBox.Text))
            {
                parameter1.ReminderName = "Untitled Reminder";
            }
            else
            {
                parameter1.ReminderName = ReminderNameBox.Text;
            }

            if (String.IsNullOrWhiteSpace(LocationBox.Text))
            {
                parameter1.LocationName = "Location: None";
            }
            else
            {
                parameter1.LocationName = LocationBox.Text;
            }

            if (String.IsNullOrWhiteSpace(EmailBox.Text))
            {
                parameter1.EmailName = "Email: None";
            }
            else
            {
                parameter1.EmailName = EmailBox.Text;
            }

            if (String.IsNullOrWhiteSpace(NotesBox.Text))
            {
                parameter1.ReminderNotes = "Notes: None";
            }
            else
            {
                parameter1.ReminderNotes = NotesBox.Text;
            }

            if (String.IsNullOrWhiteSpace(LinkBox.Text))
            {
                parameter1.ReminderLinks = "Links: None";
            }
            else
            {
                parameter1.ReminderLinks = LinkBox.Text;
            }

            var reminderDateStart = ReminderDatePickerStart.Date;
            var reminderDateEnd = ReminderDatePickerEnd.Date;

            if ((reminderDateStart.HasValue) && (reminderDateEnd.HasValue))
            {
                DateTime date1 = reminderDateStart.Value.DateTime;
                var formattedDate1 = date1.ToString("MM/dd/yyyy");

                DateTime date2 = reminderDateEnd.Value.DateTime;
                var formattedDate2 = date2.ToString("MM/dd/yyyy");

                parameter1.ReminderDate = formattedDate1 + " : " + formattedDate2;
            }
            else
            {
                parameter1.ReminderDate = "Date: None";
            }

            var reminderTimeHours = ReminderTimePicker.Time.Hours;
            var reminderTimeMinutes = ReminderTimePicker.Time.Minutes;
            
            string amPm;
            string formattedHours;
            string formattedMinutes;
            string timeString;

            if (reminderTimeHours > 12)
            {
                reminderTimeHours -= 12;
                amPm = "PM";
            }
            else
            {
                amPm = "AM";
            }

            if (reminderTimeHours < 10)
            {
                formattedHours = reminderTimeHours.ToString();
                formattedHours = "0" + formattedHours;
            }
            else
            {
                formattedHours = reminderTimeHours.ToString();
            }

            if (reminderTimeMinutes < 10)
            {
                formattedMinutes = reminderTimeMinutes.ToString();
                formattedMinutes = "0" + formattedMinutes;
            }
            else
            {
                formattedMinutes = reminderTimeMinutes.ToString();
            }

            if (reminderTimeHours == 0 && reminderTimeMinutes == 0)
            {
                timeString = "Time: None";
            }
            else
            {
                timeString = formattedHours + ":" + formattedMinutes + " " + amPm;
            }

            parameter1.ReminderTime = timeString;

            this.Frame.Navigate(typeof(MainPage), parameter1);
        }
    }
}
