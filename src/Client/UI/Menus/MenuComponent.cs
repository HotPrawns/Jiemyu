using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;

namespace Jiemyu.UI.Menus
{
    public class MenuComponent : DrawableGameComponent
    {
        string[] menuItems;
        int selectedIndex;

        Color normal = Color.White;
        Color hilite = Color.Yellow;

        KeyboardState keyboardState;
        KeyboardState oldKeyboardState;

        SpriteBatch spriteBatch;
        TextRenderer textRenderer;

        Texture2D backgroundTexture;
        Texture2D highlightTexture;

        Vector2 position;
        float width = 0f;
        float height = 0f;

        public event EventHandler ItemSelected = new EventHandler((e, a) => { });

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = value;
                if (selectedIndex < 0)
                    selectedIndex = 0;
                if (selectedIndex >= menuItems.Length)
                    selectedIndex = menuItems.Length - 1;
            }
        }

        /// <summary>
        /// Extra padding between each line
        /// </summary>
        public int LinePadding
        {
            get;
            set;
        }

        public MenuComponent(Game game,
            SpriteBatch spriteBatch,
            TextRenderer textRenderer,
            Texture2D backgroundTexture,
            Texture2D highlightTexture,
            string[] menuItems)
            : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.textRenderer = textRenderer;
            this.menuItems = menuItems;
            this.backgroundTexture = backgroundTexture;
            this.highlightTexture = highlightTexture;
            LinePadding = 5;
            MeasureMenu();
        }

        private void MeasureMenu()
        {
            height = 0;
            width = 0;
            foreach (string item in menuItems)
            {
                Vector2 size = textRenderer.MeasureString(item);
                if (size.X > width)
                    width = size.X;
                height += textRenderer.LineSpacing + LinePadding;
            }

            position = new Vector2(
                (Game.Window.ClientBounds.Width - width) / 2,
                (Game.Window.ClientBounds.Height - height) / 2);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        private bool CheckKey(Keys theKey)
        {
            return keyboardState.IsKeyUp(theKey) &&
                oldKeyboardState.IsKeyDown(theKey);
        }

        public override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();

            if (CheckKey(Keys.Down))
            {
                selectedIndex++;
                if (selectedIndex == menuItems.Length)
                    selectedIndex = 0;
            }
            if (CheckKey(Keys.Up))
            {
                selectedIndex--;
                if (selectedIndex < 0)
                    selectedIndex = menuItems.Length - 1;
            }

            if (CheckKey(Keys.Enter))
            {
                ItemSelected(this, new EventArgs());
            }

            base.Update(gameTime);

            oldKeyboardState = keyboardState;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            Vector2 location = position;
            Texture2D texture;

            for (int i = 0; i < menuItems.Length; i++)
            {
                if (i == selectedIndex)
                    texture = highlightTexture;
                else
                    texture = backgroundTexture;

                Rectangle rect = textRenderer.MeasureStringRect(menuItems[i]);
                rect.Location = new Point((int) location.X, (int) location.Y);

                spriteBatch.Draw(texture, rect, Color.White);

                textRenderer.DrawText(spriteBatch,
                    (int) location.X,
                    (int) location.Y,
                    menuItems[i]);

                location.Y += textRenderer.LineSpacing + LinePadding;
            }
        }
    }
}

