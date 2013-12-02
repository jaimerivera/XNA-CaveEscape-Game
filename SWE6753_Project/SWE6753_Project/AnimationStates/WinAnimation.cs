using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SWE6753_Project.AnimationStates
{
    public class WinAnimation : BaseAnimationState
    {
        //private int elapsedTime = 0;

        public WinAnimation(AnimationManager animationManager, RoundManager roundManager, AudioManager audioManager)
            : base(animationManager, roundManager, audioManager) { }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            _animationManager.FlipHorizontally = false;
            _animationManager.AnimationIsCompleted = true;
            this.IdleState.NextState = this.NextState;
            _animationManager.AnimationState = IdleState;

            if (_animationManager.Player.PlayerNumber == 1)
            {
                _animationManager._charRect.X = 40;
                _animationManager._charRect.Y = 498;
            }
            else
            {
                _animationManager._charRect.X = 64;
                _animationManager._charRect.Y = 498;
            }
        }
    }
}
