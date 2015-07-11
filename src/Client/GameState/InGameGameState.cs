using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Jiemyu.Map;
using JiemyuDll.Entities;
using JiemyuDll.Map;
using Microsoft.Xna.Framework.Input;

namespace Jiemyu.GameState
{
    class InGameGameState : DrawableGameComponent
    {
        MapObject tileMap;
        JiemyuAlphaInitializer entityFactory;

        int KeyboundCameraIncrement = 3;

        SpriteBatch spriteBatch;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        public InGameGameState(Game game)
            : base(game)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        override public void Initialize()
        {
            tileMap = new MapObject(GetTiles());
            entityFactory = new JiemyuAlphaInitializer();

            base.Initialize();
        }

        /// <summary>
        /// 
        /// </summary>
        override protected void LoadContent()
        {
            tileMap.AddTileTexture(Game.Content.Load<Texture2D>("Plain Block"));
            tileMap.AddTileTexture(Game.Content.Load<Texture2D>("Dirt Block"));
            tileMap.AddTileTexture(Game.Content.Load<Texture2D>("Stone Block"));
            tileMap.AddTileTexture(Game.Content.Load<Texture2D>("Water Block"));

            tileMap.AddDecalTexture(Game.Content.Load<Texture2D>("Tree Tall"));
            tileMap.AddDecalTexture(Game.Content.Load<Texture2D>("Rock"));

            tileMap.HighlightTexture = Game.Content.Load<Texture2D>("Selector");
            tileMap.MoveIndicator = Game.Content.Load<Texture2D>("Highlight");
            tileMap.AttackIndicator = Game.Content.Load<Texture2D>("Attack Highlight");

            entityFactory.LoadContent(Game.Content);

            //Initializing Entity Positions.  This should probably move.
            List<Entity> initialEntities = entityFactory.InitializeJiemyuAlpha1Layout();

            //Initializing Teams
            List<Team> initialTeams = entityFactory.InitializeJiemyuAlpha1Teams();

            foreach (Entity e in initialEntities)
            {
                tileMap.AddObject(e, tileMap.GetTileForPoint(e.Position));

                //If it's on the top half of the map, add to top team.  Otherwise bottom team.
                if (e.Position.Y < 684)
                {
                    TurnManager.Instance.Add(e, initialTeams[0]);
                }
                else
                {
                    TurnManager.Instance.Add(e, initialTeams[1]);
                }
            }

            spriteBatch = new SpriteBatch(GraphicsDevice);

            base.LoadContent();
        }

        /// <summary>
        /// 
        /// </summary>
        override protected void UnloadContent()
        {
            base.UnloadContent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        override public void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Game.Exit();

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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        override public void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            tileMap.Draw(spriteBatch);
            spriteBatch.End();
        }

        /// <summary>
        /// 
        /// </summary>
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
                    tiles[row, col] = t;
                }
            }

            return tiles;
        }
    }
}
