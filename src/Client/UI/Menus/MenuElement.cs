using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jiemyu.UI.Menus
{
    class MenuElement : DrawableGameComponent
    {
        private Texture2D elementTexture;
        private Rectangle inputRect; 

        public MenuElement(Game game)
            : base(game)
        {
            
        }
    }
}
