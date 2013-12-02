using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SWE6753_Project
{
    public class Explosion
    {        
        private SpriteBatch _spriteBatch;
        private List<ExplosionParticle> _particleList;
        private Texture2D _particle;
        private Random _rand;


        public Explosion(Texture2D particle, int intensity, int viewWidth, int viewHeight, SpriteBatch sb, Vector2 position)
        {            
            _spriteBatch = sb;
            _particle = particle;
            _rand = new Random((int)DateTime.Now.Ticks);
            _particleList = CreateParticleList(particle, intensity, viewWidth, viewHeight, position);            
        }        

        public bool IsAlive
        {
            get
            {
                foreach(var p in _particleList)
                {
                    if (p.IsAlive) return true;
                }

                return false;
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var p in _particleList)
            {
                p.Update(gameTime);
            }
        }

        public void Draw()
        {            
            foreach (var p in _particleList)
            {
                p.Draw();
            }
        }

        private List<ExplosionParticle> CreateParticleList(Texture2D particle, int intensity, int viewWidth, int viewHeight, Vector2 position)
        {
            List<ExplosionParticle> retList = new List<ExplosionParticle>();

            for (var i = 0; i < intensity * 10 + 10; i++)
            {
                ExplosionParticle ep = new ExplosionParticle(
                                                  particle,
                                                  GetVelocity(intensity),
                                                  position,
                                                  viewWidth,
                                                  viewHeight,
                                                  _spriteBatch,
                                                  null);
                retList.Add(ep);
            }

            return retList;
        }

        private Vector2 GetVelocity(int intensity)
        {            
            double rot = _rand.NextDouble() * Math.PI * 2.0;
            Vector2 vector = new Vector2((float)(Math.Cos(rot)), (float)(Math.Sin(rot)));
            vector.X *= (float)(intensity + _rand.NextDouble() * 2);
            vector.Y *= (float)(intensity + _rand.NextDouble() * 2);           
            return vector;
        }
    }
}
