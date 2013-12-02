using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SWE6753_Project
{
    public class TargetBuildHelper
    {
        private const int TARGETRELEASETIME = 1400;   
        private const string CRATE5 = "CRATE10";
        private const string CRATE3 = "CRATE3";
        private const string CRATE1 = "CRATE1";
        private const string BRIDGE = "BRIDGE";
        private const string DYNAMITE = "DYNAMITE";
        private const string EXPLOSIVE = "EXPLOSIVE";
    
        private int _elapsedSinceLastTarget;
        private Random _randomGenerator;
        private GameContent _content;
        private int _viewWidth;
        private int _viewHeight;
        private SpriteBatch _spriteBatch;
        private string[] _originalList, _currRoundList;
        

        public TargetBuildHelper(GameContent content, int viewWidth, int viewHeight, SpriteBatch sb)
        {
            _randomGenerator = new Random();
            _content = content;
            _viewWidth = viewWidth;
            _viewHeight = viewHeight;
            _spriteBatch = sb;
            GenerateTargetArrays();
        }       

        public TargetObject GetNextTargetOrNull(GameTime gameTime)
        {
            _elapsedSinceLastTarget += gameTime.ElapsedGameTime.Milliseconds;

            if (_elapsedSinceLastTarget >= TARGETRELEASETIME)
            {
                _elapsedSinceLastTarget -= TARGETRELEASETIME;

                int pointer = _randomGenerator.Next(0, 39);
                string targetType = string.Empty;

                for (var i = 0; i < 40; i++)
                {
                    targetType = _currRoundList[pointer];
                    if (!string.IsNullOrEmpty(targetType))
                    {
                        _currRoundList[pointer] = string.Empty;
                        break;
                    }
                    else
                    {
                        pointer++;
                        pointer = pointer > 39 ? 0 : pointer;
                    }
                }

                switch (targetType)
                {
                    case CRATE1: return CreateLargeTarget();
                    case CRATE3: return CreateMediumTarget();
                    case CRATE5: return CreateSmallTarget();
                    case BRIDGE: return CreateBridge();
                    case DYNAMITE: return CreateDynamite();
                    case EXPLOSIVE: return CreateExplosive();
                    default: return null;
                }                
            }
            else
            {
                return null;
            }
        }           

        public void Stop()
        {         
            _elapsedSinceLastTarget = 0;
            _currRoundList = _originalList.ToArray();
        }       

        private TargetObject CreateBridge()
        {
            Vector2 position;
            Vector2 velocity;
            GetPositionAndVelocity(out position, out velocity, 3f, 2f);
            if (position.Y < 200) position.Y = 200;

            return new TargetObject(_content.Bridge, velocity, position,
                                     70, 35, _viewWidth, _viewHeight, _spriteBatch,
                                     TargetTypeEnum.Bridge, 0, _content.GameFont);
        }

        private TargetObject CreateExplosive()
        {
            Vector2 position;
            Vector2 velocity;
            GetPositionAndVelocity(out position, out velocity, 0.8f, 0.3f);

            return new TargetObject(_content.Explosive, velocity, position,
                                     80, 80, _viewWidth, _viewHeight, _spriteBatch,
                                     TargetTypeEnum.Explosive, -3, _content.GameFont);
        }        

        private TargetObject CreateDynamite()
        {
            Vector2 position;
            Vector2 velocity;
            GetPositionAndVelocity(out position, out velocity, 0.8f, 0.3f);

            return new TargetObject(_content.Dynamite, velocity, position,
                                     80, 40, _viewWidth, _viewHeight, _spriteBatch,
                                     TargetTypeEnum.Explosive, -3, _content.GameFont);
        }

        private TargetObject CreateLargeTarget()
        {
            Vector2 position;
            Vector2 velocity;
            GetPositionAndVelocity(out position, out velocity, 2f, 0.7f);
            velocity.Y = 0;

            return new TargetObject(_content.DiagonalCrate, velocity, position,
                                     40, 40, _viewWidth, _viewHeight, _spriteBatch,
                                     TargetTypeEnum.Crate, 1, _content.GameFont);
        }

        private TargetObject CreateMediumTarget()
        {
            Vector2 position;
            Vector2 velocity;
            GetPositionAndVelocity(out position, out velocity, 3f, 1.5f);
            velocity.Y = 0;

            return new TargetObject(_content.DiagonalCrate, velocity, position,
                                     30, 30, _viewWidth, _viewHeight, _spriteBatch,
                                     TargetTypeEnum.Crate, 3, _content.GameFont);
        }

        private TargetObject CreateSmallTarget()
        {
            Vector2 position;
            Vector2 velocity;
            GetPositionAndVelocity(out position, out velocity, 4f, 2f);

            return new TargetObject(_content.ArrowCrate, velocity, position,
                                     20, 20, _viewWidth, _viewHeight, _spriteBatch,
                                     TargetTypeEnum.Crate, 5, _content.GameFont);
        }

        private void GetPositionAndVelocity(out Vector2 position, out Vector2 velocity, float velocityMax, float velocityMin)
        {
            position = new Vector2();
            velocity = new Vector2();
            velocity.X = (float)(_randomGenerator.NextDouble() * velocityMax + velocityMin);
            velocity.Y = 0f; // (float)(_randomGenerator.NextDouble() * 0.05 + .001);
            if (_randomGenerator.Next(-1, 1) < 0)
            {
                velocity.X *= -1;
                position.X = _viewWidth;                
            }
            else
            {
                position.X = 0;                
            }

            position.Y = _randomGenerator.Next(20, _viewHeight - 300);
        }

        private void GenerateTargetArrays()
        {
            _originalList = new string[40];

            //create crate5 at 0, 10, 20, 30
            for (var i = 0; i < 40; i += 10)
            {
                _originalList[i] = CRATE5;
            }
            //create dynamite at 4, 9, 14, 19, 24, 29, 34, 39
            for (var i = 4; i < 34; i += 5)
            {
                _originalList[i] = DYNAMITE;
            }
            //create explosive at 1, 6, 11, 16, 21, 26, 31, 36
            for (var i = 1; i < 34; i += 5)
            {
                _originalList[i] = EXPLOSIVE;
            }

            //create bridges 3
            _originalList[2] = BRIDGE; _originalList[12] = BRIDGE; _originalList[32] = BRIDGE;

            //create crate3 (7)
            _originalList[3] = CRATE3; _originalList[8] = CRATE3; _originalList[13] = CRATE3;
            _originalList[18] = CRATE3; _originalList[22] = CRATE3; _originalList[28] = CRATE3; _originalList[35] = CRATE3;

            //create crate1 (10)
            _originalList[5] = CRATE1; _originalList[7] = CRATE1; _originalList[15] = CRATE1; _originalList[17] = CRATE1;
            _originalList[23] = CRATE1; _originalList[25] = CRATE1; _originalList[27] = CRATE3;
            _originalList[33] = CRATE1; _originalList[37] = CRATE1; _originalList[38] = CRATE1;
            _originalList[34] = CRATE1; _originalList[39] = CRATE1; _originalList[31] = CRATE1; _originalList[36] = CRATE1;

            _currRoundList = _originalList.ToArray();
        }

    }
}
