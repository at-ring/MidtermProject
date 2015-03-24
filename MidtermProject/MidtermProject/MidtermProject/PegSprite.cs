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
    class PegSprite : Actor
    {
        // which row this peg is a part of
        protected int Row { get; set; }
        // the max distance + or - that the peg can get from the original position
        protected Vector2 MaxDisplacement { get; set; }
        // the original position of the peg
        protected Vector2 DefaultPosition { get; set; }
        // the speed the peg moves at (different than velocity)
        protected Vector2 MovementSpeed { get; set; }
        // the direction of horizontal movement
        protected bool MoveLeft { get; set; }
        // the direction of vertical movement
        protected bool MoveUp { get; set; }
        // allow horizontal movement
        protected bool MoveHorizontal { get; set; }
        // allow vertical movement
        protected bool MoveVertical { get; set; }
        // are there different types of movements
        protected bool Combo { get; set; }

        public PegSprite(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame, Vector2 velocity, Vector2 scale, int row, Vector2 maxDisplacement, Vector2 movementSpeed, bool moveLeft, bool moveUp, bool moveHorizontal, bool moveVertical, bool combo)
            : base(textureImage, position, new Point(frameSize.X - 1, frameSize.Y - 1), currentFrame, velocity, scale)
        {
            ActorBody = BodyFactory.CreateCircle(ThisWorld, ConvertUnits.ToSimUnits((FrameSize.X * Scale.X) / 2), 100, new Vector2(ConvertUnits.ToSimUnits(DrawPosition.X), ConvertUnits.ToSimUnits(DrawPosition.Y)), ActorType.Peg);
            ActorBody.BodyType = BodyType.Static;
            ActorBody.Friction = 0f;
            ActorBody.Restitution = 0f;
            PointValue = 0;
            Row = row;
            MaxDisplacement = maxDisplacement;
            MovementSpeed = movementSpeed;
            DefaultPosition = position;
            MoveLeft = moveLeft;
            MoveUp = moveUp;
            MoveVertical = moveVertical;
            MoveHorizontal = moveHorizontal;
            Combo = combo;
        }

        public override void Update(GameTime gameTime)
        {
            // are there horizontal and vertical movement only
            if (!Combo)
            {
                if (Row % 2 == 1)
                {
                    if (MoveHorizontal)
                    {
                        // if we want to continue moving left
                        if (MoveLeft)
                        {
                            BasePosition = new Vector2(BasePosition.X + -MovementSpeed.X, BasePosition.Y);
                            DrawPosition = BasePosition + OriginOffset;
                            ActorBody.Position = new Vector2(ConvertUnits.ToSimUnits(DrawPosition.X), ConvertUnits.ToSimUnits(DrawPosition.Y));
                            if (BasePosition.X <= DefaultPosition.X + -MaxDisplacement.X)
                            {
                                MoveLeft = false;
                            }

                        }
                        // if we want to switch to moving right
                        else
                        {
                            BasePosition = new Vector2(BasePosition.X + MovementSpeed.X, BasePosition.Y);
                            DrawPosition = BasePosition + OriginOffset;
                            ActorBody.Position = new Vector2(ConvertUnits.ToSimUnits(DrawPosition.X), ConvertUnits.ToSimUnits(DrawPosition.Y));
                            if (BasePosition.X >= DefaultPosition.X + MaxDisplacement.X)
                            {
                                MoveLeft = true;
                            }
                        }
                    }
                    if (MoveVertical)
                    {
                        // if we want to continue moving up
                        if (MoveUp)
                        {
                            BasePosition = new Vector2(BasePosition.X, BasePosition.Y + -MovementSpeed.Y);
                            DrawPosition = BasePosition + OriginOffset;
                            ActorBody.Position = new Vector2(ConvertUnits.ToSimUnits(DrawPosition.X), ConvertUnits.ToSimUnits(DrawPosition.Y));
                            if (BasePosition.Y <= DefaultPosition.Y + -MaxDisplacement.Y)
                            {
                                MoveUp = false;
                            }
                        }
                        // if we want to switch to moving down
                        else
                        {
                            BasePosition = new Vector2(BasePosition.X, BasePosition.Y + MovementSpeed.Y);
                            DrawPosition = BasePosition + OriginOffset;
                            ActorBody.Position = new Vector2(ConvertUnits.ToSimUnits(DrawPosition.X), ConvertUnits.ToSimUnits(DrawPosition.Y));
                            if (BasePosition.Y >= DefaultPosition.Y + MaxDisplacement.Y)
                            {
                                MoveUp = true;
                            }
                        }
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                if (Row % 4 == 1)
                {
                    // if we want to continue moving left
                    if (MoveLeft)
                    {
                        BasePosition = new Vector2(BasePosition.X + -MovementSpeed.X, BasePosition.Y);
                        DrawPosition = BasePosition + OriginOffset;
                        ActorBody.Position = new Vector2(ConvertUnits.ToSimUnits(DrawPosition.X), ConvertUnits.ToSimUnits(DrawPosition.Y));
                        if (BasePosition.X <= DefaultPosition.X + -MaxDisplacement.X)
                        {
                            MoveLeft = false;
                        }

                    }
                    // if we want to switch to moving right
                    else
                    {
                        BasePosition = new Vector2(BasePosition.X + MovementSpeed.X, BasePosition.Y);
                        DrawPosition = BasePosition + OriginOffset;
                        ActorBody.Position = new Vector2(ConvertUnits.ToSimUnits(DrawPosition.X), ConvertUnits.ToSimUnits(DrawPosition.Y));
                        if (BasePosition.X >= DefaultPosition.X + MaxDisplacement.X)
                        {
                            MoveLeft = true;
                        }
                    }
                }
                else if (Row % 4 == 3)
                {
                    // if we want to continue moving up
                    if (MoveUp)
                    {
                        BasePosition = new Vector2(BasePosition.X, BasePosition.Y + -MovementSpeed.Y);
                        DrawPosition = BasePosition + OriginOffset;
                        ActorBody.Position = new Vector2(ConvertUnits.ToSimUnits(DrawPosition.X), ConvertUnits.ToSimUnits(DrawPosition.Y));
                        if (BasePosition.Y <= DefaultPosition.Y + -MaxDisplacement.Y)
                        {
                            MoveUp = false;
                        }
                    }
                    // if we want to switch to moving down
                    else
                    {
                        BasePosition = new Vector2(BasePosition.X, BasePosition.Y + MovementSpeed.Y);
                        DrawPosition = BasePosition + OriginOffset;
                        ActorBody.Position = new Vector2(ConvertUnits.ToSimUnits(DrawPosition.X), ConvertUnits.ToSimUnits(DrawPosition.Y));
                        if (BasePosition.Y >= DefaultPosition.Y + MaxDisplacement.Y)
                        {
                            MoveUp = true;
                        }
                    }
                }
            }            
        }
    }
}
