using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SWE6753_Project
{
    public class Bullet : FlyingObjectBase
    {                        
        private float _rotation;                
        private Vector2 _origin;        

        public Bullet(Texture2D texture, Vector2 velocity, Vector2 position, int viewPortWidth, int viewPortHeight, SpriteBatch sb, Player player, SpriteFont font) 
            :base(texture, velocity, position,viewPortWidth, viewPortHeight, sb, font)
        {                                                
            UpdateRotation();
            Player = player;
            _boundingBox = new Rectangle((int)_position.X, (int)_position.Y, 16, 32);            
            _origin = new Vector2(8,16);
        }

        public Player Player { get; private set; }

        public override void Update(GameTime gameTime)
        {
            if (IsAlive)
            {
                //TODO: 
                // 1. Check for collision                
                _velocity.Y -= 0.0025f;
                _position.X -= (5 * _velocity.X);
                _position.Y -= (5 * _velocity.Y);
                _boundingBox.X = (int)_position.X;
                _boundingBox.Y = (int)_position.Y;

                UpdateRotation();

                base.Update(gameTime);                
            }
        }

        public void Draw()
        {
            if (IsAlive)
            {
                _spriteBatch.Draw(_texture, _boundingBox, null, Color.White, _rotation, _origin, SpriteEffects.None, 0);
            }
        }       

        private void UpdateRotation()
        {
            _rotation = -1 * (float)Math.Atan2(_velocity.X, _velocity.Y);
        }        
    }
}
