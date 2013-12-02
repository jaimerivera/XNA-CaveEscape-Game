using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SWE6753_Project.AnimationStates
{
    public class IdleAnimation : BaseAnimationState
    {
        public IdleAnimation(AnimationManager animationManager, RoundManager roundManager, AudioManager audioManager)
            : base(animationManager, roundManager, audioManager) { }       

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_roundManager.RoundIsRunning) return;
            else
            {
                _animationManager.AnimationState = this.NextState;
                _animationManager.AnimationIsCompleted = false;
            }
        }        
    }
}
