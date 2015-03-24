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
    class GoalSprite : Actor
    {
        public GoalSprite(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame, Vector2 velocity, Vector2 scale, ActorType type)
            : base(textureImage, position, frameSize, currentFrame, velocity, scale)
        {
            ActorBody = BodyFactory.CreateRectangle(ThisWorld, ConvertUnits.ToSimUnits(FrameSize.X * Scale.X), ConvertUnits.ToSimUnits(FrameSize.Y * Scale.Y), 10, new Vector2(ConvertUnits.ToSimUnits(DrawPosition.X), ConvertUnits.ToSimUnits(DrawPosition.Y)), type);
            ActorBody.BodyType = BodyType.Static;
            ActorBody.FixtureList[0].UserData = type;
            ActorBody.Friction = 0f;
            ActorBody.Restitution = 0f;
            if (type == ActorType.GoodGoal)
            {
                PointValue = 5;
            }
            else if (type == ActorType.GreatGoal)
            {
                PointValue = 10;
            }
            else
            {
                PointValue = -5;
            }
        }

        public override void Update(GameTime gameTime)
        {
            return;
        }
    }
}
