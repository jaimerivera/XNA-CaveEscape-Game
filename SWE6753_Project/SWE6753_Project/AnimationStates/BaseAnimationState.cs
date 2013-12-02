using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SWE6753_Project.AnimationStates
{
    public abstract class  BaseAnimationState
    {        
        protected AnimationManager _animationManager;
        protected RoundManager _roundManager;
        protected int _frameUpdateRate = 50;
        protected AudioManager _audioManager;

        public BaseAnimationState(AnimationManager animationManager, RoundManager roundManager, AudioManager audioManager)
        {           
            _animationManager = animationManager;
            _roundManager = roundManager;
            _audioManager = audioManager;
        }
                
        public BaseAnimationState NextState { get; set; }
        public BaseAnimationState IdleState { get; set; }
        public BaseAnimationState FallState { get; set; }
        
        public abstract void Update(GameTime gameTime);

        protected void AdvanceSourceRectToNextFrame()
        {
            if (_animationManager.Player.PlayerNumber == 1)
            {
                if (_animationManager._sourceRect.X < 350) _animationManager._sourceRect.X += 20;
                else _animationManager._sourceRect.X = 310;
            }
            else
            {
                if (_animationManager._sourceRect.X < 394) _animationManager._sourceRect.X += 20;
                else _animationManager._sourceRect.X = 354;
            }
        }

        protected void SetSourceRectangleToIdleFrame()
        {
            if (_animationManager.Player.PlayerNumber == 1)
            {
                _animationManager._sourceRect.X = 310;
            }
            else
            {
                _animationManager._sourceRect.X = 354;
            }
        }
    }
}
