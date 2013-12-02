using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SWE6753_Project.AnimationStates
{
    public class FallToSecondLevel : BaseAnimationState
    {
        private int elapsedTime = 0;
        private int secondLevelHeight = 368;

        public FallToSecondLevel(AnimationManager animationManager, RoundManager roundManager, AudioManager audioManager)
            : base(animationManager, roundManager, audioManager) { }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;            

            if (elapsedTime > _frameUpdateRate)
            {
                elapsedTime -= _frameUpdateRate;

                if (_animationManager._charRect.Y < secondLevelHeight)
                {
                    _animationManager._charRect.X = 870;
                    _animationManager._charRect.Y += 5;
                    _audioManager.Fall();
                }
                else
                {
                    base.SetSourceRectangleToIdleFrame();
                    _animationManager.FlipHorizontally = true;
                    _animationManager._charRect.Y = secondLevelHeight;
                    IdleState.NextState = this.NextState;
                    _animationManager.AnimationState = this.IdleState;
                    _animationManager.Player.SetScoreAfterDrop(25);
                    _animationManager.AnimationIsCompleted = true;
                }
            }
        }
    }
}
