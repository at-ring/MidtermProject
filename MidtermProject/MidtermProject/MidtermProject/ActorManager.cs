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
    class ActorManager
    {
        private List<Actor> ActorList { get; set; }
        public ActorType PlayerCollidedWith { get; private set; }
        private bool GameAlreadyOver { get; set; }
        private int scoreToAdd;
        public PlayerSprite PlayerSpriteReference { get; set; }
        private BackGroundManager ThisBackGroundManager { get; set; }

        public ActorManager(BackGroundManager thisBackGroundManager)
        {
            ActorList = new List<Actor>();
            GameAlreadyOver = false;
            scoreToAdd = 0;
            PlayerCollidedWith = ActorType.None;
            ThisBackGroundManager = thisBackGroundManager;
        }

        public void Update(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Actor actor in ActorList)
            {
                actor.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Matrix cameraMatrix, Matrix scale)
        {
            Matrix scaleAndCamera = cameraMatrix * scale;
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, scaleAndCamera);
                // draw back ground
                ThisBackGroundManager.DrawBackground(gameTime, spriteBatch);
                // draw sprites
                foreach (Actor actor in ActorList)
                {
                    actor.Draw(gameTime, spriteBatch);
                }
            spriteBatch.End();
        }

        // add sprite to the list of sprites
        public void AddSprite(Actor actorToAdd)
        {
            ActorList.Add(actorToAdd);
        }

        public void UnloadSpriteList()
        {
            foreach (Actor actor in ActorList)
            {
                actor.DestroyPhysicsBody();
            }
            ActorList = new List<Actor>();
            ThisBackGroundManager.ChooseNewBackground();
        }

        // check if the game is over (round)
        public bool HasGameEnded()
        {
            if (PlayerSpriteReference.CollidedWith != ActorType.None)
            {
                switch(PlayerSpriteReference.CollidedWith)
                {
                    case ActorType.NeutralGoal:
                        PlayerCollidedWith = PlayerSpriteReference.CollidedWith;
                        PlayerSpriteReference.CollidedWith = ActorType.None;
                        return true;
                    case ActorType.HorribleGoal:
                        PlayerCollidedWith = PlayerSpriteReference.CollidedWith;
                        PlayerSpriteReference.CollidedWith = ActorType.None;
                        return true;
                    case ActorType.GreatGoal:
                        PlayerCollidedWith = PlayerSpriteReference.CollidedWith;
                        PlayerSpriteReference.CollidedWith = ActorType.None;
                        return true;
                    case ActorType.GoodGoal:
                        PlayerCollidedWith = PlayerSpriteReference.CollidedWith;
                        PlayerSpriteReference.CollidedWith = ActorType.None;
                        return true;
                    case ActorType.BadGoal:
                        PlayerCollidedWith = PlayerSpriteReference.CollidedWith;
                        PlayerSpriteReference.CollidedWith = ActorType.None;
                        return true;
                }
            }
            return false;            
        }

        // determine the score to add to the players score
        public int ScoreToAdd()
        {
            if (!GameAlreadyOver && PlayerCollidedWith != ActorType.None)
            {
                switch (PlayerCollidedWith)
                {
                    case ActorType.NeutralGoal:
                        scoreToAdd = 0;
                        GameAlreadyOver = true;
                        PlayerCollidedWith = ActorType.None;
                        return scoreToAdd;
                    case ActorType.HorribleGoal:
                        scoreToAdd = -10;
                        GameAlreadyOver = true;
                        PlayerCollidedWith = ActorType.None;
                        return scoreToAdd;
                    case ActorType.GreatGoal:
                        scoreToAdd = 10;
                        GameAlreadyOver = true;
                        PlayerCollidedWith = ActorType.None;
                        return scoreToAdd;
                    case ActorType.GoodGoal:
                        scoreToAdd = 5;
                        GameAlreadyOver = true;
                        PlayerCollidedWith = ActorType.None;
                        return scoreToAdd;
                    case ActorType.BadGoal:
                        scoreToAdd = -5;
                        GameAlreadyOver = true;
                        PlayerCollidedWith = ActorType.None;
                        return scoreToAdd;
                    default:
                        return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        // restart the game
        public void ResetGame()
        {
            PlayerSpriteReference.ResetPosition();
            scoreToAdd = 0;
            GameAlreadyOver = false;
        }

        // determine if the ball has been released
        public bool ReleaseState()
        {
            return PlayerSpriteReference.HasBeenReleased;
        }

        public void PrintList()
        {
            foreach (Actor actor in ActorList)
            {
                Console.WriteLine("{0}", actor);
            }
        }
    }
}
