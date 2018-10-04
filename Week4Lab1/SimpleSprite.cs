using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Sprites
{
    public class SimpleSprite
    {
        public Texture2D Image;
        public Vector2 Position;
        public Vector2 Velocity;
        public Rectangle BoundingRect;
        public bool Visible = true;
        public float Width, Height;

        public SimpleSprite(Texture2D spriteImage,
                            Vector2 startPosition)
        {
            Image = spriteImage;
            Position = startPosition;
            BoundingRect = new Rectangle((int)startPosition.X, (int)startPosition.Y, Image.Width, Image.Height);
            Width = Image.Width;
            Height = Image.Height;
        }

        public void draw(SpriteBatch sp)
        {
            if(Visible)
                sp.Draw(Image, Position, Color.White);
        }

        public void Move(Vector2 delta)
        {
            Position += delta;
            BoundingRect = new Rectangle((int)Position.X, (int)Position.Y, Image.Width, Image.Height);
            BoundingRect.X = (int)Position.X;
            BoundingRect.Y = (int)Position.Y;
        }

        public bool inCollision(SimpleSprite other)
        {
            return this.BoundingRect.Intersects(other.BoundingRect);
        }

        public void displayMessage(string msg, SpriteBatch sB, SpriteFont font, Color colour)
        {         
            if (Visible)
            {
                Vector2 textSize = font.MeasureString(msg);
                Vector2 msgPos = new Vector2((this.Position.X + this.Width / 2) - textSize.X / 2, this.Position.Y - textSize.Y + 5);
                sB.DrawString(font, msg, msgPos, colour);
            }
        }
    }
}
