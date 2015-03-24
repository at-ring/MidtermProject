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
        private Vector2 OriginalPosition { get; set; }
        public bool HasBeenReleased { get; private set; }

        public PlayerSprite(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame, Vector2 velocity, Vector2 scale)
            : base(textureImage, position, frameSize, currentFrame, velocity, scale)
        {
            ActorBody = BodyFactory.CreateCircle(ThisWorld, ConvertUnits.ToSimUnits((FrameSize.X * Scale.X) / 2), 10, new Vector2(ConvertUnits.ToSimUnits(DrawPosition.X), ConvertUnits.ToSimUnits(DrawPosition.Y)), ActorType.Player);
            ActorBody.BodyType = BodyType.Dynamic;
            ActorBody.Restitution = .25f;
            ActorBody.Friction = 0f;
            ActorBody.OnCollision += OnCollision;
            PointValue = 0;
            ActorBody.IgnoreGravity = true;
            OriginalPosition = position;
            HasBeenReleased = false;
        }

        public override void Update(GameTime gameTime)
        {
            float displayX = ConvertUnits.ToDisplayUnits(ActorBody.Position.X);
            float displayY = ConvertUnits.ToDisplayUnits(ActorBody.Position.Y);
            BasePosition = new Vector2(displayX, displayY) + -OriginOffset;
            DrawPosition = BasePosition + OriginOffset;
            Rotation = ActorBody.Rotation;
        }

        public void ResetPosition()
        {
            ActorBody.Awake = false;
            ActorBody.IgnoreGravity = true;
            HasBeenReleased = false;
            BasePosition = OriginalPosition;
            DrawPosition = BasePosition + OriginOffset;
            ActorBody.Position = new Vector2(ConvertUnits.ToSimUnits(DrawPosition.X), ConvertUnits.ToSimUnits(DrawPosition.Y));
        }

        public void Release()
        {
            ActorBody.IgnoreGravity = false;
            ActorBody.Awake = true;
            ActorBody.Friction = 0;
            HasBeenReleased = true;
        }
    }
}
