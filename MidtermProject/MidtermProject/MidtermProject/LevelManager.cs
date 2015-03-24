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
    class LevelManager
    {
        public Level[] Levels { get; private set; }
        public int LevelsRemaining { get; private set; }
        public int BaseSize { get; private set; } // size of all wall like objects (width or height depending on the sprite)
        public int NumberOfCompleteRows { get; private set; } // the number of complete rows
        public Point PegSize { get; private set; } // size of the peg sprite
        public Point PlayerSize { get; private set; } // size of the player sprite
        public Point Spacing { get; private set; } // spacing between rows and cols
        public Point WallSize { get; private set; } // size of the walls
        public Point GoalSize { get; private set; } // size of the goal sprite
        public Point GoalPostSize { get; private set; } // size of the goal post sprite
        public Point WallPegSize { get; private set; } // size of the wall peg
        public Point GoalMarkerSize { get; private set; } // size of the goal marker sprite 
        public ActorManager ThisActorManager { get; private set; } // reference to the actor manager
        public Game ThisGame { get; private set; } // reference to the game

        public Texture2D GoalTexture { get; private set; }
        public Texture2D WallPegTexture { get; private set; }
        public Texture2D PegTexture { get; private set; }
        public Texture2D BadGoalMarkerTexture { get; private set; }
        public Texture2D GoodGoalMarkerTexture { get; private set; }
        public Texture2D GreatGoalMarkerTexture { get; private set; }
        public Texture2D HorribleGoalMarkerTexture { get; private set; }
        public Texture2D NeutralGoalMarkerTexture { get; private set; }
        public Texture2D GoalPostTexture { get; private set; }
        public Texture2D WallTexture { get; private set; }
        public Texture2D PlayerCircleTexture { get; private set; }

        private int LevelIndex { get; set; }

        public LevelManager(int baseSize, int numberOfCompleteRows, Point[] usedPoints, ActorManager actorManager, Game thisGame, Texture2D[] usedTextures) 
        {
            BaseSize = baseSize;
            NumberOfCompleteRows = numberOfCompleteRows;

            PegSize = usedPoints[0];
            PlayerSize = usedPoints[1];
            Spacing = usedPoints[2];
            WallSize = usedPoints[3];
            GoalSize = usedPoints[4];
            GoalPostSize = usedPoints[5];
            WallPegSize = usedPoints[6];
            GoalMarkerSize = usedPoints[7];

            ThisActorManager = actorManager;
            ThisGame = thisGame;

            GoalTexture = usedTextures[0];
            WallPegTexture = usedTextures[1];
            PegTexture = usedTextures[2];
            BadGoalMarkerTexture = usedTextures[3];
            GoodGoalMarkerTexture = usedTextures[4];
            GreatGoalMarkerTexture = usedTextures[5];
            HorribleGoalMarkerTexture = usedTextures[6];
            NeutralGoalMarkerTexture = usedTextures[7];
            GoalPostTexture = usedTextures[8];
            WallTexture = usedTextures[9];
            PlayerCircleTexture = usedTextures[10];

            LevelIndex = 0;
            Levels = new Level[5];
            Levels[0] = new StaticLevel(this);
            Levels[1] = new HorizontalLevel(this);
            Levels[2] = new VerticalLevel(this);
            Levels[3] = new DiagonalLevel(this);
            Levels[4] = new ComboLevel(this);            
            LevelsRemaining = Levels.Length;

        }

        public void LoadNextLevel() 
        {
            Levels[LevelIndex].LoadLevel();
            LevelIndex = (LevelIndex + 1) % NumberOfLevels();
            LevelsRemaining = LevelsRemaining - 1;
        }
        public int NumberOfLevels() 
        {
            return Levels.Length;
        }
        public void UnloadCurrentLevel() 
        {
            ThisActorManager.UnloadSpriteList();
        }

        public int DropLocation()
        {
            if (LevelIndex - 1 > -1)
            {
                return Levels[LevelIndex - 1].DropLocation;
            }
            else
            {
                return Levels[Levels.Length - 1].DropLocation;
            }
            
        }

        public void ResetGame()
        {
            LevelIndex = 0;
            LevelsRemaining = Levels.Length;
            UnloadCurrentLevel();
            Levels[0] = new StaticLevel(this);
            Levels[1] = new HorizontalLevel(this);
            Levels[2] = new VerticalLevel(this);
            Levels[3] = new DiagonalLevel(this);
            Levels[4] = new ComboLevel(this); 
            LoadNextLevel();
        }
    }
}
