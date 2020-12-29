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
        private LocaleManager localeManager;

        public MainPage()
        {
            this.InitializeComponent();

            new AppUtility().setMinWindowSize(450, 500).setTitleBarTranslucent(true);
            storage = new MyStorage();
            localeManager = new LocaleManager();
            LoadDataFromStorage();
            SetDisplay();
        }

        private void SetDisplay()
        {
            int DateDifference = (int)(targetDate - DateTime.Today).TotalDays;

            TextBlockTitle.Text = title;
            TextBlockDays.Text = localeManager.GetString("DaysLeft/Text", DateDifference.ToString());
            TextBoxTitle.Text = title;
            DatePickerTargetDate.Date = targetDate;

            UpdateButtonStatus();
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

            bool isStartupEnabled = await new AppUtility().requestStartupTaskAsync("DateCountdownStartupId");
            if (isStartupEnabled)
            {
                new MyNotification(
                localeManager.GetString("SuccessNotification/Title"),
                localeManager.GetString("SuccessNotification/Content", title)).Notify();
            }
            else
            {
                new MyNotification(
                localeManager.GetString("FailedNotification/Title"),
                localeManager.GetString("FailedNotification/Content")).Notify();
            }
        }

        public void DoStartupTask()
        {
            LoadDataFromStorage();
            Notify();
        }

        private void Notify()
        {
            int DateDifference = (int)(targetDate - DateTime.Today).TotalDays;

            new MyNotification(
                localeManager.GetString("DaysLeft/Text", DateDifference.ToString()),
                title).Notify();
        }

        private void UpdateButtonStatus()
        {
            bool state;
            if (state = DatePickerTargetDate.Date != targetDate || TextBoxTitle.Text != title)
            {
                state = DatePickerTargetDate.Date >= DateTimeOffset.Now && TextBoxTitle.Text != "";
            }
            ButtonSet.IsEnabled = state; 
        }

        private void DatePickerTargetDate_SelectedDateChanged(DatePicker sender, DatePickerSelectedValueChangedEventArgs args)
        {
            UpdateButtonStatus();
        }

        private void TextBoxTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateButtonStatus();
        }
    }
}
