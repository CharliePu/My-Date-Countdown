using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.Notifications;


namespace My_Date_Countdown
{
    // Make sure to install Microsoft.Toolkit.Uwp.Notifications on NuGet
    class MyNotification
    {
        string title, content;
        const string tag = "tag";


        public MyNotification(string title, string content = "")
        {
            this.title = title;
            this.content = content;
        }

        public MyNotification ToastNotify(string tag = tag)
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

            var toast = new ToastNotification(toastContent.GetXml())
            {
                Tag = tag
            };

            ToastNotificationManager.CreateToastNotifier().Show(toast);

            return this;
        }

        public MyNotification TileNotify()
        {
            // Construct the tile content
            TileContent tileContent = new TileContent()
            {
                Visual = new TileVisual()
                {
                    TileMedium = new TileBinding()
                    {
                        Content = new TileBindingContentAdaptive()
                        {
                            Children =
                            {
                                new AdaptiveText()
                                {
                                    Text = title,
                                },

                                new AdaptiveText()
                                {
                                    Text = content,
                                    HintStyle = AdaptiveTextStyle.CaptionSubtle
                                }
                            }
                        }
                    },

                    TileWide = new TileBinding()
                    {
                        Content = new TileBindingContentAdaptive()
                        {
                            Children =
                            {
                                new AdaptiveText()
                                {
                                    Text = title,
                                    HintStyle = AdaptiveTextStyle.Title
                                },

                                new AdaptiveText()
                                {
                                    Text = content,
                                    HintStyle = AdaptiveTextStyle.Body
                                }
                            }
                        }
                    }
                }
            };

            var notification = new TileNotification(tileContent.GetXml());

            TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);

            return this;
        }
    }
}
