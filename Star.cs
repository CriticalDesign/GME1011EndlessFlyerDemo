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
    //stars are pickups in our "game"
    internal class Star
    {
        //we need coordinates and a speed variable
        private float _myX, _myY, _mySpeed;
        
        //this is our texture for display
        private Texture2D _starTexture;
    
        //argumented constructor - a zero-arg one wouldn't be helpful
        public Star(float myX, float myY, float mySpeed, Texture2D starTexture)
        {
            //set all the attributes!!
            _myX = myX;
            _myY = myY;
            _mySpeed = mySpeed;
            _starTexture = starTexture;
        }

        //all a star does (on its own) is move to the left
        public void Update()
        {
            _myX -= _mySpeed;
        }

        //we'll use the star's rectangle to check for collisions with the player (pick up)
        public Rectangle getRectangle() { return new Rectangle((int)_myX, (int)_myY, _starTexture.Width, _starTexture.Height); }

        //draw all the stary goodness
        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(_starTexture, new Vector2(_myX, _myY), Color.White);
            _spriteBatch.End();
        }
    }

}
