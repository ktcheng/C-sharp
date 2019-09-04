using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static Windows.UI.Xaml.Controls.Page;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Reminders.Models;
using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.QueryStringDotNET;
using System.Xml;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Reminders
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage1 : Page
    {
        public BlankPage1()
        {
            this.InitializeComponent();
            ReminderDatePicker.Date = DateTime.Now;
            ReminderTimePicker.Time = TimeSpan.Zero;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter != null)
            {
                var editReminder = e.Parameter as Reminder;
                ReminderNameBox.Text = editReminder.ReminderName;
                LocationBox.Text = editReminder.LocationName;
                EmailBox.Text = editReminder.EmailName;
                NotesBox.Text = editReminder.ReminderNotes;
                LinkBox.Text = editReminder.ReminderLinks;
            }
        }


        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            var parameter1 = new Reminder();

            if (String.IsNullOrWhiteSpace(ReminderNameBox.Text))
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

            var reminderDate = ReminderDatePicker.Date;

            if (reminderDate.HasValue)
            {
                DateTime date1 = reminderDate.Value.DateTime;

                var notificationDate = ReminderDatePicker.Date.Value.DateTime;
                parameter1.NotificationTime = notificationDate;

                var formattedDate = date1.ToString("MM/dd/yyyy");
                parameter1.ReminderDate = formattedDate;
            }
            else
            {
                parameter1.ReminderDate = "Date: None";
                parameter1.NotificationTime = DateTime.Now;
            }

            var reminderTimeHours = ReminderTimePicker.Time.Hours;
            var reminderTimeMinutes = ReminderTimePicker.Time.Minutes;

            string amPm;
            if (reminderTimeHours > 12)
            {
                reminderTimeHours -= 12;
                amPm = "PM";
            }
            else
            {
                amPm = "AM";
            }

            string formattedHours;
            if (reminderTimeHours < 10)
            {
                formattedHours = reminderTimeHours.ToString();
                formattedHours = "0" + formattedHours;
            }
            else
            {
                formattedHours = reminderTimeHours.ToString();
            }

            string formattedMinutes;
            if (reminderTimeMinutes < 10)
            {
                formattedMinutes = reminderTimeMinutes.ToString();
                formattedMinutes = "0" + formattedMinutes;
            }
            else
            {
                formattedMinutes = reminderTimeMinutes.ToString();
            }

            string timeString;
            if (reminderTimeHours == 0 && reminderTimeMinutes == 0)
            {
                timeString = "Time: None";
            }
            else
            {
                timeString = formattedHours + ":" + formattedMinutes + " " + amPm;
            }

            parameter1.ReminderTime = timeString;

            int reminderComboBox;

            if (NotificationComboBox.SelectedValue != null)
            {
                switch (NotificationComboBox.SelectedValue.ToString())
                {
                    case "5 Minutes Before":
                        reminderComboBox = 5;
                        break;
                    case "10 Minutes Before":
                        reminderComboBox = 10;
                        break;
                    case "15 Minutes Before":
                        reminderComboBox = 15;
                        break;
                    case "30 Minutes Before":
                        reminderComboBox = 30;
                        break;
                    case "1 Hour Before":
                        reminderComboBox = 60;
                        break;
                    case "5 Hours Before":
                        reminderComboBox = 300;
                        break;
                    default:
                        reminderComboBox = 0;
                        break;
                }
            }
            else
            {
                reminderComboBox = 0;
            }

            parameter1.ReminderNotification = reminderComboBox;


            ToastVisual visual = new ToastVisual()
            {
                BindingGeneric = new ToastBindingGeneric()
                {
                    Children =
                    {
                        new AdaptiveText()
                        {
                            Text = "Reminder"
                        },
                        new AdaptiveText()
                        {
                            Text = "Body"
                        },
                        new AdaptiveText()
                        {
                            Text = "End Reminder"
                        }
                    }
                }
            };
            ToastContent toastContent = new ToastContent()
            {
                Visual = visual
            };
            var toast = new ToastNotification(toastContent.GetXml());

            //ToastNotificationManager.CreateToastNotifier().Show(toast);
            toast.ExpirationTime = DateTime.Now.AddSeconds(5);

            //var toast = new Windows.UI.Notifications.ScheduledToastNotification(toastContent.GetXml, ReminderDatePicker.Date.Value.DateTime);

            if (ReminderDatePicker.Date.HasValue)
            {
                if (ReminderDatePicker.Date.Value.Date == DateTime.Today.Date)
                {
                    if (ReminderTimePicker.Time.Hours < DateTime.Now.Hour)
                    {
                        ToastNotificationManager.CreateToastNotifier().Show(toast);
                    }
                    else
                    {
                        //var timeSpan = new TimeSpan(0, ReminderTimePicker.Time.Hours - DateTime.Now.Hour, ReminderTimePicker.Time.Minutes - DateTime.Now.Minute, 0);
                        var midnight = DateTime.Now;

                        var minutesCounter = ReminderTimePicker.Time.Minutes - DateTime.Now.Minute;
                        var hoursCounter = ReminderTimePicker.Time.Hours - DateTime.Now.Hour;
                        var intMinutesCounter = Convert.ToInt32(minutesCounter);
                        var intHoursCounter = Convert.ToInt32(hoursCounter);
                        var timeSpan = new TimeSpan(0, intHoursCounter, intMinutesCounter, 0);

                        DateTimeOffset scheduledTime = midnight + timeSpan;

                        var toast1 = new ScheduledToastNotification(toastContent.GetXml(), scheduledTime);
                        //ToastNotificationManager.CreateToastNotifier().Show(toast);
                        ToastNotificationManager.CreateToastNotifier().AddToSchedule(toast1);
                    }
                }
            }

            //var toast = new ScheduledToastNotification(toastContent.GetXml(), ReminderDatePicker.Date.Value.DateTime);
            //ToastNotificationManager.CreateToastNotifier().AddToSchedule(toast);


            this.Frame.Navigate(typeof(MainPage), parameter1);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage), null);
        }
        private void Recurring_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BlankPage2), null);
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
        private void Create_Click(object sender, RoutedEventArgs e)
        {
            var parameter1 = new Reminder();

            if (String.IsNullOrWhiteSpace(ReminderNameBox.Text))
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
            
            var reminderDate = ReminderDatePicker.Date;
            if (reminderDate.HasValue)
            {
                DateTime date1 = reminderDate.Value.DateTime;
                var formattedDate = date1.ToString("MM/dd/yyyy");
                parameter1.ReminderDate = formattedDate;
            }
            else
            {
                parameter1.ReminderDate = "Date: None";
            }

            var reminderTimeHours = ReminderTimePicker.Time.Hours;
            var reminderTimeMinutes = ReminderTimePicker.Time.Minutes;

            string amPm;
            if (reminderTimeHours > 12)
            {
                reminderTimeHours -= 12;
                amPm = "PM";
            }
            else
            {
                amPm = "AM";
            }

            string formattedHours;
            if (reminderTimeHours < 10)
            {
                formattedHours = reminderTimeHours.ToString();
                formattedHours = "0" + formattedHours;
            }
            else
            {
                formattedHours = reminderTimeHours.ToString();
            }

            string formattedMinutes;
            if (reminderTimeMinutes < 10)
            {
                formattedMinutes = reminderTimeMinutes.ToString();
                formattedMinutes = "0" + formattedMinutes;
            }
            else
            {
                formattedMinutes = reminderTimeMinutes.ToString();
            }

            string timeString;
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

        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            LinkBox.Text = "";
            ReminderNameBox.Text = "";
            LocationBox.Text = "";
            EmailBox.Text = "";
            NotesBox.Text = "";
            ReminderDatePicker.Date = DateTime.Now;
            ReminderTimePicker.Time = TimeSpan.Zero;
            NotificationComboBox.SelectedValue = null;
        }
    }
}