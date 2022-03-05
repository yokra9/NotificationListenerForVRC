using Windows.UI.Xaml.Controls;

namespace NotificationListenerForVRC
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            DataContext = App.vm;
            InitializeComponent();
        }

        /// <summary>
        /// ページ読込時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // バックグラウンドタスクを登録する
            new BackgroundTaskHelper().RegisterBackgroundTask();

            // 新着通知を取得する
            NotificationHelper notificationHelper = new();
            notificationHelper.SyncNotifications();
        }
    }
}
