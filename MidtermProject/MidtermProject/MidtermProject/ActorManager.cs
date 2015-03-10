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
    class ActorManager
    {
        private List<Actor> ActorList { get; set; }

        public ActorManager(Game game)
        {
            ActorList = new List<Actor>();
        }

        public void Update(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Actor actor in ActorList)
            {
                actor.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null);
                foreach (Actor actor in ActorList)
                {
                    actor.Draw(gameTime, spriteBatch);
                }
            spriteBatch.End();
        }

        public void AddSprite(Actor actorToAdd)
        {
            ActorList.Add(actorToAdd);
        }
    }
}
