using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Jiemyu.UI
{
    /// <summary>
    /// Helper class to draw text using SpriteBatch
    /// </summary>
    public class TextRenderer
    {
        private FontFile fontFile;
        private Texture2D texture;
        private Dictionary<char, FontChar> characterMap;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fontFile"></param>
        /// <param name="texture"></param>
        public TextRenderer(FontFile fontFile, Texture2D texture)
        {
            this.fontFile = fontFile;
            this.texture = texture;

            characterMap = new Dictionary<char, FontChar>();

            foreach(var fontCharacter in fontFile.Chars)
            {
                char c = (char)fontCharacter.ID;
                characterMap.Add(c, fontCharacter);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="text"></param>
        public void DrawText(SpriteBatch spriteBatch, int x, int y, string text)
        {
            int dx = x;
            int dy = y;
            foreach (char c in text)
            {
                FontChar fc;
                if (characterMap.TryGetValue(c, out fc))
                {
                    var sourceRectangle = MeasureChar(c);
                    var position = new Vector2(dx + fc.XOffset, dy + fc.YOffset);

                    spriteBatch.Draw(texture, position, sourceRectangle, Color.White);
                    dx += fc.XAdvance;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int LineSpacing
        {
            get
            {
                return this.fontFile.Common.LineHeight;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        internal Vector2 MeasureString(string item)
        {
            Vector2 size = Vector2.Zero;

            foreach(char c in item)
            {
                Rectangle charSize = MeasureChar(c);
                size.X += charSize.Width;
                size.Y += charSize.Height;
            }

            return size;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        internal Rectangle MeasureStringRect(string item)
        {
            Rectangle rect = new Rectangle();

            foreach (char c in item)
            {
                Rectangle charSize = MeasureChar(c);
                rect.Width += charSize.Width;
                rect.Height = Math.Max(rect.Height, charSize.Height);
            }

            return rect;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        internal Rectangle MeasureChar(char c)
        {
            FontChar fc = characterMap[c];
            // Use XAdvance over height, since that indicates the advancement
            // before the next letter to be shown
            return new Rectangle(fc.X, fc.Y, fc.XAdvance, fc.Height);
        }
    }
}