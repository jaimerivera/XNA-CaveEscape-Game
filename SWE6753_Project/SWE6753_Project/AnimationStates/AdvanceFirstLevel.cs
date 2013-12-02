using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SWE6753_Project.AnimationStates
{
    class AdvanceFirstLevel : BaseAnimationState
    {
        private int elapsedTime = 0;

        public AdvanceFirstLevel(AnimationManager animationManager, RoundManager roundManager, AudioManager audioManager)
            : base(animationManager, roundManager, audioManager) { }

        public override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            _animationManager.FlipHorizontally = false;

            if (!_animationManager._lineRect1.IsEmpty && elapsedTime > _frameUpdateRate)
            {
                if (_animationManager._charRect.X < (_animationManager._lineRect1.Right - 10) && _animationManager._lineRect1.Width > 5)
                {
                    base.AdvanceSourceRectToNextFrame();
                    _animationManager._charRect.X += 5;

                    elapsedTime -= _frameUpdateRate;
                    _audioManager.StartWalking();
                }
                else
                {
                    elapsedTime = 0;
                    _audioManager.StopWalking();
                    base.SetSourceRectangleToIdleFrame();

                    if (!_animationManager._lineRect2.IsEmpty)
                    {
                        _animationManager.AnimationState = this.NextState;

                    }
                    else
                    {
                        _animationManager.AnimationIsCompleted = true;
                        _animationManager.AnimationState = this.IdleState;                        
                    }
                }
            }
        }
    }
}
