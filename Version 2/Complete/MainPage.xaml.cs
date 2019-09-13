using CompleteList.Models;
using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.QueryStringDotNET;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CompleteList
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public ObservableCollection<Reminder> RemindersList; 
        public MainPage()
        {
            this.InitializeComponent();
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            bool newPage = true;
            bool onStart = true;

            if (newPage)
            {
                try
                {
                    var folder = ApplicationData.Current.LocalFolder;
                    var file = await folder.GetFileAsync("collection.json");

                    if (file != null)
                    {
                        using (var stream = await file.OpenStreamForReadAsync())
                        using (var reader = new StreamReader(stream, Encoding.UTF8))
                        {
                            string json = await reader.ReadToEndAsync();
                            RemindersList = JsonConvert.DeserializeObject<ObservableCollection<Reminder>>(json);
                        }
                        newPage = false;
                    }
                    else
                    {
                        RemindersList = new ObservableCollection<Reminder>();
                    }
                }
                catch
                {
                }
            }
            else if (onStart)
            {
                try
                {
                    var folder = ApplicationData.Current.LocalFolder;
                    var file = await folder.GetFileAsync("collection.json");

                    if (file != null)
                    {
                        using (var stream = await file.OpenStreamForReadAsync())
                        using (var reader = new StreamReader(stream, Encoding.UTF8))
                        {
                            string json = await reader.ReadToEndAsync();
                            RemindersList = JsonConvert.DeserializeObject<ObservableCollection<Reminder>>(json);
                        }
                        newPage = false;
                    }
                    else
                    {
                        RemindersList = new ObservableCollection<Reminder>();
                    }
                }
                catch
                {
                }
            }
        }
        private void AddNew_Click(object sender, RoutedEventArgs e)
        {
            CLListView.SelectedItems.Clear();
            ReminderView.IsPaneOpen = true;
        }
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (ReminderView.IsPaneOpen == false)
            {
                if (CLListView.SelectedItems.Count == 1)
                {
                    foreach (Reminder selectedItem in CLListView.SelectedItems)
                    {
                        ReminderView.IsPaneOpen = true;

                        if (selectedItem.ReminderName != "Untitled Reminder")
                        {
                            NameBox.Text = selectedItem.ReminderName;
                        }
                        else
                        {
                            NameBox.Text = "";
                        }

                        if (selectedItem.ReminderLocation != "Location: None")
                        {
                            LocationBox.Text = selectedItem.ReminderLocation;
                        }
                        else
                        {
                            LocationBox.Text = "";
                        }

                        if (selectedItem.ReminderNotes != "Notes: None")
                        {
                            NotesBox.Text = selectedItem.ReminderNotes;
                        }
                        else
                        {
                            NotesBox.Text = "";
                        }

                        if (selectedItem.ReminderDate != "Date: None")
                        {
                            CLDatePicker.Date = Convert.ToDateTime(selectedItem.ReminderDate);
                        }

                        if (selectedItem.ReminderTime != "Time: None")
                        {
                            int hourTime;
                            int minuteTime;

                            if (selectedItem.ReminderTime.Length < 8)
                            {
                                hourTime = Convert.ToInt32(selectedItem.ReminderTime.Substring(0, 1));
                                minuteTime = Convert.ToInt32(selectedItem.ReminderTime.Substring(2, 2));
                            }
                            else
                            {
                                hourTime = Convert.ToInt32(selectedItem.ReminderTime.Substring(0, 2));
                                minuteTime = Convert.ToInt32(selectedItem.ReminderTime.Substring(3, 2));
                            }

                            TimeSpan timeSpan = new TimeSpan(0, hourTime, minuteTime, 0);
                            CLTimePicker.SelectedTime = timeSpan;
                        }
                    }
                }
            }
        }
        private void SelectAll_Click(object sender, RoutedEventArgs e)
        {
            CLListView.SelectAll();
        }
        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (RemindersList != null && RemindersList.Any())
            {
                for (int i = CLListView.Items.Count; i > 0; i--)
                {
                    foreach (Reminder selectedItem in CLListView.SelectedItems)
                    {
                        RemindersList.Remove(selectedItem);
                    }
                }
            }

            var localfolder = ApplicationData.Current.LocalFolder;
            var file = await localfolder.CreateFileAsync("collection.json", CreationCollisionOption.ReplaceExisting);
            using (var stream = await file.OpenStreamForWriteAsync())
            using (var writer = new StreamWriter(stream, Encoding.UTF8))
            {
                string json = JsonConvert.SerializeObject(RemindersList);
                await writer.WriteAsync(json);
            }
        }
        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            NameBox.Text = "";
            LocationBox.Text = "";
            NotesBox.Text = "";
            CLDatePicker.Date = null;
            CLTimePicker.Time = TimeSpan.Zero;
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            NameBox.Text = "";
            LocationBox.Text = "";
            NotesBox.Text = "";
            CLDatePicker.Date = null;
            CLTimePicker.Time = TimeSpan.Zero;
            ReminderView.IsPaneOpen = false;
        }
        private async void Create_Click(object sender, RoutedEventArgs e)
        {
            if (CLListView.SelectedItems.Count == 1)
            {
                Reminder reminder = new Reminder();

                if (String.IsNullOrWhiteSpace(NameBox.Text))
                {
                    reminder.ReminderName = "Untitled Reminder";
                }
                else
                {
                    reminder.ReminderName = NameBox.Text;
                }

                if (String.IsNullOrWhiteSpace(LocationBox.Text))
                {
                    reminder.ReminderLocation = "Location: None";
                }
                else
                {
                    reminder.ReminderLocation = LocationBox.Text;
                }

                if (String.IsNullOrWhiteSpace(NotesBox.Text))
                {
                    reminder.ReminderNotes = "Notes: None";
                }
                else
                {
                    reminder.ReminderNotes = NotesBox.Text;
                }

                var reminderDate = CLDatePicker.Date;

                if (reminderDate.HasValue)
                {
                    DateTime date = reminderDate.Value.DateTime;
                    string formattedDate = date.ToString("MM/dd/yyyy");
                    reminder.ReminderDate = formattedDate;

                    var notificationDate = CLDatePicker.Date.Value.DateTime;
                    reminder.ReminderNotification = notificationDate;
                }
                else
                {
                    reminder.ReminderDate = "Date: None";
                    reminder.ReminderNotification = DateTime.Now;
                }

                var timeHours = CLTimePicker.Time.Hours;
                var timeMinutes = CLTimePicker.Time.Minutes;
                string amPm;
                string formattedHours;
                string formattedMinutes;
                string formattedTime;

                if (timeHours > 12)
                {
                    timeHours -= 12;
                    amPm = "PM";
                }
                else
                {
                    amPm = "AM";
                }

                formattedHours = timeHours.ToString();

                if (timeMinutes < 10)
                {
                    formattedMinutes = timeMinutes.ToString();
                    formattedMinutes = "0" + formattedMinutes;
                }
                else
                {
                    formattedMinutes = timeMinutes.ToString();
                }

                if (timeHours == 0 && timeMinutes == 0)
                {
                    formattedTime = "Time: None";
                }
                else
                {
                    formattedTime = formattedHours + ":" + formattedMinutes + " " + amPm;
                }

                reminder.ReminderTime = formattedTime;

                ToastVisual visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = reminder.ReminderName
                            },
                            new AdaptiveText()
                            {
                                Text = reminder.ReminderLocation
                            },
                            new AdaptiveText()
                            {
                                Text = reminder.ReminderNotes
                            }
                        }
                    }
                };

                ToastContent toastContent = new ToastContent()
                {
                    Visual = visual
                };

                var toast = new ToastNotification(toastContent.GetXml());
                toast.ExpirationTime = DateTime.Now.AddSeconds(5);

                if (CLDatePicker.Date.HasValue)
                {
                    if (CLDatePicker.Date.Value.Date == DateTime.Today.Date)
                    {
                        if (CLTimePicker.Time.Hours <= DateTime.Now.Hour && CLTimePicker.Time.Minutes <= DateTime.Now.Minute)
                        {
                            ToastNotificationManager.CreateToastNotifier().Show(toast);
                        }
                        else
                        {
                            var current = DateTime.Now;
                            var minutesCounter = CLTimePicker.Time.Minutes - DateTime.Now.Minute;
                            var hoursCounter = CLTimePicker.Time.Hours - DateTime.Now.Hour;

                            int intMinutes = Convert.ToInt32(minutesCounter);
                            int intHours = Convert.ToInt32(hoursCounter);
                            TimeSpan timeSpan = new TimeSpan(0, intHours, intMinutes, 0);

                            DateTimeOffset scheduledTime = current + timeSpan;
                            var toast1 = new ScheduledToastNotification(toastContent.GetXml(), scheduledTime);
                            ToastNotificationManager.CreateToastNotifier().AddToSchedule(toast1);
                        }
                    }
                    else if (CLDatePicker.Date.Value.Date < DateTime.Today.Date)
                    {
                        ToastNotificationManager.CreateToastNotifier().Show(toast);
                    }
                    else
                    {
                        var current = DateTime.Now;
                        var daysCounter = CLTimePicker.Time.Days - DateTime.Now.Day;
                        var minutesCounter = CLTimePicker.Time.Minutes - DateTime.Now.Minute;
                        var hoursCounter = CLTimePicker.Time.Hours - DateTime.Now.Hour;

                        int intDays = Convert.ToInt32(daysCounter);
                        int intMinutes = Convert.ToInt32(minutesCounter);
                        int intHours = Convert.ToInt32(hoursCounter);
                        TimeSpan timeSpan = new TimeSpan(intDays, intHours, intMinutes, 0);

                        DateTimeOffset scheduledTime = current + timeSpan;
                        var toast1 = new ScheduledToastNotification(toastContent.GetXml(), scheduledTime);
                        ToastNotificationManager.CreateToastNotifier().AddToSchedule(toast1);
                    }
                }

                RemindersList.Add(reminder);

                foreach (Reminder selectedItem in CLListView.SelectedItems)
                {
                    RemindersList.Remove(selectedItem);
                }
            }
            else
            {
                Reminder reminder = new Reminder();

                if (String.IsNullOrWhiteSpace(NameBox.Text))
                {
                    reminder.ReminderName = "Untitled Reminder";
                }
                else
                {
                    reminder.ReminderName = NameBox.Text;
                }

                if (String.IsNullOrWhiteSpace(LocationBox.Text))
                {
                    reminder.ReminderLocation = "Location: None";
                }
                else
                {
                    reminder.ReminderLocation = LocationBox.Text;
                }

                if (String.IsNullOrWhiteSpace(NotesBox.Text))
                {
                    reminder.ReminderNotes = "Notes: None";
                }
                else
                {
                    reminder.ReminderNotes = NotesBox.Text;
                }

                var reminderDate = CLDatePicker.Date;

                if (reminderDate.HasValue)
                {
                    DateTime date = reminderDate.Value.DateTime;
                    string formattedDate = date.ToString("MM/dd/yyyy");
                    reminder.ReminderDate = formattedDate;

                    var notificationDate = CLDatePicker.Date.Value.DateTime;
                    reminder.ReminderNotification = notificationDate;
                }
                else
                {
                    reminder.ReminderDate = "Date: None";
                    reminder.ReminderNotification = DateTime.Now;
                }

                var timeHours = CLTimePicker.Time.Hours;
                var timeMinutes = CLTimePicker.Time.Minutes;
                string amPm;
                string formattedHours;
                string formattedMinutes;
                string formattedTime;

                if (timeHours > 12)
                {
                    timeHours -= 12;
                    amPm = "PM";
                }
                else
                {
                    amPm = "AM";
                }

                formattedHours = timeHours.ToString();

                if (timeMinutes < 10)
                {
                    formattedMinutes = timeMinutes.ToString();
                    formattedMinutes = "0" + formattedMinutes;
                }
                else
                {
                    formattedMinutes = timeMinutes.ToString();
                }

                if (timeHours == 0 && timeMinutes == 0)
                {
                    formattedTime = "Time: None";
                }
                else
                {
                    formattedTime = formattedHours + ":" + formattedMinutes + " " + amPm;
                }

                reminder.ReminderTime = formattedTime;

                ToastVisual visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = reminder.ReminderName
                            },
                            new AdaptiveText()
                            {
                                Text = reminder.ReminderLocation
                            },
                            new AdaptiveText()
                            {
                                Text = reminder.ReminderNotes
                            }
                        }
                    }
                };

                ToastContent toastContent = new ToastContent()
                {
                    Visual = visual
                };

                var toast = new ToastNotification(toastContent.GetXml());
                toast.ExpirationTime = DateTime.Now.AddSeconds(5);

                if (CLDatePicker.Date.HasValue)
                {
                    if (CLDatePicker.Date.Value.Date == DateTime.Today.Date)
                    {
                        if (CLTimePicker.Time.Hours <= DateTime.Now.Hour && CLTimePicker.Time.Minutes <= DateTime.Now.Minute)
                        {
                            ToastNotificationManager.CreateToastNotifier().Show(toast);
                        }
                        else
                        {
                            var current = DateTime.Now;
                            var minutesCounter = CLTimePicker.Time.Minutes - DateTime.Now.Minute;
                            var hoursCounter = CLTimePicker.Time.Hours - DateTime.Now.Hour;

                            int intMinutes = Convert.ToInt32(minutesCounter);
                            int intHours = Convert.ToInt32(hoursCounter);
                            TimeSpan timeSpan = new TimeSpan(0, intHours, intMinutes, 0);

                            DateTimeOffset scheduledTime = current + timeSpan;
                            var toast1 = new ScheduledToastNotification(toastContent.GetXml(), scheduledTime);
                            ToastNotificationManager.CreateToastNotifier().AddToSchedule(toast1);
                        }
                    }
                    else if (CLDatePicker.Date.Value.Date < DateTime.Today.Date)
                    {
                        ToastNotificationManager.CreateToastNotifier().Show(toast);
                    }
                    else
                    {
                        var current = DateTime.Now;
                        var daysCounter = CLTimePicker.Time.Days - DateTime.Now.Day;
                        var minutesCounter = CLTimePicker.Time.Minutes - DateTime.Now.Minute;
                        var hoursCounter = CLTimePicker.Time.Hours - DateTime.Now.Hour;

                        int intDays = Convert.ToInt32(daysCounter);
                        int intMinutes = Convert.ToInt32(minutesCounter);
                        int intHours = Convert.ToInt32(hoursCounter);
                        TimeSpan timeSpan = new TimeSpan(intDays, intHours, intMinutes, 0);

                        DateTimeOffset scheduledTime = current + timeSpan;
                        var toast1 = new ScheduledToastNotification(toastContent.GetXml(), scheduledTime);
                        ToastNotificationManager.CreateToastNotifier().AddToSchedule(toast1);
                    }
                }
                RemindersList.Add(reminder);
            }

            NameBox.Text = "";
            LocationBox.Text = "";
            NotesBox.Text = "";
            CLDatePicker.Date = null;
            CLTimePicker.Time = TimeSpan.Zero;

            var localfolder = ApplicationData.Current.LocalFolder;
            var file = await localfolder.CreateFileAsync("collection.json", CreationCollisionOption.ReplaceExisting);
            using (var stream = await file.OpenStreamForWriteAsync())
            using (var writer = new StreamWriter(stream, Encoding.UTF8))
            {
                string json = JsonConvert.SerializeObject(RemindersList);
                await writer.WriteAsync(json);
            }

            ReminderView.IsPaneOpen = false;
        }
    }
}