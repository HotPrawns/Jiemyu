using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessDemo.Map
{
    class Vector2Comparer : IComparer<Vector2>
    {
        public int Compare(Vector2 v1, Vector2 v2)
        {
            if (v1.Y == v2.Y)
            {
                return (int)(v1.X - v2.X);
            }

            return (int)(v1.Y - v2.Y);
        }
    }
}
