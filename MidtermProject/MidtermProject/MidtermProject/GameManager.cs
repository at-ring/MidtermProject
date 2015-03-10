using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Common;

namespace MidtermProject
{
    class GameManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private static int score;
        private static bool levelEnd;
        private SpriteBatch spriteBatch;
        private ActorManager ActorManager { get; set; }
        private SoundManager SoundManager { get; set; }
        private LevelManager LevelManager { get; set; }
        private BackGroundManager BackGroundManager { get; set; }
        private PhysicsWorldManager PhysicsWorldManager { get; set; }
        private Game ThisGame { get; set; }
        public static int Score 
        {
            get
            {
                return score;
            }
            set
            {
                score += value;
            }
        }
        public static bool LevelEnd 
        {
            get
            {
                return levelEnd;
            }
            set
            {
                levelEnd = value;
            }            
        }

        public GameManager(Game game) : base(game)
        {
            levelEnd = false;
            score = 0;
            ThisGame = game;
            PhysicsWorldManager = new PhysicsWorldManager();
            ActorManager = new ActorManager(game);
            SoundManager = new SoundManager();
            LevelManager = new LevelManager();
            BackGroundManager = new BackGroundManager();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(ThisGame.GraphicsDevice);
            // TODO: use this.Content to load your game content here
            ActorManager.AddSprite(new PlayerSprite(ThisGame.Content.Load<Texture2D>(@"Images/playerCircle"), new Vector2(0, 0), new Point(49, 48), new Point(1, 1), Vector2.Zero, new Vector2(1, 1)));
            ActorManager.AddSprite(new PegSprite(ThisGame.Content.Load<Texture2D>(@"Images/peg"), new Vector2(10, 100), new Point(25, 25), new Point(1, 1), Vector2.Zero, new Vector2(1, 1)));
            ActorManager.AddSprite(new GoalSprite(ThisGame.Content.Load<Texture2D>(@"Images/goalBox"), new Vector2(0, 300), new Point(1217, 11), new Point(1, 1), Vector2.Zero, new Vector2(1, 1), ActorType.GoodGoal));
            ActorManager.AddSprite(new WallSprite(ThisGame.Content.Load<Texture2D>(@"Images/goalPost"), new Vector2(200, 200), new Point(11, 92), new Point(1, 1), Vector2.Zero, new Vector2(1, 1)));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
        public override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                ThisGame.Exit();
            // TODO: Add your update logic here
            ActorManager.Update(gameTime, spriteBatch);
            PhysicsWorldManager.UpdateWorld(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // TODO: Add your drawing code her
            ActorManager.Draw(gameTime, spriteBatch);
            base.Draw(gameTime);
        }
    }
}
