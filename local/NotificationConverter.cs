using System;
using System.Collections.Generic;
using Windows.UI.Notifications;
using Windows.UI.Xaml.Data;

namespace NotificationListenerForVRC
{
    public class NotificationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Notification notification = value as Notification;
            IReadOnlyList<AdaptiveNotificationText> textElements = notification.Visual.GetBinding(KnownNotificationBindings.ToastGeneric).GetTextElements();
            string str = textElements[0].Text ?? "";

            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
