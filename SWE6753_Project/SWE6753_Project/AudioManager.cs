using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace SWE6753_Project
{
    public class AudioManager
    {
        private SoundEffect _gunShot, _explosion2, _barrelExploding, _bridgeExplosion;
        private SoundEffectInstance _walking, _ascending, _falling;

        private SoundEffectInstance _current;
        private List<SoundEffectInstance> _backgroundSounds;

        public AudioManager(GameContent gameContent)
        {
            _gunShot = gameContent.GunShot;
            _explosion2 = gameContent.Explosion2;
            _barrelExploding = gameContent.BarrelExplosion;
            _bridgeExplosion = gameContent.BridgeExplosion;

            _walking = gameContent.Advancing.CreateInstance();
            _walking.IsLooped = true;
            _walking.Volume = 0.3f;

            _ascending = gameContent.Ascending.CreateInstance();
            _ascending.IsLooped = true;
            _ascending.Volume = 0.4f;

            _falling = gameContent.Falling.CreateInstance();
            _falling.IsLooped = false;

            _backgroundSounds = new List<SoundEffectInstance>();
            var cave = gameContent.BackgroundCave.CreateInstance();
            cave.IsLooped = true;
            cave.Volume = 0.4f;
            _backgroundSounds.Add(cave);

            var mazy = gameContent.BackgroundMazyJungle.CreateInstance();
            mazy.IsLooped = true;
            mazy.Volume = 0.4f;
            _backgroundSounds.Add(mazy);

            var rescue = gameContent.BackgroundRescue.CreateInstance();
            rescue.IsLooped = true;
            rescue.Volume = 0.4f;
            _backgroundSounds.Add(rescue);

            var stable = gameContent.BackgroundStableBoy.CreateInstance();
            stable.IsLooped = true;
            stable.Volume = 0.4f;
            _backgroundSounds.Add(stable);

            var tunnels = gameContent.BackgroundTunnels.CreateInstance();
            tunnels.IsLooped = true;
            tunnels.Volume = 0.4f;
            _backgroundSounds.Add(tunnels);
        }

        public void GunShot()
        {
            _gunShot.Play(0.3f, 0f, 0f);
        }

        public void LoudExplosion()
        {
            _explosion2.Play();
        }

        public void BarrelLowExplosion()
        {
            _barrelExploding.Play(0.4f, 0f, 0f);
        }

        public void BarrelMediumExplosion()
        {
            _barrelExploding.Play(0.6f, 0, 0);
        }

        public void BarrelLoudExplosion()
        {
            _barrelExploding.Play();
        }

        public void BridgeExplosion()
        {
            _bridgeExplosion.Play(0.6f, 0, 0);
        }

        public void StartWalking()
        {
            if (_walking.State == SoundState.Playing) return;
            else _walking.Play();

        }

        public void StopWalking()
        {
            _walking.Stop(true);
        }

        public void StartAscending()
        {
            if (_ascending.State == SoundState.Playing) return;
            else _ascending.Play();
        }

        public void StopAscending()
        {
            _ascending.Stop();
        }

        public void Fall()
        {
            if (_falling.State == SoundState.Playing) return;
            else _falling.Play();
        }

        public void StartBackground()
        {
            if (_current != null && _current.State == SoundState.Playing) _current.Stop();
            var lastCurrent = _current;

            Random rnd = new Random();            
            do
            {
                int num = rnd.Next(_backgroundSounds.Count);
                _current = _backgroundSounds.ElementAt(num);
            } while (_current == lastCurrent);

            _current.Play();
        }

        public void StopBackGround()
        {
            if (_current != null && _current.State == SoundState.Playing) _current.Stop(true);
        }
    }
}
