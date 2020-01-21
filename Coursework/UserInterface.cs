using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Coursework.Content
{
    static class UserInterface
    {
        static private Texture2D ui, barBack, eShotAmmo, baseAmmo;

        static int eShotAmmoNum = 10; //current eshot ammo count
        static Rectangle DrawRect = new Rectangle(0, 700, 600, 100);
        static Rectangle smallRect = new Rectangle(300, 705, 295, 90);
        static private SpriteFont font;
        static private SpriteFont font2;
        static private int health = 100;//player health 
        static private int energy = 100;//player energy
        static Rectangle Health;
        static Rectangle Energy;
        static Rectangle bossHealthOutline = new Rectangle(49, 19, 500, 10);
        static Rectangle EnergyDrained;
        static Rectangle baseAmmoDraw = new Rectangle(150, 715, 50, 50); //ammo icon
        static Rectangle eShotAmmoDraw = new Rectangle(210, 715, 50, 50); //ammo icon
        static bool energyDrained = false; //is the player at 0 energy
        static Color healthColor = Color.Green; //color of health bar
        const int cooldownFrames = 2;
        static int cooldownRemaining = 0;
        const int cooldownFrames2 = 15;
        static int cooldownRemaining2 = 0;
        const int energyCoolDownTime = 200;
        static int flashCooldown;
        static bool flashOn = true;
        public static int killCount;
        static float timeCount = 0f;



        static public void loadContent(ContentManager content)
        {
            ui = (content.Load<Texture2D>("UI"));
            barBack = (content.Load<Texture2D>("BarBack"));
            eShotAmmo = (content.Load<Texture2D>("eShotAmmo"));
            baseAmmo = (content.Load<Texture2D>("BaseShotAmmo"));
            font = content.Load<SpriteFont>("Font");
            font2 = content.Load<SpriteFont>("MainMenuFont");
        }
        static public void Initialize()
        {
            energy = 100;
            eShotAmmoNum = 10;
        }
        //alter the player's current energy
        static public void reduceEnergy(int reduceNum)
        {
            energy = energy - reduceNum;
        }
        //does the player have energy currently
        static public bool hasEnergy(int checkNum)
        {
            if (energy > checkNum)
            {
                return true;

            }
            else
                return false;
        }
        //set the starting values
        static public void setIntitialValues()
        {
            health = 100;
            energy = 100;
            killCount = 0;
            timeCount = 0;
            eShotAmmoNum = 10;
        }

        static public void Update(GameTime time)
        {
            //check time since last update
            timeCount += (float)time.ElapsedGameTime.TotalSeconds;

            //move to next level when enough enemies are killed
            if (killCount > 50 && GameBase.currentLevel == GameBase.level.one)
            {
                killCount = 0;
                GameBase.currentLevel = GameBase.level.transitionOne;

                setIntitialValues();
            }
            //preperation time and to warn player of upcoming threats
            if (timeCount > 10f && GameBase.currentLevel == GameBase.level.transitionOne)
            {
                GameBase.currentLevel = GameBase.level.two;
                timeCount = 0;
                setIntitialValues();
            }

            //move to next level when the player has survived long enough
            if (timeCount > 40f && GameBase.currentLevel == GameBase.level.two)
            {
                GameBase.currentLevel = GameBase.level.transitionTwo;
                timeCount = 0;
                setIntitialValues();
            }
            //preperation time and to warn player of upcoming threats
            if (timeCount > 10f && GameBase.currentLevel == GameBase.level.transitionTwo)
            {
                GameBase.currentLevel = GameBase.level.three;
                setIntitialValues();
            }
            
            //check if player is dead
            if (health <= 0)
            {
                Player.Instance.dead();
            }

            //flash energy bar when energy is drained
            if (flashCooldown <= 0)
            {
                flashOn = !flashOn;
                flashCooldown = 20;
            }
            else
            {
                flashCooldown--;
            }

            //refill energy when drain times is completed
            if (cooldownRemaining <= 0 && energyDrained == true)
            {
                energy = 100;
                energyDrained = false;
            }


            if (cooldownRemaining <= 0)
            {
                cooldownRemaining = cooldownFrames;
                //cap energy to 100
                if (energy >= 100)
                {
                    energy = 100;
                }
                //can't go below 0 either
                else if (energy <= 0)
                {
                    energy = 0;
                }
                //if 0 stop energy recovery for a short time
                if (energy == 0)
                {
                    energyDrained = true;
                    cooldownRemaining = energyCoolDownTime;
                }
                else
                {
                    //energy recovery
                    energy = energy + 1;
                }
            }

            //change length of health bar relative to current health
            Health = new Rectangle(20, 730, health, 10);
            //change length of energy bar relative to current energy
            Energy = new Rectangle(20, 765, energy, 10);
            EnergyDrained = new Rectangle(20, 765, 100, 10);
            switch (health)
            {
                //green when full health
                case 100:
                    healthColor = Color.Green;
                    break;
                    //yellow when in between full and low
                case int n when (n < 100 && n >= 30):
                    healthColor = Color.Yellow;
                    break;
                    //red when low health
                case int n when (n < 30):
                    healthColor = Color.Red;
                    break;
            }
            if (cooldownRemaining > 0)
                cooldownRemaining--;

            if (cooldownRemaining2 > 0)
                cooldownRemaining2--;



        }
        //update ammo count
        static public void updateEShot(int num)
        {
            eShotAmmoNum = eShotAmmoNum - num;
        }
        //check if ammo available
        static public int hasEShot()
        {
            return eShotAmmoNum;
        }
        //recieve damage
        static public void updateHealth(int damage)
        {
            health = health - damage;
        }

        static public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ui, DrawRect, Color.White);
            spriteBatch.Draw(ui, smallRect, Color.Gray);
            spriteBatch.DrawString(font, "Health", new Vector2(20, 715), Color.Black);
            spriteBatch.Draw(barBack, Health, healthColor);
            spriteBatch.DrawString(font, "Energy", new Vector2(20, 750), Color.Black);
            spriteBatch.Draw(eShotAmmo, eShotAmmoDraw, Color.White);
            spriteBatch.Draw(baseAmmo, baseAmmoDraw, Color.White);
            spriteBatch.DrawString(font, "INF", new Vector2(160, 770), Color.Black);
            spriteBatch.DrawString(font, eShotAmmoNum.ToString(), new Vector2(230, 770), Color.Black);

            //warn of seeker mines during transition one
            if (GameBase.currentLevel == GameBase.level.transitionOne)
            {
                spriteBatch.DrawString(font, "Nicely done, But it's not over" + System.Environment.NewLine + "yet. I'm seeing Seeker Mines" + System.Environment.NewLine + "inbound.", new Vector2(305, 710), Color.Black);
            }

            //warn of boss  during transition 2
            if (GameBase.currentLevel == GameBase.level.transitionTwo)
            {
                spriteBatch.DrawString(font, "Something Big is coming." + System.Environment.NewLine + "Be Careful!", new Vector2(305, 710), Color.Black);
            }
            //prompt return to menu
            if (GameBase.currentLevel == GameBase.level.end)
            {
                spriteBatch.DrawString(font2, "Thank you" + System.Environment.NewLine + "For Playing!", new Vector2(150, 200), Color.Gold);
                spriteBatch.DrawString(font, "press ESC to return to the menu!", new Vector2(150, 300), Color.Gold);
            }

            if (flashOn == true && energyDrained == true)
            {
                spriteBatch.Draw(barBack, EnergyDrained, Color.Red);
            }
            else
            {
                spriteBatch.Draw(barBack, Energy, Color.Purple);

            }
        }
    }
}
