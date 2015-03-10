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
    class PhysicsWorldManager
    {
        public static World ThisWorld { get; private set; }

        public PhysicsWorldManager()
        {
            ThisWorld = new World(new Vector2(0f, 1.0f));
        }

        public static void UpdateWorld(GameTime gameTime)
        {
            ThisWorld.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
        }
    }
}
