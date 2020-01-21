using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Coursework
{
    class Enemy : Entity
    {

        int cutoff; //allows partial hiding behind ui
        int health = 100; //health
        bool shotRecently;//does the hurt frame need showing
        public static Texture2D def; //default frame
        public static Texture2D hurt; //hurt frame
        const int cooldownFrames = 6; //cooldown on shoot
        int cooldownRemaining = 0;
        Vector2 direction; //aim direction
        string enemyType; //seeker or default
        int offset = 1;
        int offsetMult = 16;
        public static Random rand = new Random();


        public Enemy(Texture2D image, Texture2D image2, Texture2D image3, Vector2 position, String type)
        {
            def = image; //set base image
            hurt = image2; //set hurt image
            enemyType = type; //set appropriate type

            if (enemyType == "seeker")
            {
                this.image = image3;
                drawnArea = new Rectangle(offset * offsetMult, 0, 16, 16);
            }
            else
            {
                this.image = def;
            }



            Position = position;
            Radius = image.Width / 2;
            entityHitbox = new Rectangle((int)Position.X, (int)Position.Y, image.Width, image.Height);
        }

        public static void died()
        {
            EntityManager.died();

        }


        public override void Update()
        {

            int seed = EntityManager.enemies.Count;
            Random rand = new Random(seed);


            //destroy this enemy
            if (health <= 0)
            {
                died();
                IsExpired = true;

                GameBase.Explosion.Play(0.25f, -0.2f, 0);


            }


            //different behaviours based on type
            switch (enemyType)
            {
                case "seeker":
                    //alternate frames using offset to change the drawn area
                    if (offset == 1)
                    {
                        offset = 0;
                    }
                    else
                    {
                        offset = 1;
                    }
                    drawnArea = new Rectangle(offset * offsetMult, 0, 16, 16);

                    //destroy if reaches bottom of screen
                    if (Position.Y > 670)
                        IsExpired = true;

                    //follow player while it is above them
                    if (Position.Y < Player.Instance.PlayerPos.Y)
                    {
                        //alter direction to follow player
                        direction = new Vector2((Player.Instance.PlayerPos.X + (Player.Instance.entityHitbox.Width / 2)) - (Position.X + (image.Width / 2)), (Player.Instance.PlayerPos.Y + (Player.Instance.entityHitbox.Height / 2)) - (Position.Y + (image.Height / 2)));
                        direction.Normalize();

                        //change position
                        Position = Position + direction * 5;

                    }
                    else
                    {
                        //move towards bottom if below player
                        Position.Y = Position.Y + 5;
                    }

                    //assign hitbox
                    entityHitbox = new Rectangle((int)Position.X, (int)Position.Y, this.image.Width, this.image.Height);

                    break;


                case "dual":

                    //check if shoot cooldown is complete and that the enemy is above the player
                    if (cooldownRemaining <= 0 && Position.Y < Player.Instance.PlayerPos.Y)
                    {
                        //shoot
                        shootAtPlayer();
                        //randomly set cooldown for unpredictability
                        cooldownRemaining = rand.Next(150, 250);
                    }

                    //check if this enemy was shot in the last update
                    if (shotRecently == true)
                    {
                        this.image = hurt; //change displayed image
                        shotRecently = false; //reset check
                    }
                    else
                    {
                        this.image = def;
                    }

                    //check if enemy is below the ui
                    cutoff = 720 - (int)Position.Y;
                    if (Velocity.X == 0)
                    {
                        //alter drawn area to only show the visible portion of the enemy
                        drawnArea = new Rectangle(0, 0, 50, Math.Min(50, cutoff)); 
                    }

                    //move down screen
                    Position.Y += 1;

                    //destroy if completely covered by ui
                    if (Position.Y > 720)
                        IsExpired = true;

                    Radius = drawnArea.Width / 2;

                    //increment cooldown
                    if (cooldownRemaining > 0)
                        cooldownRemaining--;

                    entityHitbox = new Rectangle((int)Position.X, (int)Position.Y, this.image.Width, this.image.Height);
                    break;
            }
        }

        public void WasShot(int damage)
        {
            //flag expired if dead
            if (health <= 0)
            {
                IsExpired = true;
            }
            else
            {
                //recieve damage
                health = health - damage;
                //flag shot recently
                shotRecently = true;
            }
        }

        public bool wasShotRecently()
        {
            return shotRecently;
        }


        public void shootAtPlayer()
        {
            //set direction vector to aim at player
            direction = new Vector2((Player.Instance.PlayerPos.X + (Player.Instance.entityHitbox.Width / 2)) - (Position.X + (image.Width / 2)), (Player.Instance.PlayerPos.Y + (Player.Instance.entityHitbox.Height / 2)) - (Position.Y + (image.Height / 2)));
            direction.Normalize();

            //create enemy bullet
            EntityManager.Add(new Bullet(new Vector2(Position.X + 10, Position.Y + (image.Width / 2)), direction, "enemyShot"));
            GameBase.enemyShotLaunch.Play(0.5f, -0.2f, 0);
        }

        //base enemies cannot move horizontally so won't collide after spawning
        public void HandleCollision(Enemy other)
        {
            IsExpired = true;
            
        }

        //create base enemy
        public static Enemy createDualShot(Vector2 position)
        {
            var enemy = new Enemy(GameBase.dualShot, GameBase.dualShot2, GameBase.seekerMine, position, "dual");
            return enemy;
        }
        //create seeker Mine
        public static Enemy createSeeker(Vector2 position)
        {
            var enemy = new Enemy(GameBase.dualShot, GameBase.dualShot2, GameBase.seekerMine, position, "seeker");
            return enemy;
        }

    }
}
