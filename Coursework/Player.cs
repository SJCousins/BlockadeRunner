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
    class Player : Entity
    {
        private static Player instance;
        public static Player Instance
        {
            get
            {
                if (instance == null)
                    instance = new Player();

                return instance;
            }
        }

        public static Texture2D shipSprite;   //ship texture
        public int SpriteXMod = 1;
        public int PlayerX = ((int)GameBase.ScreenSize.X / 2) - 22; //player's current X
        public int PlayerY = 500; //player's current Y
        public Rectangle Sprite1; //ship drawn area
        public Vector2 PlayerPos; //player's position
        public Vector2 offsetPlayerPos; //position of left gun
        public Vector2 offsetPlayerPos2; //position of right gun
        public Vector2 offsetPlayerPosCentre;//centre of player's sprite
        public Vector2 origin = new Vector2(0, 0);
        public Vector2 vel = new Vector2(0, -10);
       
        public static float scale = 1.0f;
        public static int scaleFactor = 1;

        public override void Update()
        {
            //calculate centre
            center = new Vector2(PlayerPos.X + ((entityHitbox.Width / 2)/ scaleFactor), PlayerPos.Y + ((entityHitbox.Height / 2)/ scaleFactor));
            //calculate location of player
            PlayerPos = new Vector2(PlayerX, PlayerY);
            //set left gun pos
            offsetPlayerPos = new Vector2((PlayerX + (48/ scaleFactor)) , (PlayerY + (5/scaleFactor))) ;
            //set right gun pos
            offsetPlayerPos2 = new Vector2((PlayerX + 16), (PlayerY + 5));
            offsetPlayerPosCentre = new Vector2((PlayerX + (32/scaleFactor)), (PlayerY + (5/ scaleFactor)));
            Sprite1 = new Rectangle(SpriteXMod * 64, 0, 64, 64);
            entityHitbox = new Rectangle((int)PlayerPos.X, (int)PlayerPos.Y, 55/scaleFactor, 40 /  scaleFactor);
        }

        private Player()
        {            
            shipSprite = GameBase.ship;
            //drawn correct frame based on movement direction
            Sprite1 = new Rectangle(SpriteXMod * 64, 0, 64, 64); 
            PlayerPos = new Vector2(PlayerX, PlayerY);
            Radius = entityHitbox.Width / 2f;
        }

        public void dead()
        {
            GameBase.setState(GameBase.gameStates.mainMenu);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {      
            spriteBatch.Draw(shipSprite, PlayerPos, Sprite1, Color.White, 0.0f, origin, scale, SpriteEffects.None, 0.0f);
           
        }
    }
}
