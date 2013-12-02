using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SWE6753_Project
{
    public class FlyingObjectManager
    {
        List<Bullet> _bulletList;
        List<Bullet> _bulletsToRemove;
        List<TargetObject> _targetList;
        List<TargetObject> _targetsToRemove;
        List<Explosion> _explosionList;
        List<Explosion> _explosionToRemove;

        private TargetBuildHelper _targetBuildHelper;
        private bool _gameInProgress;
        private SpriteBatch _spriteBatch;
        private Texture2D _particle;
        private int _viewWidth;
        private int _viewHeight;
        private AudioManager _audioManager;

        public FlyingObjectManager(TargetBuildHelper targetBuildHelper, SpriteBatch sb, 
                                    Texture2D particle, int viewWidth, int viewHeight, AudioManager audioManager)
        {
            _bulletList = new List<Bullet>();
            _targetList = new List<TargetObject>();
            _bulletsToRemove = new List<Bullet>();
            _targetsToRemove = new List<TargetObject>();
            _explosionList = new List<Explosion>();
            _explosionToRemove = new List<Explosion>();

            _targetBuildHelper = targetBuildHelper;
            _spriteBatch = sb;
            _particle = particle;
            _viewWidth = viewWidth;
            _viewHeight = viewHeight;
            _audioManager = audioManager;
        }

        public void AddBullet(Bullet bullet)
        {
            _bulletList.Add(bullet);
        }

        public void Stop()
        {
            _gameInProgress = false;
            _targetBuildHelper.Stop();

            foreach (var bullet in _bulletList)
            {
                bullet.Dissolve();
            }

            _bulletList.Clear();
            _targetList.Clear();
            _bulletsToRemove.Clear();
            _targetsToRemove.Clear();
            _explosionList.Clear();
            _bulletsToRemove.Clear();
        }

        public void Reset()
        {
            this.Stop(); //nothing additional to do
        }

        public void Start()
        {
            _gameInProgress = true;
        }

        public void Update(GameTime gameTime)
        {
            if (_gameInProgress)
            {
                AddNewTargetObject(gameTime);
                UpdateBullets(gameTime);
                UpdateTargets(gameTime);
                CheckCollisions();
                UpdateExplosions(gameTime);
            }
        }       

        public void Draw()
        {
            foreach (var bullet in _bulletList)
            {
                bullet.Draw();
            }

            foreach (var target in _targetList)
            {
                target.Draw();
            }

            
            if (_explosionList.Count > 0)
            {
                _spriteBatch.End();
                _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.Additive);

                foreach (var explosion in _explosionList)
                {
                    explosion.Draw();
                }

                _spriteBatch.End();
                _spriteBatch.Begin();
            }
            
        }     

        private void AddNewTargetObject(GameTime gameTime)
        {
            var newTarget = _targetBuildHelper.GetNextTargetOrNull(gameTime);

            if (newTarget != null)
            {
                _targetList.Add(newTarget);
            }
        }

        public void CheckCollisions()
        {
            foreach (var b in _bulletList)
            {
                foreach (var t in _targetList)
                {
                    if (b.BoundingBox.Intersects(t.BoundingBox))
                    {
                        b.Player.AddTargetCollision(t.Value, t.TargetType);
                        _targetsToRemove.Add(t);
                        _bulletsToRemove.Add(b);

                        CreateExplosion(t);

                        break;
                    }
                }

                foreach (var t in _targetsToRemove)
                {
                    _targetList.Remove(t);                        
                }
                _targetsToRemove.Clear();
            }

            foreach (var b in _bulletsToRemove)
            {
                _bulletList.Remove(b);
            }

            _bulletsToRemove.Clear();
        }

        private void CreateExplosion(TargetObject target)
        {
            Vector2 position = new Vector2(target.BoundingBox.Center.X, target.BoundingBox.Center.Y);
            Explosion explosion = new Explosion(_particle, Math.Abs(target.Value), _viewWidth, _viewHeight, _spriteBatch, position);
            _explosionList.Add(explosion);

            if (target.TargetType == TargetTypeEnum.Explosive)
            {
                _audioManager.LoudExplosion();
            }
            else if (target.TargetType == TargetTypeEnum.Crate)
            {
                if (target.Value == 3) _audioManager.BarrelMediumExplosion();
                else if (target.Value == 1) _audioManager.BarrelLowExplosion();
                else _audioManager.BarrelLoudExplosion();
            }
            else
            {
                _audioManager.BridgeExplosion();
            }
        }        

        private void UpdateTargets(GameTime gameTime)
        {
            if (_targetList.Count > 0)
            {
                foreach (var t in _targetList)
                {
                    if (t.IsAlive == false)
                    {
                        _targetsToRemove.Add(t);
                    }
                }

                foreach (var t in _targetsToRemove)
                {
                    _targetList.Remove(t);
                }

                _targetsToRemove.Clear();

                foreach (var target in _targetList)
                {
                    target.Update(gameTime);
                }
            }
        }

        private void UpdateBullets(GameTime gameTime)
        {
            if (_bulletList.Count > 0)
            {

                foreach (var i in _bulletList)
                {
                    if (i.IsAlive == false)
                    {
                        _bulletsToRemove.Add(i);
                    }
                }

                foreach (var i in _bulletsToRemove)
                {
                    _bulletList.Remove(i);
                }

                _bulletsToRemove.Clear();

                foreach (var bullet in _bulletList)
                {
                    bullet.Update(gameTime);
                }

            }//end bullet update
        }

        private void UpdateExplosions(GameTime gameTime)
        {
            foreach (var ex in _explosionList)
            {
                if (ex.IsAlive) ex.Update(gameTime);
                else _explosionToRemove.Add(ex);
            }
            
            foreach (var ex in _explosionToRemove)
            {
                _explosionList.Remove(ex);
            }

            _explosionToRemove.Clear();
        }

        
    }
}
