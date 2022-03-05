using System;
using System.Linq;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;

namespace NotificationListenerForVRC
{
    public class BackgroundTaskHelper
    {

        /// <summary>
        /// バックグラウンドタスクを登録する
        /// </summary>
        public async void RegisterBackgroundTask()
        {
            App.vm.BackgroundAccessStatus = await BackgroundExecutionManager.RequestAccessAsync();

            // 通知に変化があった時にトリガーする
            if (!BackgroundTaskRegistration.AllTasks.Any(i => i.Value.Name.Equals("UserNotificationChanged")))
            {
                BackgroundTaskBuilder builder = new() { Name = "UserNotificationChanged" };
                builder.SetTrigger(new UserNotificationChangedTrigger(NotificationKinds.Toast));
                builder.Register();
            }
        }
    }
}
