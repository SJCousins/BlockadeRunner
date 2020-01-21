using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coursework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Coursework
{
    class EntityManager
    {
        static List<Entity> entities = new List<Entity>();
        static List<Entity> player = new List<Entity>();
        public static List<Enemy> enemies = new List<Enemy>();
        public static List<Boss> bosses = new List<Boss>();
        static List<Bullet> bullets = new List<Bullet>();
        static List<Enemy> enemiesInRange = new List<Enemy>();
        static Vector2 startPoint;
        static bool isUpdating;
        static List<Entity> addedEntities = new List<Entity>();
        public static int Count { get { return entities.Count; } }

        public static void Initialize()
        {
            //remove all entities and reset player position
            entities.Clear();
            player.Clear();
            Player.Instance.PlayerX = ((int)GameBase.ScreenSize.X / 2) - 22;
            Player.Instance.PlayerY = 500;
            enemies.Clear();
            bullets.Clear();
            enemiesInRange.Clear();
        }


        public static void Add(Entity entity)
        {
            if (!isUpdating)
                AddEntity(entity);
            else
                addedEntities.Add(entity);
        }

        private static void AddEntity(Entity entity)
        {
            entities.Add(entity);
            if (entity is Bullet)
                bullets.Add(entity as Bullet);
            else if (entity is Boss)
                bosses.Add(entity as Boss);

            else if (entity is Enemy)
                enemies.Add(entity as Enemy);
            else if (entity is Player)
                player.Add(entity as Player);
        }

        public static void Update()
        {
            isUpdating = true;

            //check all collisions
            HandleCollisions();

            foreach (var entity in entities)
                entity.Update();

            isUpdating = false;

            foreach (var entity in addedEntities)
                AddEntity(entity);
            addedEntities.Clear();

            entities = entities.Where(x => !x.IsExpired).ToList();
            bullets = bullets.Where(x => !x.IsExpired).ToList();
            enemies = enemies.Where(x => !x.IsExpired).ToList();
        }

        static void HandleCollisions()
        {
            // handle collisions between enemies
            for (int i = 0; i < enemies.Count; i++)
                for (int j = i + 1; j < enemies.Count; j++)
                {
                    if (IsColliding(enemies[i], enemies[j]))
                    {
                        //before enemies enter screen, remove any colliding enemies
                        if (enemies[i].Position.Y < 0 || enemies[i].Position.Y < 0)
                        {
                            enemies[j].IsExpired = true;
                        }
                        else
                        {         
                        }
                    }
                }


            // handle collisions between enemy bullets and player

            for (int j = 0; j < bullets.Count; j++)
            {
                //check if player has been hit
                if (Player.Instance.entityHitbox.Contains(bullets[j].center) && bullets[j].bulletType == "enemyShot")// && bullets[j].bulletType == "enemyShot")
                {
                    //take 10 damage
                    UserInterface.updateHealth(10);
                    //remove bullet
                    bullets[j].IsExpired = true;
                }
            }
            //handle collisions between bullets and boss    
            for (int i = 0; i < bosses.Count; i++)
                for (int j = 0; j < bullets.Count; j++)
                {
                    if (bosses[i].Position.Y > 0)
                    {
                    }
                    if (IsColliding(bosses[i], bullets[j]))
                    {
                        //check if boss was hit by base bullet
                        if (bullets[j].bulletType == "defaultShot")
                        {
                            //boss takes 25 damage
                            bosses[i].WasShot(25);
                            bullets[j].IsExpired = true;
                        }
                        //check if boss was hit by eshot
                        if (bullets[j].bulletType == "eShot")
                        {
                            startPoint = bullets[j].Position;
                            //boss takes 100 damage
                            bosses[i].WasShot(100);
                        }
                    }
                }
            //handle collisions between bullets and enemies                       
            for (int i = 0; i < enemies.Count; i++)
                for (int j = 0; j < bullets.Count; j++)
                {

                    if (enemies[i].Position.Y > 0)
                    {
                    }
                    if (IsColliding(enemies[i], bullets[j]))
                    {
                        if (bullets[j].bulletType == "defaultShot")
                        {
                            //25 damage dealt by base shot
                            enemies[i].WasShot(25);
                            bullets[j].IsExpired = true;
                        }

                        if (bullets[j].bulletType == "eShot")
                        {

                            startPoint = bullets[j].Position; //store collision position
                            //100 damage dealt by eshot
                            enemies[i].WasShot(101);
                            foreach (var Enemy in enemies)
                            {
                                if (Enemy == enemies[i])
                                {
                                }
                                else
                                {
                                    double c;
                                    c = (Math.Pow(enemies[i].Position.X - Enemy.Position.X, 2) + Math.Pow(enemies[i].Position.Y - Enemy.Position.Y, 2));

                                    if (c <= 25000)
                                    {
                                        //store all enemies within range of eshot AOE
                                        Enemy.WasShot(101);
                                        enemiesInRange.Add(Enemy as Enemy);
                                    }
                                }
                            }
                            bullets[j].IsExpired = true;
                        }
                    }
                }

            // handle collisions between the player and enemies
            for (int i = 0; i < enemies.Count; i++)
            {
                if (Player.Instance.entityHitbox.Intersects(enemies[i].entityHitbox))
                {
                    //player takes 50 damage, enemy takes 100
                    UserInterface.updateHealth(50);
                    enemies[i].WasShot(100);
                    break;
                }
            }

            // handle collisions between the player and boss
            for (int i = 0; i < bosses.Count; i++)
            {
                if (Player.Instance.entityHitbox.Intersects(bosses[i].entityHitbox))
                {
                    UserInterface.updateHealth(50);
                    bosses[i].WasShot(100);
                    break;
                }
            }

        }

        public static void died()
        {
            //update killcount on dead enemy to help check when to change level
            UserInterface.killCount += 1;
        }


        private static bool IsColliding(Entity a, Entity b)
        {
            float radius = a.Radius + b.Radius;
            return !a.IsExpired && !b.IsExpired && Vector2.DistanceSquared(a.Position, b.Position) < radius * radius;
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (var enemy in enemiesInRange)
            {
                DrawLine(spriteBatch, GameBase.eShotBeam, startPoint, enemy.Position);
            }
            enemiesInRange.Clear();
            foreach (var entity in entities)
                entity.Draw(spriteBatch);
        }

        static void DrawLine(SpriteBatch sb, Texture2D texture, Vector2 start, Vector2 end)
        {
            Vector2 edge = end - start;
            float angle = (float)Math.Atan2(edge.Y, edge.X);
            //stretch eshot texture to all hit targets
            sb.Draw(texture, new Rectangle((int)start.X, (int)start.Y, (int)edge.Length(), 15), null, Color.LightBlue, angle, new Vector2(0, 0), SpriteEffects.None, 0);
        }
    }
}
