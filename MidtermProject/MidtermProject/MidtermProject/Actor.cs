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
    enum ActorType { Player, GoodGoal, BadGoal, GreatGoal, HorribleGoal, NeutralGoal, Peg, Wall, None, GoalMarker };

    abstract class Actor
    {
        protected Texture2D TextureImage { get; set; }
        public Vector2 BasePosition { get; protected set; }
        protected Point FrameSize { get; set; }
        protected Point CurrentFrame { get; set; }
        protected Vector2 Velocity { get; set; }
        protected float Rotation { get; set; }
        protected Vector2 Scale { get; set; }
        protected Vector2 OriginOffset { get; set; }
        protected Vector2 DrawPosition { get; set; }
        protected Body ActorBody { get; set; }
        protected World ThisWorld { get; private set; }
        protected int PointValue { get; set; }
        public ActorType CollidedWith { get; set; }

        public Actor(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame, Vector2 velocity, Vector2 scale)
        {
            TextureImage = textureImage;
            FrameSize = frameSize;
            BasePosition = position;
            CurrentFrame = currentFrame;
            Velocity = velocity;
            Rotation = 0.0f;
            Scale = scale;
            OriginOffset = new Vector2(FrameSize.X / 2, FrameSize.Y / 2);
            DrawPosition = OriginOffset + BasePosition;
            ThisWorld = PhysicsWorldManager.ThisWorld;
            CollidedWith = ActorType.None;            
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureImage, DrawPosition, new Rectangle(CurrentFrame.X, CurrentFrame.Y, FrameSize.X, FrameSize.Y), Color.White, Rotation, OriginOffset, Scale, SpriteEffects.None, 0f);
        }

        public virtual void Update(GameTime gameTime) { }


        public bool OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (!GameManager.LevelEnd)
            {
                ActorType colliderType = (ActorType)fixtureA.UserData;
                ActorType collidedType = (ActorType)fixtureB.UserData;
                if (colliderType == ActorType.Player)
                {
                    switch (collidedType)
                    {
                        case ActorType.GoodGoal:
                            this.CollidedWith = collidedType;
                            SoundManager.PlaySound("score");
                            return true;
                        case ActorType.GreatGoal:
                            this.CollidedWith = collidedType;
                            SoundManager.PlaySound("score");
                            return true;
                        case ActorType.BadGoal:
                            this.CollidedWith = collidedType;
                            SoundManager.PlaySound("BadGoalSound");
                            return true;
                        case ActorType.HorribleGoal:
                            this.CollidedWith = collidedType;
                            SoundManager.PlaySound("BadGoalSound");
                            return true;
                        case ActorType.NeutralGoal:
                            this.CollidedWith = collidedType;
                            SoundManager.PlaySound("NeutralGoalSound");
                            return true;
                        case ActorType.Wall:
                            SoundManager.PlaySound("hit");
                            return true;
                        case ActorType.Peg:
                            SoundManager.PlaySound("hit");
                            return true;
                        case ActorType.None:
                            return false;
                        case ActorType.GoalMarker:
                            return false;
                        default:
                            return true;
                    }
                }
            }
            return false;
        }

        public void MoveToNewLocation(Vector2 movementVector)
        {
            ActorBody.ApplyLinearImpulse(movementVector);
            DrawPosition = new Vector2(ConvertUnits.ToDisplayUnits(ActorBody.Position.X), ConvertUnits.ToDisplayUnits(ActorBody.Position.Y));
            BasePosition = DrawPosition + -OriginOffset;
        }

        public void DestroyPhysicsBody()
        {
            ActorBody.Dispose();
        }
    }
}
