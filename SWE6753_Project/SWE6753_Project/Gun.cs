using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SWE6753_Project
{
    public class Gun
    {
        private int _posX;
        private int _posY;
        private Texture2D _tex;
        private Rectangle _rect;
        private Vector2 _origin;
        float _rotation;
        bool _isEnabled;
        private SpriteBatch _spriteBatch;
        private FlyingObjectManager _flyingObjMgr;

        Keys _left, _right, _fire;        
        Player _player;
                
        Vector2 _bulletVelocity;        
        private Texture2D _bulletTex;
        Vector2 _barrelTipPosition;

        private int _reloadTime = 800;
        private int _gunLockTime = 300;
        private int _timeSinceLastShot = 0;
        private int _viewWidth;
        private int _viewHeight;

        SpriteFont _font; //for testing                        
        private AudioManager _audioManager;

        public Gun(int caseXPosition, int caseYPosition, GameContent contentManager, 
                    Keys left, Keys right, Keys fire, Player player, 
                    int viewPortWidth, int viewPortHeight, SpriteBatch sb, FlyingObjectManager flyingObjMgr, AudioManager audioManager)
        {
            _posX = caseXPosition + 30;//to offset from case top left position
            _posY = caseYPosition + 15; 
            
            _tex = contentManager.GunTexture;
            _bulletTex = contentManager.BulletTexture;
            _font = contentManager.GameFont;

            _left = left;
            _right = right;
            _fire = fire;

            _player = player;

            _viewWidth = viewPortWidth;
            _viewHeight = viewPortHeight;

            _spriteBatch = sb;
            _flyingObjMgr = flyingObjMgr;
            _audioManager = audioManager;

            _rect = new Rectangle(_posX, _posY, _tex.Width, _tex.Height);
            _origin = new Vector2(_tex.Width / 2, _tex.Height);
            _bulletVelocity = new Vector2();
        }

        public bool Enable { get { return _isEnabled; } set { _isEnabled = value; } }

        public void Update(GameTime gameTime)
        {            

            if (_timeSinceLastShot <= _reloadTime) //make sure we don't overflow
            {
                _timeSinceLastShot += gameTime.ElapsedGameTime.Milliseconds;                
            }
            
            
            if(_isEnabled && _timeSinceLastShot > _gunLockTime)
            {
                var leftKey = Keyboard.GetState().IsKeyDown(_left);
                var rightKey = Keyboard.GetState().IsKeyDown(_right);                

                if (leftKey && _rotation > -1.3)
                {
                    _rotation -= 0.02f;
                }
                else if (rightKey && _rotation < 1.3)
                {
                    _rotation += 0.02f;
                }
            }

            if (_isEnabled && _timeSinceLastShot > _reloadTime)
            {
                var fireKey = Keyboard.GetState().IsKeyDown(_fire);

                if (fireKey)
                {
                    _timeSinceLastShot = 0;

                    _bulletVelocity.X = (float)Math.Cos(_rotation + MathHelper.PiOver2);
                    _bulletVelocity.Y = (float)Math.Sin(_rotation + MathHelper.PiOver2);
                    _barrelTipPosition.X = _posX - 2 - _bulletVelocity.X * _tex.Height;
                    _barrelTipPosition.Y = _posY - _bulletVelocity.Y * _tex.Height;

                    var bullet = new Bullet(_bulletTex, _bulletVelocity, _barrelTipPosition, _viewWidth, _viewHeight, _spriteBatch, _player, _font);
                    _flyingObjMgr.AddBullet(bullet);

                    _audioManager.GunShot();
                }
            }
        }

        public void Draw()
        {            
            _spriteBatch.Draw(_tex, _rect, null, Color.White, _rotation, _origin, SpriteEffects.None, 0);
            //spriteBatch.DrawString(_font, "T:" + _rotation, _barrelTipPosition, Color.White);            
        }


    }
}
