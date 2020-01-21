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
    
    static class EnemySpawner
    {
        static int bossCount = 0;
        static Random rand = new Random();
        static float inverseSpawnChance = 40;

        public static void Update()
        {
            //if there aren't too many current enemies and the level is not in transition
            if (EntityManager.enemies.Count < 200 && (GameBase.currentLevel == GameBase.level.one || GameBase.currentLevel == GameBase.level.two || GameBase.currentLevel == GameBase.level.three))
            
            {
                switch (GameBase.currentLevel)
                {
                    case GameBase.level.one:
                        //only spawn base enemies
                        if (rand.Next((int)inverseSpawnChance) == 0)
                            EntityManager.Add(Enemy.createDualShot(GetSpawnPosition()));

                        if (inverseSpawnChance > 10)
                            inverseSpawnChance -= 0.005f;
                        break;

                    case GameBase.level.two:
                        //start to spawn seekers and base enemies
                        Random rand2 = new Random();
                        int chance = rand.Next(1, 101);

                        if (chance <= 60)
                        {
                            if (rand.Next((int)inverseSpawnChance) == 0)
                                EntityManager.Add(Enemy.createDualShot(GetSpawnPosition()));
                            if (inverseSpawnChance > 10)
                                inverseSpawnChance -= 0.005f;
                        }
                        else
                        {
                            if (rand.Next((int)inverseSpawnChance) == 0)
                                EntityManager.Add(Enemy.createSeeker(GetSpawnPosition()));

                            if (inverseSpawnChance > 10)
                                inverseSpawnChance -= 0.005f;
                        }
                        break;

                    case GameBase.level.three:
                       //only spawn the boss
                        if (bossCount == 0) //1 boss only
                        {                    
                            EntityManager.Add(Boss.createBoss(new Vector2(400, -100)));
                            bossCount= 1;
                        }
                        break;



                    default:
                        break;
                  


                }
               
                
            }

            // slowly increase the spawn rate as time progresses
           
        }

        private static Vector2 GetSpawnPosition()
        {
            Vector2 pos;
            do
            {
                pos = new Vector2(rand.Next((int)GameBase.ScreenSize.X), -50);
            }
            while (pos.X < 20 || pos.X >GameBase.ScreenSize.X - 50);

            return pos;
        }

        public static void Reset()
        {
            inverseSpawnChance = 60;
        }










































        //static Random rand = new Random();

        //const int cooldownFrames = 50;
        //static int cooldownRemaining;

        //public static void Update()
        //{



        //    int rInt = rand.Next(1, 5);
        //    if (cooldownRemaining <= 0)
        //    {



        //        for (int i = 0; i <= rInt; ++i)
        //        {
        //            EntityManager.Add(Enemy.createDualShot(GetSpawnPosition()));
        //        }

        //        cooldownRemaining = cooldownFrames;
        //    }




        //    if (cooldownRemaining > 0)
        //        cooldownRemaining--;

        //}

        //private static Vector2 GetSpawnPosition()
        //{
        //    Vector2 pos;

        //    float result = (Game1.ScreenSize.X );
        //    int rInt = rand.Next(25, 600);


        //    pos = new Vector2((rInt), 64);


        //    return pos;
        //}


    }

}

