using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace ChessDll.Network
{
    public class MessageUtil
    {
        /// <summary>
        /// Blocking call to read a full Message object from a given stream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static Message ReadMessage(NetworkStream stream)
        {
            Debug.Assert(stream.CanRead);

            byte[] messageBytes = new byte[Message.MaxSize];
            byte[] sizeBytes = new byte[4];
            int bytesRead = 0;

            // Read the initial size from the stream
            while (bytesRead < 4)
            {
                bytesRead += stream.Read(sizeBytes, bytesRead, sizeBytes.Length);
            }

            int messageSize = BitConverter.ToInt32(sizeBytes, 0);
            Debug.Assert(messageSize < Message.MaxSize);
            bytesRead = 0;

            // Get the message bytes
            while (bytesRead < messageSize)
            {
                bytesRead += stream.Read(messageBytes, bytesRead, messageBytes.Length - bytesRead);
            }

            // Deserialize the message
            return Message.Deserialize(messageBytes);
        }


    }
}
