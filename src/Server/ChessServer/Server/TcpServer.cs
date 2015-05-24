using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using ChessDemo.Network;

namespace ChessServer.Server
{
    class TcpServer
    {
        private int _port;

        public bool Running { get; private set; }

        internal TcpServer(int port)
        {
            _port = port;
        }

        /// <summary>
        /// Starts up the server and begins listening for connections
        /// </summary>
        internal async void Start()
        {
            IPAddress ipAddre = IPAddress.Loopback;
            TcpListener listener = new TcpListener(ipAddre, _port);
            listener.Start();
            LogMessage("Server is running");
            LogMessage("Listening on port " + _port);

            this.Running = true;

            while (true)
            {
                LogMessage("Waiting for connections...");
                try
                {
                    var tcpClient = await listener.AcceptTcpClientAsync();
                    HandleConnectionAsync(tcpClient);
                }
                catch (Exception exp)
                {
                    LogMessage(exp.ToString());
                }

            }
        }

        /// <summary>
        /// Callback function to hanle a TcpClient connecting
        /// </summary>
        /// <param name="tcpClient"></param>
        private async void HandleConnectionAsync(TcpClient tcpClient)
        {
            string clientInfo = tcpClient.Client.RemoteEndPoint.ToString();
            LogMessage(string.Format("Got connection request from {0}", clientInfo));

            await Task.Run(() =>
            {
                var stream = tcpClient.GetStream();

                try
                {
                    while (stream.CanRead)
                    {
                        Message message = MessageUtil.ReadMessage(stream);
                        LogMessage(string.Format("Received message {0} from {1}", message, clientInfo));
                    }
                }
                catch (Exception) { }
                finally
                {
                    stream.Close();
                    LogMessage(string.Format("Closing the client connection - {0}", clientInfo));
                }
            });
        }

        /// <summary>
        /// Logs a message to the console
        /// </summary>
        /// <param name="message"></param>
        /// <param name="callername"></param>
        private void LogMessage(string message, [CallerMemberName]string callername = "")
        {
            System.Console.WriteLine("[{0}] - Thread-{1}- {2}",
                    callername, Thread.CurrentThread.ManagedThreadId, message);
        }
    }
}
