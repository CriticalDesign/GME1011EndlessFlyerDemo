using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

namespace GME1011EndlessFlyer
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //here are our game attributes (the above ones are MonoGame defaults)
       
        //textures we need
        private Texture2D _backgroundTexture, _obstacleTexture, _starTexture;

        //objects we need
        private WallBuilder _wallBuilder;
        private Player _player;
        private Destroyer _destroyer1, _destroyer2;

        //mostly default game constructor except that the window was resized to 1200x600 to better
        //fit the background image.
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1200;
            _graphics.PreferredBackBufferHeight = 600;
            _graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        //no changes
        protected override void Initialize()
        {
            base.Initialize();
        }

        //most of our initialization is in here
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //load and initialize our textures
            _backgroundTexture = Content.Load<Texture2D>("background");
            _obstacleTexture = Content.Load<Texture2D>("tile");
            _starTexture = Content.Load<Texture2D>("star");

            //initialize our wall builder
            _wallBuilder = new WallBuilder(_obstacleTexture, _starTexture);

            //two destroyers are better than one
            _destroyer1 = new Destroyer(0.75f, 1300f, 300f);
            _destroyer2 = new Destroyer(1.0f, 1300f, 400f);

            //one player is all we can handle for now
            _player = new Player(100, 100, Content.Load<Texture2D>("ship"), 1.75f);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //all the magic

            if (!_player.hasCrashed())  //if player hasn't crashed, do the game
                                        //when they crash, stop the updates!!
            {
                _wallBuilder.Update(gameTime); //wall builder update method (create the wall)
                _wallBuilder.MoveObstaclesAndStars(); //ask the obstacles and stars to move
                
                //destroyers move up and down only.
                _destroyer1.Update(); 
                _destroyer2.Update();
                
                //check to see if any obstacles were hit by the destroyer(s) and if the
                //player has hit an obstacle
                _wallBuilder.DestroyObstaclesAndCollideWithPlayer(_destroyer1, _player);
                _wallBuilder.DestroyObstaclesAndCollideWithPlayer(_destroyer2, _player);
                
                //check to see if the player has pick up any stars
                _wallBuilder.DestroyStars(_player);

                //move the player
                _player.Update();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //cheap method to draw the background image
            DrawBackground();

            //draw obstacles and stars
            _wallBuilder.Draw(_spriteBatch);

            //draw the player
            _player.Draw(_spriteBatch);

            base.Draw(gameTime);
        }

        //cheap, but effective
        private void DrawBackground()
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(_backgroundTexture, new Vector2(0, -100), Color.White);
            _spriteBatch.End();
        }
    }
}