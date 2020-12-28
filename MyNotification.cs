using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.Notifications;
 

namespace My_Date_Countdown
{
    // Make sure to install Microsoft.Toolkit.Uwp.Notifications on NuGet
    class MyNotification
    {
        string title, content;
        public MyNotification(string title, string content = "")
        {
            this.title = title;
            this.content = content;
        }

        public void Notify()
        {
            ToastVisual visual = new ToastVisual()
            {
                BindingGeneric = new ToastBindingGeneric()
                {
                    Children =
                    {
                    new AdaptiveText()
                    {
                        Text = title
                    },

                    new AdaptiveText()
                    {
                        Text = content
                    },
                }
                }
            };

            ToastContent toastContent = new ToastContent()
            { Visual = visual };

            var toast = new ToastNotification(toastContent.GetXml());

            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}
