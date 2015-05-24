using ChessDll.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessDll.Map
{
    public class RenderObject : IComparable<RenderObject>
    {
        private Entity _Entity;
        public Entity Entity
        {
            get
            {
                return _Entity;
            }
        }

        private Vector2 _Location;
        public Vector2 Location
        {
            get
            {
                return _Location;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="location"></param>
        public RenderObject(Entity entity, Vector2 location)
        {
            _Entity = entity;
            _Location = location;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(RenderObject other)
        {
            return Vector2Comparer.CompareVectors(this.Location, other.Location);
        }
    }
}
