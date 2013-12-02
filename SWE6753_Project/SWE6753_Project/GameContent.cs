using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace SWE6753_Project
{
    public class GameContent
    {        
        public GameContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            
            BackgroundTexture = contentManager.Load<Texture2D>("GameBackground");
            GunCaseTexture = contentManager.Load<Texture2D>("GunCase");
            GunTexture = contentManager.Load<Texture2D>("gun");
            BulletTexture = contentManager.Load<Texture2D>("Bullet");
            GameFont = contentManager.Load<SpriteFont>("myFont");
            ArrowCrate = contentManager.Load<Texture2D>("crate2");
            DiagonalCrate = contentManager.Load<Texture2D>("crate");
            Dynamite = contentManager.Load<Texture2D>("Dynamite");
            Bridge = contentManager.Load<Texture2D>("bridge");
            Explosive = contentManager.Load<Texture2D>("explosive");
            Particle = contentManager.Load<Texture2D>("particle");

            LineTexture = new Texture2D(graphicsDevice, 1, 1);
            LineTexture.SetData<Color>(new[] { Color.White });

            Lift = contentManager.Load<Texture2D>("liftPlatform");
            Luigi = contentManager.Load<Texture2D>("LuigiWorld");
            NewGameMenu = contentManager.Load<Texture2D>("NewGameMenu");

            Player1Won = contentManager.Load<Texture2D>("Player1Won");
            Player2Won = contentManager.Load<Texture2D>("Player2Won");
            RoundStarting = contentManager.Load<Texture2D>("RoundStarting");

            GunShot = contentManager.Load<SoundEffect>("GunShotAudio");
            Explosion2 = contentManager.Load<SoundEffect>("Explosion2");
            BarrelExplosion = contentManager.Load<SoundEffect>("BarrelExploding");
            BridgeExplosion = contentManager.Load<SoundEffect>("BridgeExplosion");
            Advancing = contentManager.Load<SoundEffect>("CharacterAdvancing");
            Ascending = contentManager.Load<SoundEffect>("CharacterAscending");
            Falling = contentManager.Load<SoundEffect>("fallingOff");

            BackgroundCave = contentManager.Load<SoundEffect>("background_cave");
            BackgroundMazyJungle = contentManager.Load<SoundEffect>("background_mazyjungle");
            BackgroundRescue = contentManager.Load<SoundEffect>("background_rescue");
            BackgroundStableBoy = contentManager.Load<SoundEffect>("background_stableBoy");
            BackgroundTunnels = contentManager.Load<SoundEffect>("background_tunnels");

            GamePaused = contentManager.Load<Texture2D>("GamePaused");
        }

        public Texture2D BackgroundTexture { get; private set; }

        public Texture2D GunCaseTexture { get; private set; }

        public Texture2D GunTexture { get; private set; }

        public Texture2D BulletTexture { get; private set; }

        public SpriteFont GameFont { get; set; }

        public Texture2D ArrowCrate { get; set; }

        public Texture2D DiagonalCrate { get; set; }

        public Texture2D Dynamite { get; set; }

        public Texture2D Bridge { get; set; }        

        public Texture2D Explosive { get; set; }

        public Texture2D Particle { get; set; }

        public Texture2D LineTexture { get; set; }

        public Texture2D Lift { get; set; }

        public Texture2D Luigi { get; set; }

        public Texture2D NewGameMenu { get; set; }

        public Texture2D Player1Won { get; set; }

        public Texture2D Player2Won { get; set; }

        public Texture2D RoundStarting { get; set; }

        public SoundEffect GunShot { get; set; }

        public SoundEffect Explosion2 { get; set; }

        public SoundEffect BarrelExplosion { get; set; }

        public SoundEffect BridgeExplosion { get; set; }

        public SoundEffect Advancing { get; set; }

        public SoundEffect Ascending { get; set; }

        public SoundEffect Falling { get; set; }

        public SoundEffect BackgroundCave { get; set; }

        public SoundEffect BackgroundMazyJungle { get; set; }

        public SoundEffect BackgroundRescue { get; set; }

        public SoundEffect BackgroundStableBoy { get; set; }

        public SoundEffect BackgroundTunnels { get; set; }

        public Texture2D GamePaused { get; set; }
    }
}
