using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SWE6753_Project.AnimationStates
{
    public class AdvanceSecondLevel : BaseAnimationState
    {
        private int elapsedTime = 0;

        public AdvanceSecondLevel(AnimationManager animationManager, RoundManager roundManager, AudioManager audioManager)
            : base(animationManager, roundManager, audioManager) { }

        public override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            _animationManager.FlipHorizontally = true;

            if (!_animationManager._lineRect2.IsEmpty && elapsedTime > _frameUpdateRate)
            {
                if (_animationManager._charRect.X > _animationManager._lineRect2.Left && _animationManager._lineRect2.Width > 5)
                {
                    base.AdvanceSourceRectToNextFrame();
                    _animationManager._charRect.X -= 5;                    
                    elapsedTime -= _frameUpdateRate;

                    if (_animationManager._charRect.X < 330 && _animationManager._charRect.X > 305 && 
                        _animationManager.Player.HasBridge == false)
                    {
                        _animationManager.AnimationState = FallState;
                        _audioManager.StopWalking();
                    }

                    _audioManager.StartWalking();
                }
                else
                {
                    elapsedTime = 0;
                    _audioManager.StopWalking();
                    base.SetSourceRectangleToIdleFrame();

                    if (!_animationManager._lineRect3.IsEmpty)
                    {
                        _animationManager.AnimationState = this.NextState;
                    }
                    else
                    {
                        _animationManager.AnimationIsCompleted = true;
                        _animationManager.AnimationState = this.IdleState;
                        this.IdleState.NextState = this;
                    }
                }
            }
        }
    }
}
