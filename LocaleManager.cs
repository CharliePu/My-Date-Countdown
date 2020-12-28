using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Date_Countdown
{
    class LocaleManager
    {
        Windows.ApplicationModel.Resources.ResourceLoader ResourceLoader;
        public LocaleManager(string RESWName = "Resources")
        {
            if (RESWName == "Resources")
            {
                ResourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
            }
            else
            {
                ResourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView(RESWName);
            }
        }

        public string GetString(string key)
        {
            return ResourceLoader.GetString(key);
        }

        public string GetString(string key, string f1)
        {
            return String.Format(ResourceLoader.GetString(key), f1);
        }

        public string GetString(string key, string f1, string f2)
        {
            return String.Format(ResourceLoader.GetString(key), f1, f2);
        }
    }
}
