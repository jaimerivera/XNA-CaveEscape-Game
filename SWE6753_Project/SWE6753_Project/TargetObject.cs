using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SWE6753_Project
{
    public class TargetObject : FlyingObjectBase
    {
        public TargetObject(Texture2D texture, Vector2 velocity, Vector2 position, int width, int height,
                            int viewWidth, int viewHeight, SpriteBatch sb, TargetTypeEnum targetType, int value, SpriteFont font)
            : base(texture, velocity, position, viewWidth, viewHeight, sb, font)
        {
            _boundingBox = new Rectangle((int)position.X, (int)position.Y, width, height);
            TargetType = targetType;
            Value = value;
        }

        public TargetTypeEnum TargetType { get; set; }

        public int Value { get; set; }

        public override void Update(GameTime gameTime)
        {
            _position += _velocity;

            _boundingBox.X = (int)_position.X;
            _boundingBox.Y = (int)_position.Y;

            _textPosition.X = _boundingBox.Center.X - 8;
            _textPosition.Y = _boundingBox.Center.Y - 10;

            base.Update(gameTime);
        }

        public void Draw()
        {
            _spriteBatch.Draw(_texture, _boundingBox, Color.Wheat);

            if (TargetType != TargetTypeEnum.Bridge)
                _spriteBatch.DrawString(_font, Value.ToString(), _textPosition, Color.Yellow);
        }
    }
}
