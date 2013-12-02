using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SWE6753_Project
{
    public class Player
    {
        private int _roundScore, _totalScore, _playerNumber;
        private Vector2 _scoreTextPosition;
        private string _playerName;
        private SpriteFont _font;
        private Gun _gun;
        private SpriteBatch _spriteBatch;

        private bool _hasBridge;
        private Texture2D _bridgeTex;
        private Rectangle _bridgeRect;



        public Player(Vector2 scorePosition, int playerNumber, GameContent content,
                      Point gunCasePos, int viewWidth, int viewHeight, SpriteBatch sb, FlyingObjectManager flyingObjMgr,
                      AudioManager audioManager)
        {
            _scoreTextPosition = scorePosition;
            _playerName = "Player " + playerNumber.ToString();
            _playerNumber = playerNumber;
            _font = content.GameFont;
            _spriteBatch = sb;
            _gun = CreateGun(content, gunCasePos, viewWidth, viewHeight, flyingObjMgr, audioManager);

            _bridgeTex = content.Bridge;
            _bridgeRect = new Rectangle((int)_scoreTextPosition.X + 140, (int)_scoreTextPosition.Y, 40, 20);
            
        }

        public int RoundScore { get { return _roundScore; } }

        public int TotalScore { get { return _totalScore; } }

        public bool HasBridge { get { return _hasBridge; } }

        public bool HasWon
        {
            get
            {
                if ((_totalScore + _roundScore) >= 59)
                {
                    if (_totalScore > 53) return true;
                    else if (_totalScore < 54 && HasBridge) return true;
                    else return false;
                }
                else
                {
                    return false;
                }
            }
        }

        public int PlayerNumber { get { return _playerNumber; } }

        public void AddTargetCollision(int value, TargetTypeEnum targetType)
        {
            switch (targetType)
            {
                case TargetTypeEnum.Bridge:
                    {
                        _hasBridge = true;
                        break;
                    }
                case TargetTypeEnum.Crate:
                    {
                        _roundScore += value;
                        //_roundScore += 20; //used for cheating!
                        break;
                    }
                case TargetTypeEnum.Explosive:
                    {
                        _roundScore += value;
                        _roundScore = _roundScore >= 0 ? _roundScore : 0;
                        break;
                    }
            }
        }

        public void SetScoreAfterDrop(int score)
        {
            _totalScore = score;
            _roundScore = 0;
        }

        public void Stop()
        {
            _gun.Enable = false;
        }

        internal void Reset()
        {
            _roundScore = 0;
            _gun.Enable = false;
            _hasBridge = false;
            _totalScore = 0;
        }

        public void Start()
        {
            _totalScore += _roundScore;
            _roundScore = 0;
            _gun.Enable = true;
            _hasBridge = false;
        }

        public void Update(GameTime gameTime)
        {
            _gun.Update(gameTime);
        }

        public void Draw()
        {
            _spriteBatch.DrawString(_font, _playerName + ": " + _roundScore.ToString(), _scoreTextPosition, Color.White);

            if (HasBridge)
                _spriteBatch.Draw(_bridgeTex, _bridgeRect, Color.White);

            _gun.Draw();
        }

        private Gun CreateGun(GameContent content, Point gunCasePos, int viewWidth, int viewHeight, FlyingObjectManager flyingObjMgr, AudioManager audioManager)
        {
            if (_playerNumber == 1)
            {
                return new Gun(gunCasePos.X, gunCasePos.Y, content, Keys.A, Keys.D, Keys.S, this, viewWidth, viewHeight, _spriteBatch, flyingObjMgr, audioManager);
            }
            else
            {
                return new Gun(gunCasePos.X, gunCasePos.Y, content, Keys.NumPad4, Keys.NumPad6, Keys.NumPad5, this, viewWidth, viewHeight, _spriteBatch, flyingObjMgr, audioManager);
            }
        }
    }
}
