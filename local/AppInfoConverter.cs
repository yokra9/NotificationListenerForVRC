using System;
using Windows.ApplicationModel;
using Windows.UI.Xaml.Data;

namespace NotificationListenerForVRC
{
    public class AppInfoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            AppInfo appInfo = value as AppInfo;
            return appInfo.DisplayInfo.DisplayName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
