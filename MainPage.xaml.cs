using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace My_Date_Countdown
{
    public sealed partial class MainPage : Page
    {
        private DateTimeOffset targetDate;
        private string title;
        private MyStorage storage;

        public MainPage()
        {
            this.InitializeComponent();

            new AppUtility().setMinWindowSize(450, 500).setTitleBarTranslucent(true);
            storage = new MyStorage();
            LoadDataFromStorage();
            SetDisplay();
        }

        private void SetDisplay()
        {
            TextBlockTitle.Text = title;
            int DateDifference = (int)(targetDate - DateTime.Today).TotalDays;
            TextBlockDays.Text = DateDifference.ToString() + " days left";
            TextBoxTitle.Text = title;
            DatePickerTargetDate.Date = targetDate;
        }

        private void LoadDataFromStorage()
        {
            if (storage.Get("title") != null)
            {
                title = (string)storage.Get("title");
            }
            else
            {
                title = "";
            }
            if (storage.Get("targetDate") != null)
            {
                targetDate = (DateTimeOffset)storage.Get("targetDate");
            }
            else
            {
                targetDate = DateTimeOffset.Now;
            }
        }

        private void StoreData()
        {
            storage.Store("title", title);
            storage.Store("targetDate", targetDate);
        }

        private async void ButtonSet_Click(object sender, RoutedEventArgs e)
        {
            targetDate = DatePickerTargetDate.Date;
            title = TextBoxTitle.Text;
            SetDisplay();
            StoreData();
            bool state = await new AppUtility().requestStartupTaskAsync("DateCountdownStartupId");
            if (state == true)
            {
                new MyNotification("Success!", "Countdown " + title + " will show on startup.").Notify();
            }
            else
            {
                new MyNotification("Failed to add startup", "Make sure to enable it on Task Manager by yourself.").Notify();
            }
        }

        public void DoStartupTask()
        {
            LoadDataFromStorage();
            int DateDifference = (int)(targetDate - DateTime.Today).TotalDays;
            new MyNotification(DateDifference + " days left", title).Notify();
        }

        private void updateButtonStatus()
        {
            if (DatePickerTargetDate.Date >= DateTimeOffset.Now &&
                TextBoxTitle.Text != "")
            {
                ButtonSet.IsEnabled = true;
            }
            else
            {
                ButtonSet.IsEnabled = false;
            }
        }

        private void DatePickerTargetDate_SelectedDateChanged(DatePicker sender, DatePickerSelectedValueChangedEventArgs args)
        {
            updateButtonStatus();
        }

        private void TextBoxTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            updateButtonStatus();
        }
    }
}
