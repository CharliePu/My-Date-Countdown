using System;
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
        private String title;

        public MainPage()
        {
            this.InitializeComponent();

        }

        private void ButtonSet_Click(object sender, RoutedEventArgs e)
        {
            targetDate = DatePickerTargetDate.Date;
            title = TextBlockTitle.Text;

            TextBlockTitle.Text = title;
            int DateDifference = (int)(targetDate - DateTime.Today).TotalDays;
            TextBlockDays.Text = DateDifference.ToString() + " days left";
        }
    }
}
