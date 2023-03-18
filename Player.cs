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
    //the player of our game!!
    internal class Player
    {
        //coordinates and speed please
        private float _myX, _myY, _mySpeed;

        //this is what we look like
        private Texture2D _playerTexture;

        //we need to know if we've crashed into one of the obstacles
        private bool _crashed;

        //argumented constructor (a zero arg one wouldn't be helpful at all imho)
        public Player(float myX, float myY, Texture2D playerTexture, float speed)
        {
            //set all the attributes!!
            this._myX = myX;
            this._myY = myY;
            this._mySpeed = speed;
            this._playerTexture = playerTexture;

            //don't allow a pre-crashed player, hard-code this to not crashed.
            this._crashed = false;
        }

        //the only thing that the player does on it's own is move in the four directions
        //using keyboard mappings.
        public void Update()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                _myX -= _mySpeed;
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                _myX += _mySpeed;
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                _myY -= _mySpeed;
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                _myY += _mySpeed;
        }

        //the player's rectangle (bounds) will be used to detect collisions with 
        //stars and obstacles.
        public Rectangle getRectangle() { return new Rectangle((int)_myX, (int)_myY, _playerTexture.Width, _playerTexture.Height); }

        //two quick methods to set and to access our crashed variable.
        public void setCrashed() { _crashed = true; }
        public bool hasCrashed() { return _crashed; }

        //draw the player's beauty
        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(_playerTexture, new Vector2(_myX, _myY), Color.White);
            _spriteBatch.End();
        }
    }
}
