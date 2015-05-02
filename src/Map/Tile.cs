using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessDemo.Map
{
    class Tile
    {
        public int TextureIndex = 0;
        public int DecalIndex = -1;

        public bool HasDecal { get { return DecalIndex >= 0; } }

        public bool HasTexture { get { return TextureIndex >= 0; } }
    }
}
