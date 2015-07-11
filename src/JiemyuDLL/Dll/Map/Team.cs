using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiemyuDll.Map
{
    public class Team
    {
        /// <summary>
        /// Name of the team
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Team's color.  Currently used for highlighting movement.
        /// </summary>
        public Color Color { get; set; }
    }
}
