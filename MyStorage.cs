namespace My_Date_Countdown
{
    class MyStorage
    {
        private readonly Windows.Storage.ApplicationDataContainer localSettings;

        public MyStorage()
        {
            localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        }

        public void Store(string key, object value)
        {
            localSettings.Values[key] = value;
        }

        public object Get(string key)
        {
            return localSettings.Values[key];
        }
    }
}
