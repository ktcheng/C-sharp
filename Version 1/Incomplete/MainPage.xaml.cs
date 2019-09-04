using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Reminders
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //public ObservableCollection<Reminder> Reminders;

        private ObservableCollection<Reminder> Reminders { get; set; } = new ObservableCollection<Reminder>();
        public MainPage()
        {
            //Reminders = new ObservableCollection<Reminder>();

            this.InitializeComponent();
        }

        private void AddNew_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BlankPage1), null);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var displayReminders = e.Parameter as Reminder;

            

            if (displayReminders != null)
            {
                //Reminders.Application.ReminderList.Add(displayReminders);

                Reminders.Add(displayReminders);

                if (RemindersListView.SelectedItems.Count == 1)
                {
                    foreach (Reminder selectedItem in RemindersListView.SelectedItems)
                    {
                        Reminders.Remove(selectedItem);
                    }
                }
            }


        }

        private void DeleteReminderButton_Click(object sender, RoutedEventArgs e)
        {
            if (Reminders == null || !Reminders.Any())
            {  
            }
            else
            {
                //for (int i = RemindersListView.SelectedItems.Count; i > 0; i--)
                //{
                //    foreach (Reminder selectedItem in RemindersListView.SelectedItems)
                //    {
                //        Reminders.Remove(selectedItem);
                //    }
                //}

                foreach (Reminder selectedItem in RemindersListView.SelectedItems)
                {
                    Reminders.Remove(selectedItem);
                }
            }
        }

        private void SelectAllButton_Click(object sender, RoutedEventArgs e)
        {
            RemindersListView.SelectAll();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (RemindersListView.SelectedItems.Count == 1)
            {
                foreach (Reminder selectedItem in RemindersListView.SelectedItems)
                {
                    this.Frame.Navigate(typeof(BlankPage1), selectedItem);
                }

                //Reminders.Remove(RemindersListView.SelectedItem);
            }
        }
    }
}
