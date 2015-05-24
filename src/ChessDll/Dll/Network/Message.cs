using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace ChessDll.Network
{
    [Serializable()]
    public class Message
    {
        /// <summary>
        /// Max message size
        /// </summary>
        public static readonly int MaxSize = 1024;

        /// <summary>
        /// Creates a message from serialized bytes
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Message Deserialize(byte[] bytes)
        {
            BinaryFormatter bf = new BinaryFormatter();

            using (var ms = new MemoryStream())
            {
                ms.Write(bytes, 0, bytes.Length);
                ms.Seek(0, SeekOrigin.Begin);
                var obj = bf.Deserialize(ms);
                return (Message) obj;
            }
        }

        /// <summary>
        /// Serializes a message into bytes
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static byte[] Serialize(Message message)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, message);

                Debug.Assert(ms.Length <= Message.MaxSize);
                return ms.ToArray();
            }
        }
    }
}
