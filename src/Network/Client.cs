using System;
using System.Net;
using System.Net.Sockets;

namespace ChessDemo.Network
{
    class Client
    {
        public event EventHandler Connected;

        private int _port;
        private TcpClient _client;

        private bool _Connected;
        /// <summary>
        /// Represents the connected state for the client.
        /// Raises the Connected event if connection changes to true.
        /// </summary>
        public bool IsConnected {
            get
            {
                return _Connected;
            }
            private set
            {
                if (value != _Connected)
                {
                    _Connected = value;

                    if (_Connected)
                    {
                        // Keep a copy in case someone unsubscribes while doing a callback
                        EventHandler handler = Connected;

                        if (handler != null)
                        {
                            handler(this, null);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// TCP Client to connect to the server
        /// </summary>
        /// <param name="port"></param>
        internal Client(int port)
        {
            _port = port;
        }

        /// <summary>
        /// Start the client
        /// </summary>
        internal void Start()
        {
            _client = new TcpClient();

            // For now, hardcode to the loopback
            _client.BeginConnect(IPAddress.Loopback, _port, new AsyncCallback(ConnectionCallback), this);
        }

        /// <summary>
        /// Close the tcp connection
        /// </summary>
        internal void Stop()
        {
            try
            {
                _client.Close();
            }
            catch { }
            finally
            {
                IsConnected = false;
            }
        }

        /// <summary>
        /// Static callback to handle the connection callback on a client
        /// </summary>
        /// <param name="ar"></param>
        private static void ConnectionCallback(IAsyncResult ar)
        {
            Client client = ar.AsyncState as Client;

            if (client != null)
            {
                client._client.EndConnect(ar);
                client.IsConnected = true;
            }
        }
    }
}
