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

namespace MidtermProject
{
    class GameManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        // field: total score for the player
        private int playerScore;
        private int computerScore;
        private Matrix cameraMatrix = Matrix.Identity;
        // field: is the level over?
        private static bool levelEnd;
        private SpriteBatch spriteBatch;
        private ActorManager ActorManager { get; set; } 
        private SoundManager SoundManager { get; set; } 
        private LevelManager LevelManager { get; set; } 
        private BackGroundManager BackGroundManager { get; set; }
        private PhysicsWorldManager PhysicsWorldManager { get; set; }
        private Vector3 PreviousCameraPosition { get; set; }
        private Game ThisGame { get; set; }
        private PlayerSprite ThisPlayer { get; set; }
        private float ScaleValue { get; set; }
        private float ScaleUpRate { get; set; }
        private int BallCount { get; set; }
        private int BallsRemaining { get; set; }
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
        private SpriteFont font;
        private bool PlayerTurn { get; set; }
        private bool GameOver { get; set; }

        public GameManager(Game game) : base(game)
        {
            levelEnd = false;
            playerScore = 0;
            computerScore = 0; 
            ThisGame = game;
            PhysicsWorldManager = new PhysicsWorldManager();
            SoundManager = new SoundManager(@"Content/Sounds/PlinkoSoundEffects");
            ScaleValue = 0.5f;
            ScaleUpRate = 0.005f;
            BallCount = 0; // number of balls + 1
            BallsRemaining = BallCount;
            PreviousCameraPosition = new Vector3(0, 0, 0);
            PlayerTurn = false;
            GameOver = false;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(ThisGame.GraphicsDevice);

            font = ThisGame.Content.Load<SpriteFont>(@"Fonts/myFont");
            // TODO: use this.Content to load your game content here
                       
            Texture2D backgroundBricks = ThisGame.Content.Load<Texture2D>("Backgrounds/background1");
            Texture2D backgroundWood = ThisGame.Content.Load<Texture2D>("Backgrounds/background2");
            Texture2D backgroundTile = ThisGame.Content.Load<Texture2D>("Backgrounds/background3");

            Texture2D[] backgrounds = { backgroundBricks, backgroundWood, backgroundTile };
            
            Point screenDimentions = new Point(ThisGame.Window.ClientBounds.Width, ThisGame.Window.ClientBounds.Height * 2);
            BackGroundManager = new BackGroundManager(backgrounds, screenDimentions);
            BackGroundManager.ChooseNewBackground();

            ActorManager = new ActorManager(BackGroundManager);

            Texture2D goalTexture = ThisGame.Content.Load<Texture2D>(@"Images/goal");
            Texture2D wallPegTexture = ThisGame.Content.Load<Texture2D>(@"Images/WallPeg");
            Texture2D pegTexture = ThisGame.Content.Load<Texture2D>(@"Images/Peg");
            Texture2D badGoalMarkerTexture = ThisGame.Content.Load<Texture2D>(@"Images/GoalMarkerBad");
            Texture2D goodGoalMarkerTexture = ThisGame.Content.Load<Texture2D>(@"Images/GoalMarkerGood");
            Texture2D greatGoalMarkerTexture = ThisGame.Content.Load<Texture2D>(@"Images/GoalMarkerGreat");
            Texture2D horribleGoalMarkerTexture = ThisGame.Content.Load<Texture2D>(@"Images/GoalMarkerHorrible");
            Texture2D neutralGoalMarkerTexture = ThisGame.Content.Load<Texture2D>(@"Images/GoalMarkerNeutral");
            Texture2D goalPostTexture = ThisGame.Content.Load<Texture2D>(@"Images/goalPost");
            Texture2D wallTexture = ThisGame.Content.Load<Texture2D>(@"Images/wall");
            Texture2D playerCircleTexture = ThisGame.Content.Load<Texture2D>(@"Images/playerCircle");

            Texture2D[] textures = { goalTexture, wallPegTexture, pegTexture, badGoalMarkerTexture, goodGoalMarkerTexture, greatGoalMarkerTexture, horribleGoalMarkerTexture, neutralGoalMarkerTexture, goalPostTexture, wallTexture, playerCircleTexture };


            int numberOfCompleteRows = 12;
            int baseSize = 10; // size of all wall like objects (width or height depending on the sprite)
            Point pegSize = new Point(pegTexture.Width, pegTexture.Height); // size of the peg sprite
            Point playerSize = new Point(playerCircleTexture.Width, playerCircleTexture.Height); // size of the player sprite
            Point spacing = new Point(75, 70); // spacing between rows and cols
            Point wallSize = new Point(wallTexture.Width, wallTexture.Height);
            Point goalSize = new Point(goalTexture.Width, goalTexture.Height);
            Point goalPostSize = new Point(goalPostTexture.Width, goalPostTexture.Height);
            Point wallPegSize = new Point(wallPegTexture.Width, wallPegTexture.Height);
            Point goalMarkerSize = new Point(goalTexture.Width, goalPostTexture.Height); // size of the goal marker sprite

            Point[] points = { pegSize, playerSize, spacing, wallSize, goalSize, goalPostSize, wallPegSize, goalMarkerSize }; 


            LevelManager = new LevelManager(baseSize, numberOfCompleteRows, points, ActorManager, ThisGame, textures);
            LevelManager.LoadNextLevel();
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
            if (LevelManager.LevelsRemaining > -1)
            {
                KeyboardState keyboardState = Keyboard.GetState();
                // reset ball if it gets stuck
                if (keyboardState.IsKeyDown(Keys.R))
                {
                    ActorManager.ResetGame();
                    ScaleValue = 0.5f;
                    PreviousCameraPosition = new Vector3(0, 0, 0);
                    cameraMatrix = Matrix.Identity;
                }
                // determine who should control the player circle
                if (!PlayerTurn)
                {
                    Vector2 movementVector = new Vector2(.005f, 0);
                    // if the player sprite hasn't been released yet and is not at the drop location
                    if (!ActorManager.ReleaseState() && ActorManager.PlayerSpriteReference.BasePosition.X < LevelManager.DropLocation())
                    {
                        ActorManager.PlayerSpriteReference.MoveToNewLocation(movementVector);
                    }
                    else
                    {
                        ActorManager.PlayerSpriteReference.Release();
                    }
                    PhysicsWorldManager.UpdateWorld(gameTime);
                    ActorManager.Update(gameTime, spriteBatch);
                    bool isRoundOver = ActorManager.HasGameEnded();
                    // check if the game is over
                    if (isRoundOver)
                    {
                        int scoreToAdd = ActorManager.ScoreToAdd();
                        computerScore += scoreToAdd;
                        if (BallsRemaining > 0)
                        {
                            ActorManager.ResetGame();
                            ScaleValue = 0.5f;
                            BallsRemaining -= 1;
                            PreviousCameraPosition = new Vector3(0, 0, 0);
                        }
                        // load next level
                        else
                        {
                            // set to player's turn
                            PlayerTurn = true;
                            // reset ball counter
                            BallsRemaining = BallCount;
                            // reset the camera
                            PreviousCameraPosition = new Vector3(0, 0, 0);
                            // reset game
                            ActorManager.ResetGame();
                            // reset the scale
                            ScaleValue = 0.5f;
                            // reset the camera matrix
                            cameraMatrix = Matrix.Identity;
                        }
                    }
                }
                else
                {
                    // if the player sprite hasn't been released yet
                    if (!ActorManager.ReleaseState())
                    {
                        Vector2 movementVector = new Vector2(1, 0);
                        // move the player sprite left
                        if (keyboardState.IsKeyDown(Keys.Left))
                        {
                            ActorManager.PlayerSpriteReference.MoveToNewLocation(-movementVector);
                        }
                        // move the player sprite right
                        else if (keyboardState.IsKeyDown(Keys.Right))
                        {
                            ActorManager.PlayerSpriteReference.MoveToNewLocation(movementVector);
                        }
                        // drop the player sprite
                        else if (keyboardState.IsKeyDown(Keys.Down))
                        {
                            ActorManager.PlayerSpriteReference.Release();
                        }
                    }
                    PhysicsWorldManager.UpdateWorld(gameTime);
                    ActorManager.Update(gameTime, spriteBatch);
                    bool isRoundOver = ActorManager.HasGameEnded();
                    // check if the game is over
                    if (isRoundOver)
                    {
                        int scoreToAdd = ActorManager.ScoreToAdd();
                        playerScore += scoreToAdd;
                        if (BallsRemaining > 0)
                        {
                            ActorManager.ResetGame();
                            ScaleValue = 0.5f;
                            BallsRemaining -= 1;
                            PreviousCameraPosition = new Vector3(0, 0, 0);
                        }
                        // load next level
                        else
                        {
                            // unload level
                            LevelManager.UnloadCurrentLevel();
                            // load new level
                            LevelManager.LoadNextLevel();
                            // reset ball counter
                            BallsRemaining = BallCount;
                            // reset the camera
                            PreviousCameraPosition = new Vector3(0, 0, 0);
                            // reset game
                            ActorManager.ResetGame();
                            // reset the scale
                            ScaleValue = 0.5f;
                            // draw the next level
                            PlayerTurn = false;
                            // reset the camera matrix
                            cameraMatrix = Matrix.Identity;
                        }
                    }
                }
            }
            else
            {
                GameOver = true;
                KeyboardState keyboardState = Keyboard.GetState();
                // reset ball if it gets stuck
                if (keyboardState.IsKeyDown(Keys.R))
                {
                    ActorManager.ResetGame();
                    ScaleValue = 0.5f;
                    PreviousCameraPosition = new Vector3(0, 0, 0);
                    LevelManager.ResetGame();
                    computerScore = 0;
                    playerScore = 0;
                    GameOver = false;
                }
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            
            GraphicsDevice.Clear(Color.CornflowerBlue);
            if (!GameOver)
            {
                // TODO: Add your drawing code here
                
                Vector3 newCameraPosition = new Vector3(0, -(ActorManager.PlayerSpriteReference.BasePosition.Y - (Game.Window.ClientBounds.Height / 2)), 0);

                // if the player is not lower than the bottom half of the screen
                if ((ActorManager.PlayerSpriteReference.BasePosition.Y > Game.Window.ClientBounds.Height / 2f) && (ActorManager.PlayerSpriteReference.BasePosition.Y < Game.Window.ClientBounds.Height * 1.5f))
                {
                    // only move the camera if the new position is lower than the previous position
                    
                    if (-newCameraPosition.Y > -PreviousCameraPosition.Y)
                    {
                        cameraMatrix = Matrix.CreateTranslation(newCameraPosition);
                        PreviousCameraPosition = newCameraPosition;
                    }
                    // otherwise use the previous position
                    else
                    {
                        cameraMatrix = Matrix.CreateTranslation(PreviousCameraPosition);
                    }
                }
                // if the player sprite isn't below the halfway mark then the camera stays at (0,0,0)
                //else
                //{
                //    cameraMatrix = Matrix.CreateTranslation(new Vector3(0, 0, 0));
                //}
                Matrix scaleMatrix;
                // if the player sprite has been released
                if (ActorManager.ReleaseState())
                {
                    // zoom in to the player sprite if we are not at scale of 1 yet
                    if (ScaleValue < 1f)
                    {
                        ScaleValue += ScaleUpRate;
                    }
                    scaleMatrix = Matrix.CreateScale(ScaleValue);
                }
                // use the scale value given at the beginning to see the whole board
                else
                {
                    scaleMatrix = Matrix.CreateScale(ScaleValue);
                }
                ActorManager.Draw(gameTime, spriteBatch, cameraMatrix, scaleMatrix);
                DrawText();

                // play sound if player collided with wall or peg
                if (ActorManager.PlayerCollidedWith == ActorType.Wall)
                {
                    SoundManager.PlaySound("hit");
                }
            }
            else
            {
                DrawEndGameText();
            }

            base.Draw(gameTime);
        }
        private void DrawText()
        {
            spriteBatch.Begin();
                String turn;
                if (PlayerTurn)
                {
                    turn = "Player's turn";
                }
                else
                {
                    turn = "Computer's turn";
                }
                spriteBatch.DrawString(font, turn, new Vector2((ThisGame.Window.ClientBounds.X) / ScaleValue, 0), Color.Black);
                spriteBatch.DrawString(font, "Computer Score: " + computerScore, new Vector2((ThisGame.Window.ClientBounds.X) / ScaleValue, 20), Color.Black);
                spriteBatch.DrawString(font, "Player Score: " + playerScore, new Vector2((ThisGame.Window.ClientBounds.X) / ScaleValue, 40), Color.Black);
                spriteBatch.DrawString(font, "Balls Remaining: " + BallsRemaining, new Vector2((ThisGame.Window.ClientBounds.X) / ScaleValue, 60), Color.Black);
            spriteBatch.End();

        }

        private void DrawEndGameText()
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
                if (playerScore > computerScore)
                {
                    spriteBatch.DrawString(font, "You win! With a score of " + playerScore + " to " + computerScore, new Vector2(((ThisGame.Window.ClientBounds.Width / 2) - 60), (ThisGame.Window.ClientBounds.Height / 2)), Color.Black);
                }
                else if (computerScore > playerScore)
                {
                    spriteBatch.DrawString(font, "You lose! With a score of " + computerScore + " to " + playerScore, new Vector2(((ThisGame.Window.ClientBounds.Width / 2) - 60), (ThisGame.Window.ClientBounds.Height / 2)), Color.Black);
                }
                else
                {
                    spriteBatch.DrawString(font, "You tied! With a score of " + computerScore, new Vector2(((ThisGame.Window.ClientBounds.Width / 2) - 60), (ThisGame.Window.ClientBounds.Height / 2)), Color.Black);
                }
                spriteBatch.DrawString(font, "Press 'R' to start a new game", new Vector2(((ThisGame.Window.ClientBounds.Width / 2) - 80), (ThisGame.Window.ClientBounds.Height / 2 + 20)), Color.Black);
            spriteBatch.End();
        }
    }
}
