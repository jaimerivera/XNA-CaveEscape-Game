using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SWE6753_Project
{
    public abstract class FlyingObjectBase
    {
        protected Texture2D _texture;
        protected Vector2 _velocity;                
        protected Vector2 _position;
        protected Rectangle _boundingBox;        
        protected int _viewWidth;
        protected int _viewHeight;
        protected SpriteBatch _spriteBatch;
        protected SpriteFont _font;
        protected Vector2 _textPosition;

        public FlyingObjectBase(Texture2D texture, Vector2 velocity, Vector2 position, 
                                int viewWidth, int viewHeight, SpriteBatch sb, SpriteFont font)
        {
            _texture = texture;
            _velocity = velocity;
            _position = position;
            _viewWidth = viewWidth;
            _viewHeight = viewHeight;
            _spriteBatch = sb;
            _font = font;
            _textPosition = position;
            IsAlive = true;
        }                

        public Rectangle BoundingBox
        {
            get { return _boundingBox; }
        }

        public void Dissolve()
        {
            IsAlive = false;
        }

        public bool IsAlive { get; protected set; }

        public virtual void Update(GameTime gameTime)
        {
            if (_position.X < 0 || _position.X > _viewWidth) IsAlive = false;
            else if (_position.Y < 0 || _position.Y > _viewHeight) IsAlive = false;
        }
    }
}
