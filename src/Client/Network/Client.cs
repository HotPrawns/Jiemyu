using ChessDll.Network;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ChessDemo.Network
{
    public class MessageArgs : EventArgs
    {
        public Message ReceivedMessage { get; set; }
    }

    class Client
    {
        public event EventHandler Connected;
        public event EventHandler<MessageArgs> MessageReceived;

        private int _port;
        private TcpClient _client;

        Thread _receiveThread;

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
        /// 
        /// </summary>
        /// <param name="message"></param>
        internal void SendMessage(Message message)
        {
            byte[] bytes = Message.Serialize(message);

            var stream = _client.GetStream();

            if (stream.CanWrite)
            {
                // First four bytes contain size of message
                byte[] dataLen = BitConverter.GetBytes((Int32)bytes.Length);
                stream.Write(dataLen, 0, 4);

                // Follow up the length with the actual data
                stream.Write(bytes, 0, bytes.Length);
            }
            else
            {
                throw new Exception("Unable to send data");
            }
        }

        /// <summary>
        /// Initializes the thread to start reading data
        /// </summary>
        protected void StartReceivingMessages()
        {
            _receiveThread = new Thread(() =>
            {
                while (true)
                {
                    do
                    {
                        Message m = MessageUtil.ReadMessage(_client.GetStream());

                        MessageArgs mArgs = new MessageArgs();
                        mArgs.ReceivedMessage = m;

                        EventHandler<MessageArgs> handler = MessageReceived;
                        handler(this, mArgs);

                    } while (_client.GetStream().DataAvailable);

                    // Make sure to sleap inbetween reading when there is no data available
                    Thread.Sleep(50);
                }
            });

            _receiveThread.Start();
        }

        /// <summary>
        /// Interrupts the message receiving thread
        /// </summary>
        protected void StopReceivingMessages()
        {
            _receiveThread.Abort();
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
