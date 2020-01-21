using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Coursework
{
    class Background
    {

      
        private Texture2D background;
        

        Rectangle rect1;
        Rectangle rect2;
        
        public void Initialize()
        {
            //background 1
            rect1 = new Rectangle(0, 0, 960, 3840);
            rect2 = new Rectangle(0, 3840, 960, 3840);
          
        }

        public void loadContent(ContentManager content)
        {
            background = (content.Load<Texture2D>("background1"));
        

        }

        public void Update()
        {
            //Background 1
            if (rect1.Y  >= 1280)
            {
                rect1.Y = 0 - rect2.Y - background.Height;
            }     
            if (rect2.Y  >= 1280)
            {
                rect2.Y = 0 - rect1.Y - background.Height;
            }
            rect1.Y += +5;
            rect2.Y += +5;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
  
          spriteBatch.Draw(background, rect1, Color.White);
            spriteBatch.Draw(background, rect2, Color.White);
                }



    }
}


