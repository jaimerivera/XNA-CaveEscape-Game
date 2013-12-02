using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SWE6753_Project
{
    public class RoundManager
    {
        private Vector2 _textPosition;
        private Player _player1;
        private Player _player2;
        private float _roundLeftTime;
        private const float ROUNDLENGTH = 50000;
        private SpriteFont _font;
        private int _roundNumber;
        private SpriteBatch _spriteBatch;
        private FlyingObjectManager _flyingObjMgr;

        public RoundManager(Vector2 textPosition, Player player1, Player player2,
                            GameContent content, SpriteBatch sb, FlyingObjectManager flyingObjMgr)
        {
            _textPosition = textPosition;
            _player1 = player1;
            _player2 = player2;
            _font = content.GameFont;
            _spriteBatch = sb;
            _flyingObjMgr = flyingObjMgr;
        }
        public int SecondsTimeLeftInRound { get { return (int)_roundLeftTime/1000; } }

        public bool RoundIsRunning
        {
            get
            {
                return _roundLeftTime > 0;
            }
        }

        internal void StopRound()
        {
            _roundLeftTime = 0;
            _flyingObjMgr.Stop();
            _player1.Stop();
            _player2.Stop();
        }

        public void StartRound()
        {
            _flyingObjMgr.Start();
            _player1.Start();
            _player2.Start();

            _roundNumber++;
            _roundLeftTime = ROUNDLENGTH;
        }

        public void Reset()
        {
            _flyingObjMgr.Reset();
            _player1.Reset();
            _player2.Reset();

            _roundNumber = 0;
            _roundLeftTime = 0;
        }

        public void Update(GameTime gameTime)
        {
            if (_roundLeftTime >= 0)
            {
                _roundLeftTime -= gameTime.ElapsedGameTime.Milliseconds;

                if (_roundLeftTime <= 0)
                {
                    _flyingObjMgr.Stop();
                    _player1.Stop();
                    _player2.Stop();
                }
            }
        }

        public void Draw()
        {
            string leftTime = Math.Round((_roundLeftTime / 1000), 0).ToString();
            string msg = string.Format("Round:{0} Time:{1}", _roundNumber.ToString(), leftTime);

            _spriteBatch.DrawString(_font, msg, _textPosition, Color.White);
        }
    }
}
