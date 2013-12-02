using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SWE6753_Project.AnimationStates
{
    class AdvanceThirdLevel : BaseAnimationState
    {
        private int elapsedTime = 0;

        public AdvanceThirdLevel(AnimationManager animationManager, RoundManager roundManager, AudioManager audioManager)
            : base(animationManager, roundManager, audioManager) { }

        public override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;            

            if (!_animationManager._lineRect3.IsEmpty && elapsedTime > _frameUpdateRate)
            {
                if (_animationManager._charRect.X < (_animationManager._lineRect3.Right - 10) && _animationManager._lineRect3.Width > 5)
                {
                    base.AdvanceSourceRectToNextFrame();
                    _animationManager._charRect.X += 5;
                    elapsedTime -= _frameUpdateRate;

                    if (_animationManager._charRect.X > 865 && _animationManager._charRect.X < 875 &&
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

                    if ((_animationManager.Player.RoundScore + _animationManager.Player.TotalScore) >= 59)
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
