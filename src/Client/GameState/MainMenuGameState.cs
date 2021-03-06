﻿using Jiemyu.UI;
using Jiemyu.UI.Menus;
using Jiemyu.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Jiemyu.GameState
{
    class MainMenuGameState : DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        TextRenderer textRenderer;

        string[] menuItems = { "Start Game", "Close" };

        MenuComponent menu;

        Texture2D highlightTexture;
        Texture2D backgroundTexture;

        EventList eventList = new EventList();

        public MainMenuGameState(Game game)
            : base(game)
        {

        }

        /// <summary>
        /// 
        /// </summary>
       override  public void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            var fontFilePath = Path.Combine(Game.Content.RootDirectory, "calibri32.fnt");
            using(var stream = TitleContainer.OpenStream(fontFilePath))
            {
                var fontFile = FontFile.Load(stream);
                var fontTexture = Game.Content.Load<Texture2D>("calibri32_0.png");

                textRenderer = new TextRenderer(fontFile, fontTexture);
                stream.Close();
            }

            base.Initialize();
        }

        /// <summary>
        /// 
        /// </summary>
        override protected void LoadContent()
        {
            backgroundTexture = Game.Content.Load<Texture2D>("Black");
            highlightTexture = Game.Content.Load<Texture2D>("Menu Highlight");
            menu = new MenuComponent(Game, spriteBatch, textRenderer, backgroundTexture, highlightTexture, menuItems);
            eventList.Add(MenuComponent.ItemSelectedEvent.Subscribe(menu, new MenuComponent.ItemSelectedHandler(menu_ItemSelected)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void menu_ItemSelected(MenuComponent sender, int index)
        {
            if (index == 0)
            {
                GameStateManager.Instance.SetState(GameStates.InGame);
            }
            else if (index == 1)
            {
                GameStateManager.Instance.SetState(GameStates.Exit);
            }
        }

        override protected void UnloadContent()
        {
            menu = null;
        }

        override public void Update(GameTime gameTime)
        {
            menu.Update(gameTime);
        }

        override public void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            menu.Draw(gameTime);
            spriteBatch.End();
        }
    }
}
