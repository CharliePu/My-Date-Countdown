using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playground
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
