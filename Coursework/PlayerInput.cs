using Coursework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework
{
    static class PlayerInput
    {
        const int cooldownFrames = 6;
        static int cooldownRemaining = 0;
        static int movementSpeed = 8;
        //static UserInterface ui = new UserInterface();



        public static void Update()
        {
            //press esc to exit when boss is dead
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) && GameBase.currentLevel == GameBase.level.end)
            {
                GameBase.setState(GameBase.gameStates.mainMenu);
            }
    
            //press D to move right
                if (Keyboard.GetState().IsKeyDown(Keys.D) && !(Keyboard.GetState().IsKeyDown(Keys.A)) && Player.Instance.PlayerX < GameBase.Viewport.Width - (64 / Player.scaleFactor))
            {
                Player.Instance.PlayerX += movementSpeed;
                Player.Instance.SpriteXMod = 2;//change player's shown sprite to appropriate movement direction
            }
                //press A to move left
            else if (Keyboard.GetState().IsKeyDown(Keys.A) && !(Keyboard.GetState().IsKeyDown(Keys.D)) && Player.Instance.PlayerX > 0)
            {
                Player.Instance.PlayerX -= movementSpeed;
                Player.Instance.SpriteXMod = 3; //change player's shown sprite to appropriate movement direction
            }
            else
            {
                Player.Instance.SpriteXMod = 1;//change player's shown sprite to appropriate movement direction
            }

            //press W to move up
            if (Keyboard.GetState().IsKeyDown(Keys.W) && !(Keyboard.GetState().IsKeyDown(Keys.S)) && Player.Instance.PlayerY > 0)
            {
                Player.Instance.PlayerY -= movementSpeed;
            }
            //press S to move down
            else if (Keyboard.GetState().IsKeyDown(Keys.S) && !(Keyboard.GetState().IsKeyDown(Keys.W)) && Player.Instance.PlayerY < GameBase.Viewport.Height - 164)
            {
                Player.Instance.PlayerY += movementSpeed;
            }
            //press space to fire (with cooldown)
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && cooldownRemaining <= 0 && UserInterface.hasEnergy(4) == true)
            {
                cooldownRemaining = cooldownFrames;
                UserInterface.reduceEnergy(5);

                EntityManager.Add(new Bullet(Player.Instance.offsetPlayerPos, Player.Instance.vel, "defaultShot"));
                EntityManager.Add(new Bullet(Player.Instance.offsetPlayerPos2, Player.Instance.vel, "defaultShot"));
                GameBase.baseShotLaunch.Play(0.5f, -0.2f, 0);
            }
            //press 1 to fire eShot
            else if (Keyboard.GetState().IsKeyDown(Keys.D1) && cooldownRemaining <= 0 && UserInterface.hasEnergy(29) == true && UserInterface.hasEShot() > 0)
            {
                cooldownRemaining = cooldownFrames;
                UserInterface.reduceEnergy(30);
                UserInterface.updateEShot(1);
                EntityManager.Add(new Bullet(Player.Instance.offsetPlayerPosCentre, Player.Instance.vel, "eShot"));
                GameBase.eShotLaunch.Play(0.5f, -0.2f, 0);

            }

            if (cooldownRemaining > 0)
                cooldownRemaining--;

        }


    }
}
