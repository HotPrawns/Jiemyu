using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChessServer.Server
{
    class Server
    {
        private int _port;

        internal Server(int port)
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
            try
            {
                using (var networkStream = tcpClient.GetStream())
                using (var reader = new StreamReader(networkStream))
                using (var writer = new StreamWriter(networkStream))
                {
                    writer.AutoFlush = true;
                    while (true)
                    {
                        var dataFromServer = await reader.ReadLineAsync();
                        if (string.IsNullOrEmpty(dataFromServer))
                        {
                            break;
                        }
                        LogMessage(dataFromServer);
                        await writer.WriteLineAsync("FromServer-" + dataFromServer);
                    }
                }
            }
            catch (Exception exp)
            {
                LogMessage(exp.Message);
            }
            finally
            {
                LogMessage(string.Format("Closing the client connection - {0}",
                            clientInfo));
                tcpClient.Close();
            }

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
}
