using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GME1011EndlessFlyer
{
    //our wall is built of obstacles
    internal class Obstacle
    {
        //like most things in the program, we need coordinates and speed
        //obstacles simply move from right to left (endlessly)
        private float _myX, _myY, _mySpeed;
        
        //this is our lovely texture
        private Texture2D _obstacleTexture;

        //argumented constructor (zero-arg constructor not useful here)
        public Obstacle(int myX, int myY, Texture2D obstacleTexture, float speed)
        {
            //set all the attributes!!!
            this._myX = myX;
            this._myY = myY;
            this._obstacleTexture = obstacleTexture;
            this._mySpeed = speed;
        }

        //all an obstacle does is move from right to left (endlessly)
        public void Update()
        {
            _myX -= _mySpeed;
        }

        //this rectangle defines the bounds of an obstacle and will be used in detecting collisions
        //with the player
        public Rectangle getRectangle() { return new Rectangle((int)_myX, (int)_myY, _obstacleTexture.Width, _obstacleTexture.Height); }

        //these four methods will help us with collision dection between the obstacles and the
        //destroyer object
        public float getX() { return _myX; }
        public float getY() { return _myY; }
        public float getWidth() {return _obstacleTexture.Width; }
        public float getHeight() { return _obstacleTexture.Height; }

        //draw our beautiful obstacles (so pretty)
        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(_obstacleTexture, new Vector2(_myX, _myY), Color.White);
            _spriteBatch.End();
        }
    }
}
