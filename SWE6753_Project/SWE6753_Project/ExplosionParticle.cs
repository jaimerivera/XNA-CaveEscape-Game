using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SWE6753_Project
{
    public class ExplosionParticle : FlyingObjectBase
    {
        //float _rotation;
        //Vector2 _origin;
        int age = 255;
        Color _color;
        //private Vector2 _scale;

        public ExplosionParticle(Texture2D texture, Vector2 velocity, Vector2 position,
                                 int viewWidth, int viewHeight, SpriteBatch sb, SpriteFont font)
            :base(texture, velocity, position, viewWidth, viewHeight, sb, font)
        {
            _boundingBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            //_origin = new Vector2(texture.Width / 2, texture.Height / 2);
            //_scale = new Vector2(0.5f, 0.5f);
            _color = new Color(age, age, age/2, age);
            //_rotation = (float)Math.Atan2(_velocity.Y, _velocity.X);
        }

        public override void Update(GameTime gameTime)
        {            
            age -= 2;
            age = age >= 0 ? age : 0;

            _color.A = (byte)age;
            _color.B = (byte)(age / 2);
            _color.G = (byte)age;
            
            if (age == 0)
            {
                IsAlive = false;
            }
            
            //if (_position.Y + _velocity.Y >= _viewHeight)
            //{
            //    _velocity.Y = -_velocity.Y / 2;
            //}
            
            _velocity.Y += 0.05f;            
            //_rotation = (float)Math.Atan2(_velocity.Y, _velocity.X);
            
            _position += _velocity;
            
            //_scale.X = _velocity.Length()/ 5;

            base.Update(gameTime);

        }

        public void Draw()
        {
            _spriteBatch.Draw(_texture, _position, _color);
            //_spriteBatch.Draw(_texture, _position, null, _color, (float)_rotation, _origin, _scale, SpriteEffects.None, 1.0f);
        }
    }
}
