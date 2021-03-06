﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.ViewManagement;

namespace My_Date_Countdown
{
    class AppUtility
    {
        public AppUtility setMinWindowSize(double width, double height)
        {
            ApplicationView.GetForCurrentView().SetPreferredMinSize(
                new Size(
                    width,
                    height
                    )
                );

            return this;
        }

        public AppUtility setTitleBarTranslucent(bool state)
        {
            ApplicationViewTitleBar formattableTitleBar = ApplicationView.GetForCurrentView().TitleBar;
            formattableTitleBar.ButtonBackgroundColor = Colors.Transparent;
            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = state;

            return this;
        }


        // Make sure to declare in appmanifest
        // https://blogs.windows.com/windowsdeveloper/2017/08/01/configure-app-start-log/
        public async Task<bool> requestStartupTaskAsync(string startupId)
        {
            StartupTask startupTask = await StartupTask.GetAsync(startupId);
            if (startupTask.State != StartupTaskState.Enabled)
            {
                await startupTask.RequestEnableAsync();
                if (startupTask.State != StartupTaskState.Enabled)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
