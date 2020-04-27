using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                    450,
                    500
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
    }
}
