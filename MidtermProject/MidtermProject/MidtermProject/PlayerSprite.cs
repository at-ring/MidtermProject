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
    class PlayerSprite : Actor
    {
        private bool hasBeenReleased = false;

        public PlayerSprite(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame, Vector2 velocity, Vector2 scale)
            : base(textureImage, position, frameSize, currentFrame, velocity, scale)
        {
            ActorBody = BodyFactory.CreateCircle(ThisWorld, ConvertUnits.ToSimUnits((FrameSize.X * Scale.X) / 2), 10, new Vector2(ConvertUnits.ToSimUnits(DrawPosition.X), ConvertUnits.ToSimUnits(DrawPosition.Y)), ActorType.Player);
            ActorBody.BodyType = BodyType.Dynamic;
            ActorBody.Friction = 0f;
            ActorBody.Restitution = 0f;
            ActorBody.OnCollision += OnCollision;
            PointValue = 0;
        }

        public override void Update(GameTime gameTime)
        {
            if (false)
            {
                KeyboardState keyboardState = Keyboard.GetState();
                // move player sphere left if able
                if (keyboardState.IsKeyDown(Keys.Left))
                {
                    BasePosition += new Vector2(-2, 0);
                    DrawPosition += new Vector2(-2, 0);
                    ActorBody.Position = new Vector2(ConvertUnits.ToSimUnits(DrawPosition.X), ConvertUnits.ToSimUnits(DrawPosition.Y));
                }
                // move player sphere right if able
                else if(keyboardState.IsKeyDown(Keys.Right))
                {
                    BasePosition += new Vector2(2, 0);
                    DrawPosition += new Vector2(2, 0);
                    ActorBody.Position = new Vector2(ConvertUnits.ToSimUnits(DrawPosition.X), ConvertUnits.ToSimUnits(DrawPosition.Y));
                }
                // release player sphere
                else if(keyboardState.IsKeyDown(Keys.Down))
                {
                    hasBeenReleased = true;
                }
                else
                {
                    return;
                }
            }
            else
            {
                float displayX = ConvertUnits.ToDisplayUnits(ActorBody.Position.X);
                float displayY = ConvertUnits.ToDisplayUnits(ActorBody.Position.Y);
                BasePosition = new Vector2(displayX, displayY) + -OriginOffset;
                DrawPosition = BasePosition + OriginOffset;
                Rotation = ActorBody.Rotation;
            }
        }
    }
}
