using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Threading.Tasks;
using Windows.UI.Notifications;


namespace My_Date_Countdown
{
    // Make sure to install Microsoft.Toolkit.Uwp.Notifications on NuGet
    class MyNotification
    {
        const string tag = "tag";


        public MyNotification()
        {
        }

        public MyNotification ToastNotify(string title = "", string content = "", string tag = tag)
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

        public void ClearTile()
        {
            TileUpdateManager.CreateTileUpdaterForApplication().Clear();
        }
        private async Task<bool> CheckTileSupport()
        {
            // Check for pin-to-start support
            // https://docs.microsoft.com/en-us/windows/uwp/design/shell/tiles-and-notifications/primary-tile-apis
            Windows.ApplicationModel.Core.AppListEntry entry = (await Windows.ApplicationModel.Package.Current.GetAppListEntriesAsync())[0];
            bool isSupported = Windows.UI.StartScreen.StartScreenManager.GetDefault().SupportsAppListEntry(entry);
            return isSupported;
        }

        public async Task<bool> CheckTileExist()
        {
            //https://docs.microsoft.com/en-us/windows/uwp/design/shell/tiles-and-notifications/primary-tile-apis#check-whether-youre-currently-pinned
            // Get your own app list entry
            Windows.ApplicationModel.Core.AppListEntry entry = (await Windows.ApplicationModel.Package.Current.GetAppListEntriesAsync())[0];

            // Check if your app is currently pinned
            bool isPinned = await Windows.UI.StartScreen.StartScreenManager.GetDefault().ContainsAppListEntryAsync(entry);

            return isPinned;
        }

        public async void PinTile()
        {
            // https://docs.microsoft.com/en-us/windows/uwp/design/shell/tiles-and-notifications/primary-tile-apis#pin-your-primary-tile
            // Get your own app list entry
            Windows.ApplicationModel.Core.AppListEntry entry = (await Windows.ApplicationModel.Package.Current.GetAppListEntriesAsync())[0];

            // And pin it to Start
            await Windows.UI.StartScreen.StartScreenManager.GetDefault().RequestAddAppListEntryAsync(entry);
        }

        public async Task<bool> CheckToastPermission()
        {
            return await new AppUtility().requestStartupTaskAsync("DateCountdownStartupId");
        }

        public MyNotification TileNotify(string title = "", string content = "")
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
