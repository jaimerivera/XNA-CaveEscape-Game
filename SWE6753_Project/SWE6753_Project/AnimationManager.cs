using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SWE6753_Project.AnimationStates;

namespace SWE6753_Project
{
    /// <summary>
    /// Class to manage the animation of the characters
    /// </summary>
    public class AnimationManager
    {
        Player _player;
        RoundManager _roundManager;

        public Texture2D _charTex, _lift, _bridge, _lineTex;
        public Rectangle _charRect, _sourceRect;
        SpriteBatch _spriteBatch;
        public static Rectangle _liftRect1, _liftRect2, _liftRect3;
        public Rectangle _lineRect1, _lineRect2, _lineRect3;
        private Rectangle _bridge1Rect, _bridge2Rect;
        SpriteFont _font;
        string _playerText;
        Vector2 _textPosition, _charOrigin;
        Color _lineColor;
        private AudioManager _audioManager;        


        public AnimationManager(Player player, GameContent content, SpriteBatch spriteBatch, 
                                RoundManager roundManager, AudioManager audioManager)
        {
            _player = player;
            _roundManager = roundManager;
            _spriteBatch = spriteBatch;
            _audioManager = audioManager;

            _charTex = content.Luigi;
            _lift = content.Lift;
            _font = content.GameFont;
            _bridge = content.Bridge;
            _lineTex = content.LineTexture;

            SetPlayerRectanglesAndColor();

            _textPosition = new Vector2();

            _liftRect1 = new Rectangle(1148, 528, 90, 20);
            _liftRect2 = new Rectangle(43, 400, 90, 20);
            _liftRect3 = new Rectangle(1148, 270, 90, 20);

            _lineRect1 = new Rectangle();
            _lineRect2 = new Rectangle();
            _lineRect3 = new Rectangle();

            CreateAndUpdateAnimationStates();
            this.AnimationIsCompleted = true;
            _charOrigin = new Vector2();

            _bridge1Rect = new Rectangle(309, 392, 30, 30);
            _bridge2Rect = new Rectangle(869, 263, 30, 30);
            
        }        

        public BaseAnimationState AnimationState { get; set; }
        public Player Player { get { return _player; } }
        public bool AnimationIsCompleted { get; set; }
        public bool FlipHorizontally { get; set; }
        public bool Animating { get; set; }

        public void Reset()
        {
            CreateAndUpdateAnimationStates();
            UpdateLineRectangles();
            SetPlayerRectanglesAndColor();

            _textPosition.X = _charRect.X + 5;
            _textPosition.Y = _charRect.Y - 25;
        }

        public void Update(GameTime gameTime)
        {
            _textPosition.X = _charRect.X + 5;
            _textPosition.Y = _charRect.Y - 25;

            UpdateLineRectangles();
            AnimationState.Update(gameTime);

            //_playerText = Math.Round((double)_charRect.X).ToString() + "," + Math.Round((double)_charRect.Y).ToString();
        }

        public void Draw()
        {
            if (this.FlipHorizontally)
            {
                _spriteBatch.Draw(_charTex, _charRect, _sourceRect, Color.White, 0f, _charOrigin, SpriteEffects.FlipHorizontally, 0);
            }
            else
            {
                _spriteBatch.Draw(_charTex, _charRect, _sourceRect, Color.White, 0f, _charOrigin, SpriteEffects.None, 0);
            }
            
            _spriteBatch.DrawString(_font, _playerText, _textPosition, Color.White);

            _spriteBatch.Draw(_lift, _liftRect1, Color.White);
            _spriteBatch.Draw(_lift, _liftRect2, Color.White);
            _spriteBatch.Draw(_lift, _liftRect3, Color.White);

            if (!_lineRect1.IsEmpty) _spriteBatch.Draw(_lineTex, _lineRect1, _lineColor);
            if (!_lineRect2.IsEmpty) _spriteBatch.Draw(_lineTex, _lineRect2, _lineColor);
            if (!_lineRect3.IsEmpty) _spriteBatch.Draw(_lineTex, _lineRect3, _lineColor);

            if (_player.HasBridge && Animating)
            {
                _spriteBatch.Draw(_bridge, _bridge1Rect, Color.White);
                _spriteBatch.Draw(_bridge, _bridge2Rect, Color.White);
            }
        }

        private int GetLineHeight(int scoreValue)
        {
            if (_player.PlayerNumber == 1)
            {
                if (scoreValue < 22) return 560;
                else if (scoreValue < 41) return 430;
                else return 300;
            }
            else
            {
                if (scoreValue < 22) return 550;
                else if (scoreValue < 41) return 420;
                else return 290;
            }
        }

