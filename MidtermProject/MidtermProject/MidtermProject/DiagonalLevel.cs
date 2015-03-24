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
    class DiagonalLevel : Level
    {
        public DiagonalLevel(LevelManager thisLevelManager)
            : base(thisLevelManager)
        { }

        public override void LoadLevel()
        {
            int rowDrawPos = ThisLevelManager.PlayerSize.Y + ThisLevelManager.Spacing.Y;
            int colDrawPos = ThisLevelManager.WallSize.X + (ThisLevelManager.Spacing.X - (ThisLevelManager.PegSize.X / 2));
            bool moveLeft = true; // should peg move right or left first?
            bool moveUp = false; // should the peg move up or down first?
            bool moveHorizontal = true; // should the peg move horizontal?
            bool moveVertical = true; // should the peg move vertical?
            for (int row = 0; row < ThisLevelManager.NumberOfCompleteRows * 2 + 1; row++)
            {
                for (int col = 0; (row % 2 == 0 && col < 13) || (row % 2 == 1 && col < 12); col++)
                {
                    ThisLevelManager.ThisActorManager.AddSprite(new PegSprite(ThisLevelManager.PegTexture, new Vector2(colDrawPos, rowDrawPos), ThisLevelManager.PegSize, new Point(1, 1), new Vector2(0, 0), new Vector2(1, 1), row, new Vector2(ThisLevelManager.Spacing.X, ThisLevelManager.Spacing.Y / 2), new Vector2(1.5f, 1.5f), moveLeft, moveUp, moveHorizontal, moveVertical, false));
                    colDrawPos += ThisLevelManager.Spacing.X;
                }
                // set the colDrawPos to the start for the odd rows
                if (row % 2 == 0)
                {
                    colDrawPos = ((2 * (ThisLevelManager.WallSize.X + (ThisLevelManager.Spacing.X - (ThisLevelManager.PegSize.X / 2)))) + ThisLevelManager.Spacing.X) / 2;
                }
                // set the colDrawPos to the start for the even rows
                else
                {
                    ThisLevelManager.ThisActorManager.AddSprite(new PegSprite(ThisLevelManager.WallPegTexture, new Vector2((0 - (ThisLevelManager.WallPegSize.X / 2)) + ThisLevelManager.WallSize.X, (rowDrawPos + (ThisLevelManager.PegSize.Y / 2)) - (ThisLevelManager.WallPegSize.Y / 2)), new Point(49, 48), new Point(1, 1), new Vector2(0, 0), new Vector2(1, 1), 0, new Vector2(0, 0), new Vector2(0, 0), false, false, false, false, false));
                    ThisLevelManager.ThisActorManager.AddSprite(new PegSprite(ThisLevelManager.WallPegTexture, new Vector2(14 * ThisLevelManager.Spacing.X - (ThisLevelManager.WallPegSize.X / 2), (rowDrawPos + (ThisLevelManager.PegSize.Y / 2)) - (ThisLevelManager.WallPegSize.Y / 2)), new Point(49, 48), new Point(1, 1), new Vector2(0, 0), new Vector2(1, 1), 0, new Vector2(0, 0), new Vector2(0, 0), false, false, false, false, false));
                    colDrawPos = ThisLevelManager.WallSize.X + (ThisLevelManager.Spacing.X - (ThisLevelManager.PegSize.X / 2));
                }
                // advance the row
                rowDrawPos += ThisLevelManager.Spacing.Y;
                // alternate which direction the small row should move first
                if (moveUp && row % 2 == 1)
                {
                    moveUp = false;
                }
                // revert back to the other direction
                else if (!moveUp && row % 2 == 1)
                {
                    moveUp = true;
                }

                // alternate which direction the small row should move first
                if (moveLeft && row % 2 == 1)
                {
                    moveLeft = false;
                }
                // revert back to the other direction
                else if (!moveLeft && row % 2 == 1)
                {
                    moveLeft = true;
                }
            }
            LoadBoardAndGoal();
        }
    }
}
