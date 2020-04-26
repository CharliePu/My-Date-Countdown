﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
            storage = new MyStorage();
            LoadDataFromStorage();
            SetDisplay();
        }

        private void SetDisplay()
        {
            TextBlockTitle.Text = title;
            int DateDifference = (int)(targetDate - DateTime.Today).TotalDays;
            TextBlockDays.Text = DateDifference.ToString() + " days left";
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

        private void ButtonSet_Click(object sender, RoutedEventArgs e)
        {
            targetDate = DatePickerTargetDate.Date;
            title = TextBoxTitle.Text;
            SetDisplay();
            StoreData();
        }
    }
}
