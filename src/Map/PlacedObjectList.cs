using ChessDemo.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessDemo.Map
{
    class PlacedObjectList : SortedList<Vector2, Entity>
    {
        public PlacedObjectList(IComparer<Vector2> comparer) : base(comparer)
        {
        }

        public void UpdatePosition(Entity entity, Vector2 newPosition)
        {
            int index = this.IndexOfValue(entity);

            // Remove from the list and add back in order to keep the list sorted
            this.RemoveAt(index);
            this.Add(newPosition, entity);
        }
    }
}
