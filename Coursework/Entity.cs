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
    abstract class Entity
    {
        protected Texture2D image;
        public Rectangle drawnArea;
        public Rectangle entityHitbox;
        protected Color color = Color.White;
        public float Orientation;
        public Vector2 center;
        public Vector2 Position, Velocity;
        public float Radius = 1;   // used for circular collision detection
        public bool IsExpired;      // true if the entity was destroyed and should be deleted.                

        public Vector2 Size
        {
            get
            {
                return image == null ? Vector2.Zero : new Vector2(image.Width, image.Height);
            }
        }

        public abstract void Update();

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, Position, drawnArea, color, Orientation, Size / 2f, 1f, 0, 0);
        }
    }
}


