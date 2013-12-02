using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SWE6753_Project.AnimationStates
{
    public class FallToFirstLevel : BaseAnimationState
    {
        private int elapsedTime = 0;
        private int firstLevelHeight = 498;

        public FallToFirstLevel(AnimationManager animationManager, RoundManager roundManager, AudioManager audioManager)
            : base(animationManager, roundManager, audioManager) { }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            _animationManager.FlipHorizontally = true;

            if (elapsedTime > _frameUpdateRate)
            {
                elapsedTime -= _frameUpdateRate;

                if (_animationManager._charRect.Y < firstLevelHeight)
                {
                    _animationManager._charRect.X = 310;
                    _animationManager._charRect.Y += 5;
                    _audioManager.Fall();
                }
                else
                {
                    base.SetSourceRectangleToIdleFrame();
                    _animationManager.FlipHorizontally = false;
                    _animationManager._charRect.Y = firstLevelHeight;
                    IdleState.NextState = this.NextState;
                    _animationManager.AnimationState = this.IdleState;                    
                    _animationManager.Player.SetScoreAfterDrop(5);
                    _animationManager.AnimationIsCompleted = true;
                }
            }
        }
    }
}
