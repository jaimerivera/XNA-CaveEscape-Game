using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SWE6753_Project
{
    public enum GameState
    {
        GameInProgress = 1,
        NewGameMenu = 2,
        Paused = 3,
        GameWonAnimation = 4
    }
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        GameContent _gameContent;
        Player _player1, _player2;
        RoundManager _roundManager;
        FlyingObjectManager _flyingObjManager;
        AnimationManager _player1Animation;
        AnimationManager _player2Animation;
        AudioManager _audioManager;

        Rectangle _backgroundRect, _gunCaseLeftRect, _gunCaseRightRect;
        Rectangle _centeredPromptRect, _winMessage, _roundMessage;

        const float HEIGHT = 720.0f;
        const float WIDTH = 1280.0f;
        private bool _isPaused;
        GameState _gameState = GameState.NewGameMenu;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _graphics.PreferredBackBufferWidth = (int)WIDTH;
            _graphics.PreferredBackBufferHeight = (int)HEIGHT;
            //_graphics.IsFullScreen = true;
        }

        protected override void Initialize()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _isPaused = false;
            _gameContent = new GameContent(Content, GraphicsDevice);
            _audioManager = new AudioManager(_gameContent);

            int ht = _graphics.GraphicsDevice.Viewport.Height;
            int wt = _graphics.GraphicsDevice.Viewport.Width;

            float heightRatio = HEIGHT / (float)ht;
            float widthRatio = WIDTH / (float)wt;

            _backgroundRect = new Rectangle(0, 0, wt, ht);
            TargetBuildHelper targetHelper = new TargetBuildHelper(_gameContent, wt, ht, _spriteBatch);
            _flyingObjManager = new FlyingObjectManager(targetHelper, _spriteBatch, _gameContent.Particle, wt, ht, _audioManager);
            

            int gunCaseX = (int)(350 * widthRatio);
            int gunCaseY = (int)(600 * heightRatio); //black line is at 630            
            _gunCaseLeftRect = new Rectangle(gunCaseX, gunCaseY, 60, 30);
            _gunCaseRightRect = new Rectangle(wt - gunCaseX - 60, gunCaseY, 60, 30);

            _player1 = new Player(new Vector2(10, ht - 50), 1, _gameContent, new Point(gunCaseX, gunCaseY), wt, ht, _spriteBatch, _flyingObjManager, _audioManager);
            _player2 = new Player(new Vector2(wt - 190, ht - 50), 2, _gameContent, new Point(wt - gunCaseX - 60, gunCaseY), wt, ht, _spriteBatch, _flyingObjManager, _audioManager);

            _roundManager = new RoundManager(new Vector2(wt / 2 - 100, ht - 50), _player1, _player2, _gameContent, _spriteBatch, _flyingObjManager);

            _player1Animation = new AnimationManager(_player1, _gameContent, _spriteBatch, _roundManager, _audioManager);
            _player2Animation = new AnimationManager(_player2, _gameContent, _spriteBatch, _roundManager, _audioManager);

            _centeredPromptRect = new Rectangle(340, 200, 600, 300);
            _winMessage = new Rectangle(520, 570, _gameContent.Player1Won.Width, _gameContent.Player1Won.Height);
            _roundMessage = new Rectangle(520, 570, _gameContent.RoundStarting.Width, _gameContent.RoundStarting.Height);

            _audioManager.StartBackground();
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) this.Exit();

            switch (_gameState)
            {
                case GameState.GameInProgress:
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.P))
                        {
                            UpdateGamePaused(gameTime);
                        }
                        else
                        {
                            UpdateGameInProgress(gameTime); 
                        }
                        break;
                    }
                case GameState.GameWonAnimation: UpdateGameWonAnimation(gameTime); break;
                case GameState.NewGameMenu: UpdateNewGameMenu(gameTime); break;
                case GameState.Paused: UpdateGamePaused(gameTime); break;
            }

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            _spriteBatch.Draw(_gameContent.BackgroundTexture, _backgroundRect, Color.White);

            _roundManager.Draw();

            _player1Animation.Draw();
            _player2Animation.Draw();

            _flyingObjManager.Draw();

            _player1.Draw();
            _player2.Draw();

            _spriteBatch.Draw(_gameContent.GunCaseTexture, _gunCaseLeftRect, Color.White);
            _spriteBatch.Draw(_gameContent.GunCaseTexture, _gunCaseRightRect, Color.White);            

            if (_gameState == GameState.Paused)
            {
                _spriteBatch.Draw(_gameContent.GamePaused, _centeredPromptRect, Color.White);
                //_spriteBatch.DrawString(_gameContent.GameFont, "GAME PAUSED", new Vector2(WIDTH / 2, HEIGHT / 2), Color.Red);
            }
            else if (_gameState == GameState.NewGameMenu)
            {
                _spriteBatch.Draw(_gameContent.NewGameMenu, _centeredPromptRect, Color.White);
            }
            else if (_gameState == GameState.GameWonAnimation)
            {
                Texture2D tex;
                if (_player1.HasWon) tex = _gameContent.Player1Won;
                else tex = _gameContent.Player2Won;

                _spriteBatch.Draw(tex, _winMessage, Color.White);
            }
            else if (_gameState == GameState.GameInProgress && _roundManager.SecondsTimeLeftInRound > 48)
            {
                _spriteBatch.Draw(_gameContent.RoundStarting, _roundMessage, Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }        

        private void UpdateGameInProgress(GameTime gameTime)
        {            
            //if (Keyboard.GetState().IsKeyDown(Keys.N) && _roundManager.RoundIsRunning == false)
            //{
            //    _roundManager.StartRound();
            //}

            if (_roundManager.RoundIsRunning)
            {
                _roundManager.Update(gameTime);

                _flyingObjManager.Update(gameTime);

                _player1.Update(gameTime);
                _player2.Update(gameTime);                

                if (_player1.HasWon || _player2.HasWon)
                {
                    _gameState = GameState.GameWonAnimation;
                    _roundManager.StopRound();
                    _audioManager.StopBackGround();
                }

                _player1Animation.Update(gameTime);
                _player2Animation.Update(gameTime);
            }
            else
            {
                if (! _player1Animation.AnimationIsCompleted)
                {
                    _audioManager.StopBackGround();
                    _player1Animation.Animating = true;
                    _player2Animation.Animating = false;
                    _player1Animation.Update(gameTime);
                }
                else if (!_player2Animation.AnimationIsCompleted)
                {
                    _audioManager.StopBackGround();
                    _player1Animation.Animating = false;
                    _player2Animation.Animating = true;
                    _player2Animation.Update(gameTime);
                }
                else
                {
                    _player1Animation.Animating = false;
                    _player2Animation.Animating = false;
                    _roundManager.StartRound();
                    _audioManager.StopBackGround();
                    _audioManager.StartBackground();
                }
            }
        }

        private void UpdateNewGameMenu(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.N))
            {
                _roundManager.StartRound();
                _gameState = GameState.GameInProgress;
                _audioManager.StopBackGround();
                _audioManager.StartBackground();
            }                        
        }

        private void UpdateGameWonAnimation(GameTime gameTime)
        {
            if (!_player1Animation.AnimationIsCompleted)
            {
                _player1Animation.Animating = true;
                _player2Animation.Animating = false;
                _player1Animation.Update(gameTime);
            }
            else if (!_player2Animation.AnimationIsCompleted)
            {
                _player1Animation.Animating = false;
                _player2Animation.Animating = true;
                _player2Animation.Update(gameTime);
            }
            else
            {
                _player1Animation.Animating = false;
                _player2Animation.Animating = false;
                _roundManager.Reset();
                _player1Animation.Reset();
                _player2Animation.Reset();

                _audioManager.StartBackground();
                _gameState = GameState.NewGameMenu;                
            }
        }

        private void UpdateGamePaused(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.P)) _isPaused = true;
            if (Keyboard.GetState().IsKeyDown(Keys.R)) _isPaused = false;

            if (!_isPaused) _gameState = GameState.GameInProgress;
            else _gameState = GameState.Paused;
        }
    }
}
