using Coursework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework
{
    class Boss : Entity
    {

        static Texture2D def = GameBase.Boss;// default boss texture
        static Texture2D hurt = GameBase.BossHurt; //boss hurt texture
        string enemyType; //type
        public int health = 5000; //health
        bool shotRecently;
        Vector2 direction;
        const int cooldownFrames = 6;
        int cooldownRemaining = 0;
        public static Random rand = new Random();


        public Boss(Vector2 position, string v)
        {
            image = def;
            Position = position;
            this.enemyType = v;
            Radius = image.Width / 2;
            drawnArea = new Rectangle(0, 0, 96, 70);
            entityHitbox = new Rectangle((int)Position.X, (int)Position.Y, image.Width, image.Height);
        }

        public override void Update()
        {
            //check if dead
            if (health <= 0)
            {
                IsExpired = true;
                GameBase.Explosion.Play(0.25f, -0.2f, 0);
                GameBase.currentLevel = GameBase.level.end;
            }
            //boss moves in from off screen
            if (Position.Y < 100)
            {
                Position.Y = Position.Y + 5;
            }

            //check cooldown on shoot
            if (cooldownRemaining <= 0 && Position.Y > 0)
            {
                //shoot
                shootAtPlayer();
                //set cooldown (shorter than base enemy)
                cooldownRemaining = rand.Next(10, 60);
            }                                   

            //follow players movement
            if (Position.X < Player.Instance.center.X && Position.X < 704)
            {
                Position.X += 2;
            }
            //follow players movement
            if (Position.X > Player.Instance.center.X && Position.X > 0)
            {
                Position.X -= 2;
            }

            // check if shot
            if (shotRecently == true)
            {
                //change image
                image = hurt;
                //reset flag
                shotRecently = false;
            }
            else
            {
                image = def;
            }
            //increment cooldown
            if (cooldownRemaining > 0)
                cooldownRemaining--;

            entityHitbox = new Rectangle((int)Position.X, (int)Position.Y, image.Width, image.Height);
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
                //receieve damage
                health = health - damage;

                shotRecently = true;
            }
        }

        public void shootAtPlayer()
        {
            //aim at player
            direction = new Vector2((Player.Instance.PlayerPos.X + (Player.Instance.entityHitbox.Width / 2)) - (Position.X + (image.Width / 2)), (Player.Instance.PlayerPos.Y + (Player.Instance.entityHitbox.Height / 2)) - (Position.Y + (image.Height / 2)));
            direction.Normalize();

            //shoot twice
            EntityManager.Add(new Bullet(new Vector2(Position.X + 5, Position.Y + (70)), direction, "enemyShot"));
            EntityManager.Add(new Bullet(new Vector2(Position.X + 40, Position.Y + (70)), direction, "enemyShot"));
            GameBase.enemyShotLaunch.Play(0.5f, -0.2f, 0);
        }

        public bool wasShotRecently()
        {
            return shotRecently;
        }
        //spawn the boss
        public static Boss createBoss(Vector2 position)
        {
            var boss = new Boss(position, "Boss");
            return boss;
        }

        public int getHealth()
        {
            return health;
        }

    }
}
