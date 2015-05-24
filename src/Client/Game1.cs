using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ChessDemo.Map;
using ChessDll.Entities;
using System.Collections.Generic;
using ChessDll.Map;

namespace ChessDemo
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        MapObject tileMap;
        ChessInitializer entityFactory;

        int KeyboundCameraIncrement = 3;

        public Game1()
        {
            tileMap = new MapObject(GetTiles());
            entityFactory = new ChessInitializer();
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
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            tileMap.AddTileTexture(Content.Load<Texture2D>("Plain Block"));
            tileMap.AddTileTexture(Content.Load<Texture2D>("Dirt Block"));
            tileMap.AddTileTexture(Content.Load<Texture2D>("Stone Block"));
            tileMap.AddTileTexture(Content.Load<Texture2D>("Water Block"));

            tileMap.AddDecalTexture(Content.Load<Texture2D>("Tree Tall"));
            tileMap.AddDecalTexture(Content.Load<Texture2D>("Rock"));

            tileMap.HighlightTexture = Content.Load<Texture2D>("Selector");
            tileMap.MoveIndicator = Content.Load<Texture2D>("Highlight");
            tileMap.AttackIndicator = Content.Load<Texture2D>("Attack Highlight");

            entityFactory.LoadContent(Content);


            //Initializing Entity Positions.  This should probably move.
            List<Entity> initialEntities = entityFactory.InitializeChessLayout();

            //Initializing Teams... ""
            List<Team> initialTeams = entityFactory.InitializeChessTeams();

            foreach(Entity e in initialEntities)
            {
                tileMap.AddObject(e, tileMap.GetTileForPoint(e.Position));

                //If it's on the top half of the map, add to top team.  Otherwise bottom team.
                if (e.Position.Y < 684)
                {
                    tileMap.AssignTeam(e, initialTeams[0]);
                }
                else
                {
                    tileMap.AssignTeam(e, initialTeams[1]);
                }
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            int CameraXIncrement = 0;
            int CameraYIncrement = 0;

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                CameraXIncrement += -KeyboundCameraIncrement;
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                CameraXIncrement += KeyboundCameraIncrement;

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                CameraYIncrement += -KeyboundCameraIncrement;
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                CameraYIncrement += KeyboundCameraIncrement;

            // Make sure to update the camera before the cursor
            tileMap.MoveCamera(new Vector2(CameraXIncrement, CameraYIncrement));
            tileMap.UpdateCursor(Mouse.GetState());

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            tileMap.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }


        int[,] map = new int[,]
        {
            {3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3},
            {3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3},
            {3, 3, 1, 0, 1, 0, 1, 0, 1, 0, 3, 3},
            {3, 3, 0, 1, 0, 1, 0, 1, 0, 1, 3, 3},
            {3, 3, 1, 0, 1, 0, 1, 0, 1, 0, 3, 3},
            {3, 3, 0, 1, 0, 1, 0, 1, 0, 1, 3, 3},
            {3, 3, 1, 0, 1, 0, 1, 0, 1, 0, 3, 3},
            {3, 3, 0, 1, 0, 1, 0, 1, 0, 1, 3, 3},
            {3, 3, 1, 0, 1, 0, 1, 0, 1, 0, 3, 3},
            {3, 3, 0, 1, 0, 1, 0, 1, 0, 1, 3, 3},
            {3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3},
            {3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3}
        };


        /// <summary>
        /// Converts the static map int index to Tile objects
        /// </summary>
        /// <returns></returns>
        private Tile[,] GetTiles()
        {
            int width = map.GetLength(0),
                height = map.GetLength(1);

            Tile[,] tiles = new Tile[width, height];

            for (int row = 0; row < map.GetLength(0); row++)
            {
                for (int col = 0; col < map.GetLength(1); col++)
                {
                    Tile t = new Tile();
                    t.TextureIndex = map[row, col];
                    tiles[row,col] = t;
                }
            }

            return tiles;
        }
    }
}
