using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SWE6753_Project.AnimationStates
{
    public class AscendingLeft :BaseAnimationState
    {
        private int elapsedTime = 0;
        private int liftBase = 400;
        private int liftTop = 270;
        private int charDropOff = 140;

        public AscendingLeft(AnimationManager animationManager, RoundManager roundManager, AudioManager audioManager)
            : base(animationManager, roundManager, audioManager) { }

        public override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            _animationManager.FlipHorizontally = false;

            if (elapsedTime > _frameUpdateRate)
            {
                elapsedTime -= _frameUpdateRate;

                if (AnimationManager._liftRect2.Y > liftTop && _animationManager._charRect.X < charDropOff)
                {
                    base.SetSourceRectangleToIdleFrame();
                    AnimationManager._liftRect2.Y -= 5;
                    _animationManager._charRect.Y -= 5;
                    _audioManager.StartAscending();
                    _audioManager.StopWalking();
                }
                else if (AnimationManager._liftRect2.Y <= liftTop && _animationManager._charRect.X < charDropOff)
                {
                    base.AdvanceSourceRectToNextFrame();
                    _animationManager._charRect.X += 5;
                    _audioManager.StopAscending();
                    _audioManager.StartWalking();
                }
                else if (AnimationManager._liftRect2.Y < liftBase)
                {
                    base.SetSourceRectangleToIdleFrame();
                    AnimationManager._liftRect2.Y += 5;
                    _audioManager.StartAscending();
                    _audioManager.StopWalking();
                }
                else
                {
                    elapsedTime = 0;
                    _audioManager.StopAscending();
                    _audioManager.StopWalking();

                    AnimationManager._liftRect2.Y = liftBase;
                    base.SetSourceRectangleToIdleFrame();
                    _animationManager.AnimationState = this.NextState;
                }
            }
        }
    }
}
