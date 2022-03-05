using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;
using Windows.UI.Notifications.Management;

namespace NotificationListenerForVRC
{
    public class ViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 変更通知用イベントハンドラ
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// プロパティの変更を通知する
        /// </summary>
        /// <param name="name">プロパティ名</param>
        protected void OnPropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        /// <summary>
        /// 通知へのアクセス状態
        /// </summary>
        private UserNotificationListenerAccessStatus accessStatus;

        /// <summary>
        /// バックグラウンド処理へのアクセス状態
        /// </summary>
        private BackgroundAccessStatus backgroundAccessStatus;

        /// <summary>
        /// 通知一覧
        /// </summary>
        private IReadOnlyList<UserNotification> userNotifications;

        /// <summary>
        /// 通知へのアクセス状態
        /// </summary>
        public UserNotificationListenerAccessStatus AccessStatus { get => accessStatus; set { accessStatus = value; OnPropertyChanged("AccessStatus"); } }

        /// <summary>
        /// バックグラウンド処理へのアクセス状態
        /// </summary>
        public BackgroundAccessStatus BackgroundAccessStatus { get => backgroundAccessStatus; set { backgroundAccessStatus = value; OnPropertyChanged("BackgroundAccessStatus"); } }

        /// <summary>
        /// 通知一覧
        /// </summary>
        public IReadOnlyList<UserNotification> UserNotifications { get => userNotifications; set { userNotifications = value; OnPropertyChanged("UserNotifications"); } }
    }
}
