using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace GME1011EndlessFlyer
{
    internal class WallBuilder
    {
        //this is the most complicated class in the bunch. The Wallbuilder spawns all of the obstacles
        //in a "wall" formation. The destroyer interacts with the individual obstacles to carve a path
        //through the wall. The player interacts with the obstacles to record a crash (on collision).
        //Stars (pick ups) are occaisionally left behind during the carving. Stars are picked up
        //but the player. All these activites happen in here!!

        //a list is like an array but more convenient. We're going to keep a list of all
        //the obstacles and a list of all the stars in our scene. This way we can add to the list
        //and remove from the list as-needed!
        private List<Obstacle> _obstacles;
        private List<Star> _stars;

        //since the wall builder spawns both obstacles and stars, we need to pass those textures in
        private Texture2D _obstacleTexture, _starTexture;

        //this is the frequency with which we add a column of obstacles
        private float _repeatTime;

        //good ol'rng to keep things interesting
        private Random _rng;

        //argumented constructor, but with only two arguments, everything else is "hard-coded"
        public WallBuilder(Texture2D obstacleTexture, Texture2D starTexture)
        {
            //set by argument
            _obstacleTexture = obstacleTexture;  
            _starTexture = starTexture; 

            //hard-coded
            _obstacles = new List<Obstacle>(); //initialize obstacle list
            _stars = new List<Star>();  //initialize star list
            _repeatTime = 1.0f; //frequence of obstacle column creation (don't mess with this
                                //without also messing with the obstacle speed).

            _rng = new Random(); //initialize rng
        }


        //this is a method that simply waits an alotted amount of time and then creates a column
        //of obstacles.
        public void Update(GameTime gameTime)
        {
            //here is the waiting
            if (_repeatTime > 0)
            {
                _repeatTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;  //wait
                if (_repeatTime <= 0)
                {
                    _repeatTime = 0;

                    //here is the column. Notice how the x-coordinate (1400) is to the right of
                    //our 1200 pixel screen. We want the obstacles to spawn off-screen. Then
                    //we use the i-coordinate to create the column of 20 stacked obstacles all
                    //with x speed of 1.5f.
                    for (int i = 0; i < 20; i++)
                    {
                        _obstacles.Add(new Obstacle(1400, -100 + i * 50, _obstacleTexture, 1.5f));
                    }

                    _repeatTime = 0.55f; //reset the timer!!!
                }

            }

        }

        //this is a method we'll call in our game's update. It is designed to help the destroyer
        //carve the path (by looking for collisions) and to help determine if the player has 
        //hit an obstacle. We do this here because the WallBuilder keeps track of all the obstacles
        //and stars (we'll deal with stars after this method).
        public void DestroyObstaclesAndCollideWithPlayer(Destroyer destroyer, Player player)
        {
            //flag to see if we're going to destroy an obstacle
            bool flagForDestroy = false;

            //get the destroy object's x and y (we need to know where it is)
            float destroyerX = destroyer.getX();
            float destroyerY = destroyer.getY();

            //this is a meaty loop - for EVERY obstacle in the game...
            for (int i = 0; i < _obstacles.Count; i++)
            {
                //grab the obstacle
                Obstacle o = _obstacles.ElementAt(i);

                //get the obstacle's x, y, width and height
                float obstacleX = o.getX();
                float obstacleY = o.getY();
                float obstacleWidth = o.getWidth();
                float obstacleHeight = o.getHeight();

                //only do this part if the obstacle hasn't yet entered our view. We want to carve the
                //path prior to the obstacles entering our play area, not after. This will save
                //a bit of computation since we won't try to carve a path in obstacles that have
                //already entered the scene.
                if (obstacleX > 1200)
                {
                    //check to see if the obstacle in question is in contact with the destroyer.
                    //It's worth noting that the destroyer is only an x/y pair, it doesn't have
                    //height and width. The * 2f below helps us to carve a slightly larger path
                    //to avoid frustrating out player.
                    if (destroyerX > obstacleX && destroyerY > obstacleY && destroyerX < obstacleX + obstacleWidth * 2f && destroyerY < obstacleY + obstacleHeight * 2f)
                        flagForDestroy = true;

                    //here's the thing - we need to destroy blocks that have come in contact with 
                    //the destroy AND if they have left the scene. This second part is key to making 
                    //sure that we don't have a zillion blocks that have passed through the scene -
                    //that will eventually slow our game down or crash it.
                    if (o.getX() < -50 || flagForDestroy)
                    {
                        _obstacles.RemoveAt(i);  //obstacle was hit by the destroyer or is off-screen
                                                //Get rid of it!
                        
                        //cute little bit of code that says a destroyed obstacle has a 5% chance
                        //of spawning a pick up star. Awwww, that's cute.
                        if(_rng.NextDouble() < 0.05)
                        {
                            _stars.Add(new Star(obstacleX, obstacleY, 1.5f, _starTexture));
                        }
                    }

                    //reset the flag for the next block to check
                    flagForDestroy = false;
                }
                else
                {   
                    //it takes this much code to see if an obstacle has hit our player. 
                    //If it has, set the player crashed variable to stop the action.
                    if (player.getRectangle().Intersects(o.getRectangle())) 
                        player.setCrashed();
                }
            }
        }

        //similar, but more consice than what we did above, check EVERY star in the game to see
        //if it's overlapping the player. If it is, pick it up!! and remove it from the list.
        public void DestroyStars(Player player)
        {
            for (int i = 0; i < _stars.Count; i++)
            {
                Star s = _stars.ElementAt(i);

                if (player.getRectangle().Intersects(s.getRectangle()))
                     _stars.RemoveAt(i);
            }
        }

        //move all the obstacles and stars by calling their update functions. This will be called
        //in Game1.cs update().
        public void MoveObstaclesAndStars()
        {
            foreach (Obstacle o in _obstacles)
            {
                o.Update();
            }

            foreach (Star s in _stars)
            {
                s.Update();
            }
        }

        //draw all our obstacles and stars <3
        public void Draw(SpriteBatch _spriteBatch)
        {
            foreach (Obstacle o in _obstacles)
            {
                o.Draw(_spriteBatch);
            }

            foreach (Star s in _stars)
            {
                s.Draw(_spriteBatch);
            }
        }

    }
}