        private void UpdateLineRectangles()
        {
            int rect1Result = 0;
            int rect2Result = 0;

            int length = _player.RoundScore * 55;
            int startY = GetLineHeight(_player.TotalScore);

            if (_player.TotalScore < 22)
            {
                rect1Result = UpdateRect1(_player.TotalScore, startY, length);
            }
            else
            {
                _lineRect1 = Rectangle.Empty;
            }


            if (_player.TotalScore > 21)
            {
                rect2Result = UpdateRect2(_player.TotalScore, startY, length);
            }
            else if (rect1Result > 0)
            {
                rect2Result = UpdateRect2(21, GetLineHeight(22), rect1Result);
            }
            else
            {
                _lineRect2 = Rectangle.Empty;
            }

            if (_player.TotalScore > 40)
            {
                UpdateRect3(_player.TotalScore, startY, length);
            }
            else if (rect2Result > 0)
            {
                UpdateRect3(40, GetLineHeight(41), rect2Result);
            }
            else
            {
                _lineRect3 = Rectangle.Empty;
            }
        }

        public int UpdateRect1(int scoreValue, int startY, int length)
        {
            if (scoreValue < 22)
            {
                _lineRect1.X = scoreValue * 55 + 45;
                _lineRect1.Y = startY;
                _lineRect1.Width = length > 0 ? length : 5;
                _lineRect1.Height = 3;

                if ((_lineRect1.X + length) > 1200)
                {
                    int extra = _lineRect1.X + length - 1200;
                    _lineRect1.Width = length - extra;

                    return extra;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                _lineRect1 = Rectangle.Empty;
                return 0;
            }
        }

        public int UpdateRect2(int scoreValue, int startY, int length)
        {
            if (scoreValue < 41)
            {
                _lineRect2.Width = length > 0 ? length : 5;
                _lineRect2.X = 1110 - ((scoreValue - 21) * 55) - length;
                _lineRect2.Y = startY;
                _lineRect2.Height = 3;

                if (_lineRect2.X < 65)
                {
                    var diff = 65 - _lineRect2.X;
                    _lineRect2.X = 65;
                    _lineRect2.Width = length - diff;
                    return diff;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                _lineRect2 = Rectangle.Empty;
                return 0;
            }
        }

        public void UpdateRect3(int scoreValue, int startY, int length)
        {
            _lineRect3.X = (scoreValue - 40) * 55 + 140;
            _lineRect3.Y = startY;
            _lineRect3.Width = length > 0 ? length : 5;
            _lineRect3.Height = 3;

            if ((_lineRect3.X + length) > 1185)
            {
                int extra = _lineRect3.X + length - 1185;
                _lineRect3.Width = length - extra;
            }
        }

        private void CreateAndUpdateAnimationStates()
        {
            BaseAnimationState idle = new IdleAnimation(this, _roundManager, _audioManager);
            BaseAnimationState advFirstLevel = new AdvanceFirstLevel(this, _roundManager, _audioManager);
            BaseAnimationState ascRight1 = new AscendingRight1(this, _roundManager, _audioManager);
            BaseAnimationState advSecondlevel = new AdvanceSecondLevel(this, _roundManager, _audioManager);
            BaseAnimationState fallTo1 = new FallToFirstLevel(this, _roundManager, _audioManager);
            BaseAnimationState ascLeft = new AscendingLeft(this, _roundManager, _audioManager);
            BaseAnimationState advThirdLevel = new AdvanceThirdLevel(this, _roundManager, _audioManager);
            BaseAnimationState fallTo2 = new FallToSecondLevel(this, _roundManager, _audioManager);
            BaseAnimationState ascRight2 = new AscendingRight2(this, _roundManager, _audioManager);
            BaseAnimationState win = new WinAnimation(this, _roundManager, _audioManager);

            idle.NextState = advFirstLevel;

            advFirstLevel.IdleState = idle;
            advFirstLevel.NextState = ascRight1;

            ascRight1.IdleState = idle;
            ascRight1.NextState = advSecondlevel;

            advSecondlevel.IdleState = idle;
            advSecondlevel.FallState = fallTo1;
            advSecondlevel.NextState = ascLeft;

            fallTo1.IdleState = idle;
            fallTo1.NextState = advFirstLevel;

            ascLeft.IdleState = idle;
            ascLeft.NextState = advThirdLevel;

            advThirdLevel.IdleState = idle;
            advThirdLevel.FallState = fallTo2;
            advThirdLevel.NextState = ascRight2;            

            fallTo2.IdleState = idle;
            fallTo2.NextState = advSecondlevel;

            ascRight2.IdleState = idle;
            ascRight2.NextState = win;

            win.IdleState = idle;
            win.NextState = advFirstLevel;

            this.AnimationState = idle;           
        }

        private void SetPlayerRectanglesAndColor()
        {
            if (_player.PlayerNumber == 1)
            {
                _charRect = new Rectangle(40, 498, 30, 45);
                _sourceRect = new Rectangle(309, 40, 19, 30);
                _playerText = "P1";
                _lineColor = Color.Green;
            }
            else
            {
                _charRect = new Rectangle(64, 498, 30, 45);
                _sourceRect = new Rectangle(353, 169, 19, 30);
                _playerText = "P2";
                _lineColor = Color.WhiteSmoke;
            }

            FlipHorizontally = false;
        }
    }
}
