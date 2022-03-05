using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Notifications;
using Windows.UI.Notifications.Management;
using Windows.UI.Popups;

namespace NotificationListenerForVRC
{
    public class NotificationHelper
    {
        /// <summary>
        /// 新着通知を取得する
        /// </summary>
        public async void SyncNotifications()
        {
            // 通知リスナーへのアクセスを要求する
            UserNotificationListener listener = await RequestAccessToUserNotificationListener();

            // 現在の通知一覧を取得
            IReadOnlyList<UserNotification> userNotifications = await listener.GetNotificationsAsync(NotificationKinds.Toast);

            // 通知済みのIDを取得
            List<uint> AlreadyNotifiedIds = GetAlreadyNotifiedIds();

            Debug.WriteLine("---");
            foreach (UserNotification notification in userNotifications)
            {
                // 新着の通知だったら
                if (!AlreadyNotifiedIds.Contains(notification.Id))
                {
                    // VRCに通知タイトルを送信する
                    SendTextToVRC(notification.AppInfo.DisplayInfo.DisplayName);
                }
            }

            // ViewModel の通知一覧を更新する
            App.vm.UserNotifications = userNotifications;
        }

        /// <summary>
        /// 通知リスナーへのアクセスを要求する
        /// </summary>
        /// <returns>通知リスナー</returns>
        private async Task<UserNotificationListener> RequestAccessToUserNotificationListener()
        {
            UserNotificationListener listener = UserNotificationListener.Current;
            App.vm.AccessStatus = await listener.RequestAccessAsync();

            switch (App.vm.AccessStatus)
            {
                case UserNotificationListenerAccessStatus.Allowed:
                    break;
                case UserNotificationListenerAccessStatus.Denied:
                    _ = await new MessageDialog("通知を許可してください").ShowAsync();
                    _ = await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings:appsfeatures-app"));
                    CoreApplication.Exit();
                    break;
                case UserNotificationListenerAccessStatus.Unspecified:
                    _ = await new MessageDialog("通知の許可状態が取得できませんでした").ShowAsync();
                    _ = await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings:appsfeatures-app"));
                    CoreApplication.Exit();
                    break;
            }

            return listener;
        }

        /// <summary>
        /// 通知済みのIDを取得
        /// </summary>
        /// <returns>通知済みのID</returns>
        private List<uint> GetAlreadyNotifiedIds()
        {
            List<uint> ids = new();
            if (!(App.vm.UserNotifications is null))
            {
                foreach (UserNotification notification in App.vm.UserNotifications)
                {
                    ids.Add(notification.Id);
                }
            }

            return ids;
        }

        /// <summary>
        /// VRCに通知を送信する
        /// </summary>
        private async void SendTextToVRC(string text)
        {
            using OscHelper osc = new("127.0.0.1", 9001, 9000);

            // JIS基本漢字の範囲のみ
            string str = CSharp.Japanese.Kanaxs.Kana.ToZenkaku(text);

            // 区点位置を知りたいので文字列を ISO-2022-JP でバイト配列に変換
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            byte[] raw = System.Text.Encoding.GetEncoding("iso-2022-jp").GetBytes(str);

            // 先頭3バイトと末尾3バイトはエスケープシーケンスなので捨てる
            byte[] bytes = raw.Skip(3).SkipLast(3).ToArray();

            // 1文字(2バイト)ごとにループ
            for (int i = 0; i < bytes.Length; i += 2)
            {
                // 上位バイトから32を引いたものが区
                int row = bytes[i] - 32;
                Debug.WriteLine("区:" + row);

                // 下位バイトから32を引いたものが点
                int cell = bytes[i + 1] - 32;
                Debug.WriteLine("点:" + cell);

                // テスクチャのオフセット（X座標）を motion time で指定するパラメタ
                // アニメーションの1-100フレームにオフセットを記録しているため、点*0.01 となる
                float posX = (float)decimal.Divide(cell, 100m);
                Debug.WriteLine("X座標:" + posX);

                // 文字が表示されないよう端まで移動 => 区に移動 => 点に移動
                osc.Send("/avatar/parameters/posX", 0.99f);
                await Task.Delay(200);
                osc.Send("/avatar/parameters/row", (float)row);
                await Task.Delay(1500);
                osc.Send("/avatar/parameters/posX", posX);
                await Task.Delay(1500);
            }
        }
    }
}
