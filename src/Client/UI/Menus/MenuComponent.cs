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
using Jiemyu.Input;
using Jiemyu.Util;

namespace Jiemyu.UI.Menus
{
    class MenuComponent : DrawableGameComponent
    {
        string[] menuItems;
        int selectedIndex;

        Color normal = Color.White;
        Color highlight = Color.Yellow;

        KeyboardState keyboardState;
        KeyboardState oldKeyboardState;

        MouseState oldMouseState;
        MouseState mouseState;

        SpriteBatch spriteBatch;
        TextRenderer textRenderer;

        Texture2D backgroundTexture;
        Texture2D highlightTexture;

        Vector2 position;
        Rectangle surroundingRect = new Rectangle();
        Rectangle paddedRect = new Rectangle();

        const int padding = 10;

        int menuItemHeight = 0;
        bool mouseInMenu = false;

        EventList eventList = new EventList();

        public delegate void ItemSelectedHandler(MenuComponent menu, int index);
        public event ItemSelectedHandler ItemSelected;

        public static readonly EventInfo<MenuComponent, ItemSelectedHandler> ItemSelectedEvent = new EventInfo<MenuComponent, ItemSelectedHandler>((menu, handler) => menu.ItemSelected += handler, (menu, handler) => menu.ItemSelected -= handler);


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
            surroundingRect.Width = 0;
            surroundingRect.Height = 0;

            foreach (string item in menuItems)
            {
                Vector2 size = textRenderer.MeasureString(item);
                if (size.X > surroundingRect.Width)
                    surroundingRect.Width = (int) size.X;
                surroundingRect.Height += textRenderer.LineSpacing + LinePadding;
                menuItemHeight = Math.Max(menuItemHeight, textRenderer.LineSpacing + LinePadding);
            }

            position = new Vector2(
                (Game.Window.ClientBounds.Width - surroundingRect.Width) / 2,
                (Game.Window.ClientBounds.Height - surroundingRect.Height) / 2);

            paddedRect = surroundingRect;
            paddedRect.Width += padding * 2;
            paddedRect.Height += padding * 2;

            eventList.Add(TheMouse.MouseMoveEvent.Subscribe(TheMouse.Instance(), (Point newPoint) =>
            {
                if (InsideMenu(newPoint))
                {
                    mouseInMenu = true;
                }
            }));

            eventList.Add(MouseButton.ClickedEvent.Subscribe(TheMouse.Instance().LeftButton, (MouseButton btn) =>
            {
                if (mouseInMenu)
                {
                    ItemSelected(this, SelectedIndex);
                }
            }));
        }

        /// <summary>
        /// Says whether a point is located within the menu items
        /// </summary>
        /// <param name="newPoint"></param>
        /// <returns></returns>
        private bool InsideMenu(Point newPoint)
        {
            return newPoint.X >= (position.X + padding) && newPoint.X <= (position.X + paddedRect.Width) &&
                newPoint.Y >= (position.Y + padding) && newPoint.Y <= (position.Y + paddedRect.Height);
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

            // Check if the mouse is over any of the menu items
            mouseState = Mouse.GetState();

            if (mouseState.X >= (position.X + padding) && mouseState.X <= (position.X + paddedRect.Width) &&
                mouseState.Y >= (position.Y + padding) && mouseState.Y <= (position.Y + paddedRect.Height))
            {
                // Find the menu item based on the y value
                var mouseY = (mouseState.Y - (position.Y + padding));
                selectedIndex = (int)Math.Floor(mouseY / menuItemHeight);
            }

            // Always do selection after movemenet, to ensure we never have an odd race between keyboard and mouse
            if (CheckKey(Keys.Enter))
            {
                ItemSelected(this, SelectedIndex);
            }

            base.Update(gameTime);

            oldMouseState = mouseState;
            oldKeyboardState = keyboardState;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            Vector2 location = position;

            // Draw the background
            Vector2 backgroundLocation = location;
            backgroundLocation.X -= padding;
            backgroundLocation.Y -= padding; 
            spriteBatch.Draw(backgroundTexture, backgroundLocation, paddedRect, Color.White);


            for (int i = 0; i < menuItems.Length; i++)
            {
                Rectangle rect = textRenderer.MeasureStringRect(menuItems[i]);
                rect.Location = new Point((int)location.X, (int)location.Y);

                textRenderer.DrawText(spriteBatch,
                    (int)location.X,
                    (int)location.Y,
                    menuItems[i]);

                // Always draw over text, since it has black on the background
                if (i == selectedIndex)
                {
                    spriteBatch.Draw(highlightTexture, new Rectangle((int) backgroundLocation.X, rect.Location.Y-5, paddedRect.Width, rect.Height*2 + 5), Color.White);
                }

                location.Y += textRenderer.LineSpacing + LinePadding;
            }
        }
    }
}

