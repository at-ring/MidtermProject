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
    abstract class Level
    {
        protected LevelManager ThisLevelManager { get; private set; }
        public int DropLocation { get; private set; }


        public Level(LevelManager thisLevelManager)
        {
            ThisLevelManager = thisLevelManager;
        }

        public virtual void LoadLevel() { }

        protected void LoadBoardAndGoal()
        {
            ActorType[] goalTypes = new ActorType[14];
            // create list of goals
            for (int i = 0; i < 14; i++)
            {
                if (i <= 1)
                {
                    goalTypes[i] = ActorType.GreatGoal;
                }
                else if (i <= 3)
                {
                    goalTypes[i] = ActorType.HorribleGoal;
                }
                else if (i <= 6)
                {
                    goalTypes[i] = ActorType.GoodGoal;
                }
                else if (i <= 9)
                {
                    goalTypes[i] = ActorType.BadGoal;
                }
                else
                {
                    goalTypes[i] = ActorType.NeutralGoal;
                }
            }
            // shuffle goals
            Random random = new Random();
            for (int i = 0; i < 100; i++)
            {
                int swapLoc1 = random.Next(14);
                int swapLoc2 = random.Next(14);
                ActorType temp = goalTypes[swapLoc1];
                goalTypes[swapLoc1] = goalTypes[swapLoc2];
                goalTypes[swapLoc2] = temp;
            }

            GetDropLocation(goalTypes);

            // create goals
            int goalDrawLocX = ThisLevelManager.WallSize.X;
            int goalDrawLocY = ThisLevelManager.PlayerSize.Y + (((ThisLevelManager.NumberOfCompleteRows + 1) * 2) * ThisLevelManager.Spacing.Y) + ThisLevelManager.Spacing.Y;
            for (int i = 0; i < 14; i++)
            {
                Texture2D textureToUse;
                if (goalTypes[i] == ActorType.BadGoal)
                {
                    textureToUse = ThisLevelManager.BadGoalMarkerTexture;
                }
                else if (goalTypes[i] == ActorType.GoodGoal)
                {
                    textureToUse = ThisLevelManager.GoodGoalMarkerTexture;
                }
                else if (goalTypes[i] == ActorType.GreatGoal)
                {
                    textureToUse = ThisLevelManager.GreatGoalMarkerTexture;
                }
                else if (goalTypes[i] == ActorType.HorribleGoal)
                {
                    textureToUse = ThisLevelManager.HorribleGoalMarkerTexture;
                }
                else
                {
                    textureToUse = ThisLevelManager.NeutralGoalMarkerTexture;
                }
                ThisLevelManager.ThisActorManager.AddSprite(new GoalMarkerSprite(textureToUse, new Vector2(goalDrawLocX, goalDrawLocY - ThisLevelManager.Spacing.Y), ThisLevelManager.GoalMarkerSize, new Point(1, 1), Vector2.Zero, new Vector2(1, 1)));
                ThisLevelManager.ThisActorManager.AddSprite(new GoalSprite(ThisLevelManager.GoalTexture, new Vector2(goalDrawLocX, goalDrawLocY), ThisLevelManager.GoalSize, new Point(1, 1), Vector2.Zero, new Vector2(1, 1), goalTypes[i]));
                goalDrawLocX += ThisLevelManager.Spacing.X;
            }

            // create goal posts
            int goalPostDrawLocX = ThisLevelManager.WallSize.X + (ThisLevelManager.Spacing.X - (ThisLevelManager.GoalPostSize.X / 2));
            int goalPostDrawLocY = ThisLevelManager.PlayerSize.Y + (((ThisLevelManager.NumberOfCompleteRows + 1) * 2) * ThisLevelManager.Spacing.Y);
            for (int i = 0; i < 13; i++)
            {
                ThisLevelManager.ThisActorManager.AddSprite(new WallSprite(ThisLevelManager.GoalPostTexture, new Vector2(goalPostDrawLocX, goalPostDrawLocY), ThisLevelManager.GoalPostSize, new Point(1, 1), Vector2.Zero, new Vector2(1, 1)));
                goalPostDrawLocX += ThisLevelManager.Spacing.X;
            }

            // create walls
            ThisLevelManager.ThisActorManager.AddSprite(new WallSprite(ThisLevelManager.WallTexture, new Vector2(0, 0), new Point(10, 1950), new Point(1, 1), Vector2.Zero, new Vector2(1, 1)));
            ThisLevelManager.ThisActorManager.AddSprite(new WallSprite(ThisLevelManager.WallTexture, new Vector2(14 * ThisLevelManager.Spacing.X, 0), new Point(10 + (ThisLevelManager.WallPegSize.X / 2), 1950), new Point(1, 1), Vector2.Zero, new Vector2(1, 1)));

            // create player
            ThisLevelManager.ThisActorManager.PlayerSpriteReference = new PlayerSprite(ThisLevelManager.PlayerCircleTexture, new Vector2(ThisLevelManager.WallSize.X + 1, 0), new Point(49, 48), new Point(1, 1), Vector2.Zero, new Vector2(1, 1));
            ThisLevelManager.ThisActorManager.AddSprite(ThisLevelManager.ThisActorManager.PlayerSpriteReference);
        }

        private void GetDropLocation(ActorType[] goals)
        {
            int goalToDropOver = 0;
            // check the left edge goals
            int potentialScore = ScoreLocation(goals[0]);
            goalToDropOver = 0;

            // check the middle goals
            for (int i = 1; i < goals.Length - 1; i++)
            {
                int leftValue = ScoreLocation(goals[i - 1]);
                int centerValue = ScoreLocation(goals[i]);
                int rightValue = ScoreLocation(goals[i + 1]);
                int totalScore = leftValue + centerValue + rightValue;
                if (totalScore >= potentialScore)
                {
                    potentialScore = totalScore;
                    goalToDropOver = i;
                }
                //Console.WriteLine("i: {0}, lv: {1}, cv: {2}, rv: {3}, ts: {4}, gd: {5}", i, leftValue, centerValue, rightValue, totalScore, goalToDropOver);
            }
            // check the right edge goal
            if ((ScoreLocation(goals[goals.Length - 1]) + ScoreLocation(goals[goals.Length - 2]) )>= potentialScore)
            {
                goalToDropOver = goals.Length - 1;
            }
            if (((ScoreLocation(goals[0]) + ScoreLocation(goals[1]) )>= potentialScore))
            {
                goalToDropOver = 0;
            }

            if (goalToDropOver < goals.Length / 2)
            {
                DropLocation = ThisLevelManager.WallSize.X + (goalToDropOver * ThisLevelManager.Spacing.X);
            }
            else
            {
                DropLocation = ThisLevelManager.WallSize.X + (Math.Max(goalToDropOver - 1, 0) * ThisLevelManager.Spacing.X);
            }
            
        }

        // convert from goal type to value to decide were to drop the ball
        private int ScoreLocation(ActorType goalValue)
        {
            if (goalValue == ActorType.BadGoal)
            {
                return -5;
            }
            else if (goalValue == ActorType.GoodGoal)
            {
                return 5;
            }
            else if (goalValue == ActorType.GreatGoal)
            {
                return 10;
            }
            else if (goalValue == ActorType.HorribleGoal)
            {
                return -10;
            }
            else
            {
                return 0;
            }
        }
    }
}
