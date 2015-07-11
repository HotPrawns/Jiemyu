using JiemyuDll.Network;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Jiemyu.Network
{
    /// <summary>
    /// Network manager works to queue network messages in a thread safe way
    /// using the TaskQueue
    /// </summary>
    class NetworkManager
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        private static NetworkManager _instance = null;

        /// <summary>
        /// Default TaskQueue with 1 consumer thread.
        /// </summary>
        private TaskQueue<Message> _Q = new TaskQueue<Message>(1);

        private Client _client = new Client(8800); // TODO: Client(JiemyuDll.Properties.Settings.Default.Port);

        /// <summary>
        /// 
        /// </summary>
        private NetworkManager()
        {
            // Sets up the callback for the TaskQueue
            _client.Connected += Client_connect;
            _client.Start();
        }

        private void Client_connect(object sender, EventArgs e)
        {
            // All work will happen on the same thread, so sending messages is thread safe.
            _Q.WorkAvailable += (object _QSender, EventArgsWrapper<Message> data) =>
            {
                Message message = data.Data;
                Debug.Assert(_client.IsConnected);
                _client.SendMessage(message);
            };
        }


        /// <summary>
        /// Gets the singleton instance of the NetworkManager
        /// </summary>
        /// <returns></returns>
        public static NetworkManager Instance()
        {
            if (_instance == null)
            {
                _instance = new NetworkManager();
            }

            return _instance;
        }

        /// <summary>
        /// Enqueues a message to be sent to the server
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(Message message)
        {
            _Q.EnqueTask(message);
        }
    }
}
