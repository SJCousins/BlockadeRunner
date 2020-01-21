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
    class Bullet : Entity
    {
        public string bulletType; //type of bullet      

        public Bullet(Vector2 position, Vector2 velocity, string type)
        {
           
            bulletType = type;
            drawnArea = new Rectangle(0, 0, 8, 8);
            entityHitbox = new Rectangle(0, 0, 8, 8);
            switch (bulletType)
            {
                //set correct image
                case "defaultShot":
                    image = GameBase.defaultBullet;
                    break;

                case "eShot":
                    image = GameBase.eShot;
                    break;
                case "enemyShot":
                    image = GameBase.enemyShot;                   
                    break;
            }            
           
            Position = position;
            Velocity = velocity;
            Radius = image.Width / 2f;
           

        }

        public override void Update()
        {
            center = new Vector2(Position.X + (entityHitbox.Width / 2), Position.Y + (entityHitbox.Height / 2));
            //move enemy bullet down
            if (bulletType == "enemyShot")
            {                
                Position += Velocity * 5;                
            }
            Position += Velocity;

            // delete bullets that go off-screen
            if (!GameBase.Viewport.Bounds.Contains(Position.ToPoint()) || Position.Y > 700)
                IsExpired = true;
        }

        

    }
}
