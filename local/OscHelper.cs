using Rug.Osc;
using System;
using System.Net;

namespace NotificationListenerForVRC
{
    public class OscHelper : IDisposable
    {
        private readonly OscSender sender;

        /// <param name="ip">IPアドレス</param>
        /// <param name="localPort">ローカル側（本アプリ）のポート</param>
        /// <param name="remotePort">リモート側（VRChat）のポート</param>
        public OscHelper(string ip, int localPort, int remotePort)
        {
            sender = new OscSender(IPAddress.Parse(ip), localPort, remotePort);
            sender.Connect();
        }

        /// <summary>
        /// ソケットを閉じ、リソースを破棄する
        /// </summary>
        public void Dispose()
        {
            sender.Dispose();
        }

        /// <summary>
        /// OSCメッセージを送信する
        /// </summary>
        /// <param name="address">アドレス</param>
        /// <param name="args">パラメタ</param>
        public void Send(string address, params object[] args)
        {
            sender.Send(new OscMessage(address, args));
        }
    }
}
