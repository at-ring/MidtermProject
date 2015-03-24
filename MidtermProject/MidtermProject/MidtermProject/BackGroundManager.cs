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
    class BackGroundManager
    {
        private Texture2D[] Backgrounds { get; set; }
        private Point ScreenDimentions { get; set; }
        private Texture2D CurrentBackground { get; set; }

        public BackGroundManager(Texture2D[] backgrounds, Point newScreenDimentions) 
        {
            Backgrounds = backgrounds;
            ScreenDimentions = newScreenDimentions;
        }

        public void  DrawBackground (GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(CurrentBackground, new Vector2(0 , 0), new Rectangle(0, 0, ScreenDimentions.X, ScreenDimentions.Y), Color.White);
        }

        public void ChangeScreenDimentions(Point newScreenDimentions)
        {
            ScreenDimentions = newScreenDimentions;
        }

        public void ChooseNewBackground()
        {
            Random random = new Random();
            CurrentBackground = Backgrounds[random.Next(Backgrounds.Length)];
        }
    }
}
