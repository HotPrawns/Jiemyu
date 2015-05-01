using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_Demo.Utilities
{
    class Assets
    {
        public static string ASSET_PATH = "assets";

        public static StreamReader GetStreamForAsset(string relativePath)
        {
            return new StreamReader(Path.Combine(ASSET_PATH, relativePath));
        }
    }
}
