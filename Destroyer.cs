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
    internal class Destroyer
    {
        //this is an unusual "helper" class. We won't see it at all in the game
        //but it simply moves up and down to "carve" a path in the wall of obstacles.

        //coordinates and a speed
        private float _myX, _myY, _mySpeed;

        //a boolean to know if we're going up or down
        private bool _goingDown;

        //argumented constructor (a zero arg one doesn't make sense for this object)
        public Destroyer(float speed, float myX, float myY)
        {
            //set all the attributes!
            this._mySpeed = speed;
            this._myX = myX;
            this._myY = myY;

            //hard code this because it doesn't matter too much, but you could set it with an argument
            //if you wanted.
            _goingDown = true;
        }

        //looks complex, but this simply says to change from up to down when you get to the
        //edge of the screen. If we're going up, subtract from myY, if we're going down
        //add to myY. The destroy ONLY moves up and down. 
        public void Update()
        {
            if( _myY > 600 ) { _goingDown = false; }
            if( _myY < 0) { _goingDown = true; }

            if( _goingDown ) { _myY += _mySpeed; }
            else _myY -= _mySpeed;
        }

        //these two methods will help us with collision dection between the obstacles and the
        //destroyer object
        public float getX() { return _myX; }
        public float getY() { return _myY; }

    }
}
