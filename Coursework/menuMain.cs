using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coursework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Coursework
{
    class menuMain
    {
        public enum menuState { mainMenu, help }
        private menuState state;

        float scale = 1.02f;
        private Texture2D background;
        private Texture2D button;
        private SpriteFont font, font2;
        Rectangle DrawRect = new Rectangle(0, 0, 600, 800);
        Rectangle playButton;
        Rectangle helpButton;
        Rectangle backButton;

        Vector2 mousePoint;
        MouseState mouseState;

        public void Initialize()
        {
            playButton = new Rectangle(100, 300, button.Width, button.Height);
            helpButton = new Rectangle(100, 500, button.Width, button.Height);
            backButton = new Rectangle(100, 600, button.Width, button.Height);
        }


        public void loadContent(ContentManager content)
        {
            background = (content.Load<Texture2D>("Background1"));
            button = (content.Load<Texture2D>("MenuButton"));
            font = content.Load<SpriteFont>("MainMenuFont");
            font2 = content.Load<SpriteFont>("Font");

        }
        public void Update()
        {
            mouseState = Mouse.GetState(); //mouse position
            mousePoint = new Vector2(mouseState.X, mouseState.Y);



            //does the player click play
            if (playButton.Contains(mousePoint) && (mouseState.LeftButton == ButtonState.Pressed) && state == menuState.mainMenu)
            {
                //begin playing
                UserInterface.setIntitialValues();
                GameBase.setState(GameBase.gameStates.Playing); 
                EntityManager.Initialize();
                GameBase.setLevel(GameBase.level.one);


            }
                  
            //does the player click help
            if (helpButton.Contains(mousePoint) && (mouseState.LeftButton == ButtonState.Pressed) && state == menuState.mainMenu)
            {
                state = menuState.help; //send to help screenn
            }

            //does the player click back
            if (backButton.Contains(mousePoint) && (mouseState.LeftButton == ButtonState.Pressed) && state == menuState.help)
            {

                state = menuState.mainMenu; //send  back to main menu
            }
           

        }


        public void Draw(SpriteBatch spriteBatch)
        {
            switch (state)
            {

                case menuState.mainMenu:


                    spriteBatch.Draw(background, DrawRect, Color.White);

                    if (playButton.Contains(mousePoint))
                    {
                        spriteBatch.Draw(button, new Vector2(100, 300), Color.Gray); //change color of button if the player is hovering over it
                    }
                    else
                    {
                        spriteBatch.Draw(button, new Vector2(100, 300), Color.White);
                    }

                    if (helpButton.Contains(mousePoint))
                    {
                        spriteBatch.Draw(button, new Vector2(100, 500), Color.Gray);//change color of button if the player is hovering over it
                    }
                    else
                    {
                        spriteBatch.Draw(button, new Vector2(100, 500), Color.White);
                    }

                    spriteBatch.DrawString(font, "PLAY", new Vector2((playButton.X + 140), (playButton.Y + 50)), Color.Black);
                    spriteBatch.DrawString(font, "HELP", new Vector2((helpButton.X + 140), (helpButton.Y + 50)), Color.Black);

                    break;

                case menuState.help:

                    spriteBatch.Draw(background, DrawRect, Color.White);

                    if (backButton.Contains(mousePoint))
                    {
                        spriteBatch.Draw(button, new Vector2(100, 600), Color.Gray);//change color of button if the player is hovering over it
                    }
                    else
                    {
                        spriteBatch.Draw(button, new Vector2(100, 600), Color.White);
                    }

                    spriteBatch.DrawString(font, "BACK", new Vector2((backButton.X + 140), (backButton.Y + 50)), Color.Black);

                    spriteBatch.DrawString(font2, "                 Controls " +
                        System.Environment.NewLine +
                        System.Environment.NewLine + "Control your Ship with WASD" +
                        System.Environment.NewLine +
                        System.Environment.NewLine + "Press Space to fire your ship's basic repeaters" +
                        System.Environment.NewLine +
                        System.Environment.NewLine + "Press 1 to launch a lightning shot that " +
                        System.Environment.NewLine + "will destroy all enemies near the target " +
                        System.Environment.NewLine +
                        System.Environment.NewLine +

                        System.Environment.NewLine + "                 Story " +
                          System.Environment.NewLine +
                        System.Environment.NewLine + "The planet Salleon is currently the target" +
                          System.Environment.NewLine +
                        System.Environment.NewLine + "of a military blockade. It's people are " +
                          System.Environment.NewLine +
                        System.Environment.NewLine + "injured and starving from countless years " +
                          System.Environment.NewLine +
                        System.Environment.NewLine + "of war that has no end in sight. You have " +
                          System.Environment.NewLine +
                        System.Environment.NewLine + "been contracted to navigate the blockade " +
                          System.Environment.NewLine +
                        System.Environment.NewLine + "and deliver much needed supplies. ",

                        new Vector2(100, 150) + new Vector2(-1 * scale, -1 * scale),
                        Color.LightBlue, 0, new Vector2(0, 0),
                        scale, SpriteEffects.None, 1f);
                    spriteBatch.DrawString(font2, "You are the Blockade Runner.", new Vector2(100, 550), Color.DarkRed);


                    break;


            }






            spriteBatch.DrawString(font, "BLOCKADE" + System.Environment.NewLine + " RUNNER", new Vector2(185, 50) + new Vector2(-1 * scale, -1 * scale), Color.White, 0, new Vector2(0, 0), scale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(font, "BLOCKADE" + System.Environment.NewLine + " RUNNER", new Vector2(185, 50) + new Vector2(1 * scale, -1 * scale), Color.DarkRed, 0, new Vector2(0, 0), scale, SpriteEffects.None, 1f);


        }


    }
}
