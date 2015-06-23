using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessDll.Network.Messages
{
    [Serializable()]
    public class MoveMessage : EntityMessage
    {
        public float X { get; set; }
        public float Y { get; set; }

        public MoveMessage(Vector2 newLocation)
        {
            X = newLocation.X;
            Y = newLocation.Y;
        }
    }
}
