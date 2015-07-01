using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ChessDemo.Map;
using ChessDll.Entities;
using System.Collections.Generic;
using ChessDll.Map;
using ChessDemo.GameState;

namespace ChessDemo
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;

        private Dictionary<GameStates, DrawableGameComponent> gameStateMap = new Dictionary<GameStates, DrawableGameComponent>();

        public Game1()
        {
            gameStateMap[GameStates.MainMenu] = new MainMenuGameState(this);
            gameStateMap[GameStates.InGame] = new InGameGameState(this);

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.IsMouseVisible = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            GraphicsDevice.BlendState = BlendState.AlphaBlend;

            foreach (var gameComponent in gameStateMap.Values)
            {
                /// Initialize is called here, because it will call loadContent
                /// on a DrawableGameObject. 
                gameComponent.Initialize();
            }

            base.LoadContent();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (GameStateManager.Instance.CurrentState != GameStates.Uninitialized)
            {
                gameStateMap[GameStateManager.Instance.CurrentState].Update(gameTime);
            }

            GameStateManager.Instance.Update();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (GameStateManager.Instance.CurrentState != GameStates.Uninitialized)
            {
                gameStateMap[GameStateManager.Instance.CurrentState].Draw(gameTime);
            }
        }
    }
}
