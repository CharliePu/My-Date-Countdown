using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.StartScreen;
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
        private MyNotification notification;
        private LocaleManager localeManager;
        private int DateDifference;
        private bool toastEnabled, tileEnabled, newToastEnabled, newTileEnabled;

        public MainPage()
        {
            this.InitializeComponent();

            new AppUtility().setMinWindowSize(450, 500).setTitleBarTranslucent(true);
            storage = new MyStorage();
            localeManager = new LocaleManager();
            notification = new MyNotification();

            LoadData();
            SetDisplay();
        }

        private void SetDisplay()
        {
            DateDifference = (int)(targetDate - DateTime.Today).TotalDays;

            TextBlockTitle.Text = title;
            TextBlockDays.Text = localeManager.GetString("DaysLeft/Text", DateDifference.ToString());
            TextBoxTitle.Text = title;
            DatePickerTargetDate.Date = targetDate;
            DatePickerTargetDate.MinDate = DateTime.Today;

            ToolTipService.SetToolTip(TileButton, 
                new ToolTip {Content = localeManager.GetString("TileButton/Tooltip")});

            ToolTipService.SetToolTip(NotifyButton,
                new ToolTip { Content = localeManager.GetString("ToastButton/Tooltip") });

            UpdateButtonStatus();
        }

        private void LoadData()
        {
            title = (string)(storage.Get("title") ?? "");
            targetDate = (DateTimeOffset)(storage.Get("targetDate") ?? DateTimeOffset.Now);
            tileEnabled = (bool) (storage.Get("tileEnabled") ?? false);
            toastEnabled = (bool)(storage.Get("toastEnabled") ?? false);

            newTileEnabled = tileEnabled;
            newToastEnabled = toastEnabled;
        }

        private void StoreData()
        {
            storage.Store("title", title);
            storage.Store("targetDate", targetDate);
            storage.Store("tileEnabled", tileEnabled);
            storage.Store("toastEnabled", toastEnabled);
        }

        private async void ButtonSet_Click(object sender, RoutedEventArgs e)
        {
            targetDate = DatePickerTargetDate.Date ?? DateTime.Now;
            title = TextBoxTitle.Text;
            tileEnabled = newTileEnabled;
            toastEnabled = newToastEnabled;
            SetDisplay();
            StoreData();

            if (tileEnabled && await notification.CheckToastPermission())
            {
                if (!await notification.CheckTileExist())
                {
                    notification.PinTile();
                }
                notification.TileNotify(
                    localeManager.GetString("DaysLeft/Text", DateDifference.ToString()),
                    title);
            }
            else
            {
                notification.ClearTile();
            }

            if (toastEnabled && await notification.CheckToastPermission())
            {
                notification.ToastNotify(
                localeManager.GetString("SuccessNotification/Title"),
                localeManager.GetString("SuccessNotification/Content", title));
            }
            else
            {
                if (toastEnabled)
                {
                    newToastEnabled = false;
                    toastEnabled = false;
                    UpdateButtonStatus();
                    notification.ToastNotify(
                    localeManager.GetString("CreateStartupTaskFailedNotification/Title"),
                    localeManager.GetString("CreateStartupTaskFailedNotification/Content"));
                }
            }
        }

        public void DoStartupTask()
        {
            LoadData();
            DateDifference = (int)(targetDate - DateTime.Today).TotalDays;
            if (toastEnabled)
            {
                notification.ToastNotify(localeManager.GetString("DaysLeft/Text", DateDifference.ToString()), title);
            }
            if (tileEnabled)
            {
                notification.TileNotify(localeManager.GetString("DaysLeft/Text", DateDifference.ToString()), title);
            }
        }


        private void UpdateButtonStatus()
        {
            ButtonSet.IsEnabled =
                (DatePickerTargetDate.Date != targetDate ||
                TextBoxTitle.Text != title ||
                tileEnabled != newTileEnabled ||
                toastEnabled != newToastEnabled) &&
                DatePickerTargetDate.Date >= DateTimeOffset.Now &&
                TextBoxTitle.Text != "";
            TileButton.Foreground = new SolidColorBrush(newTileEnabled ?
                (Color)Application.Current.Resources["SystemBaseHighColor"] :
                (Color)Application.Current.Resources["SystemBaseMediumLowColor"]);
            NotifyButton.Foreground = new SolidColorBrush(newToastEnabled ? 
                (Color)Application.Current.Resources["SystemBaseHighColor"] :
                (Color)Application.Current.Resources["SystemBaseMediumLowColor"]);
        }

        private void TextBoxTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateButtonStatus();
        }

        private void DatePickerTargetDate_SelectedDateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            UpdateButtonStatus();
        }

        private async void TileButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!await notification.CheckTileExist())
            {
                notification.PinTile();
            }
            newTileEnabled = !newTileEnabled;
            UpdateButtonStatus();
        }

        private async void NotifyButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!await notification.CheckToastPermission())
            {
                notification.ToastNotify(
                localeManager.GetString("CreateStartupTaskFailedNotification/Title"),
                localeManager.GetString("CreateStartupTaskFailedNotification/Content"));
                newToastEnabled = false;
                toastEnabled = false;
            }
            else
            {
                newToastEnabled = !newToastEnabled;
            }
            UpdateButtonStatus();
        }
    }
}
