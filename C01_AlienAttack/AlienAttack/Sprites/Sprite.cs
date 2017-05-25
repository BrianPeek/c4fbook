using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AlienAttack
{
	public abstract class Sprite
	{
		// all textures in animation set
		protected Texture2D[] spriteTextures;

		// current frame to draw
		protected int frameIndex;

		public ContentManager Content;
		public Vector2 Position;
		public Vector2 Velocity;

		// bounding box of image...used for collision detection
		private Rectangle boundingBox;

		public Sprite()
		{
		}

		public Sprite(ContentManager contentManager)
		{
			this.Content = contentManager;
		}

		public Sprite(ContentManager contentManager, string contentName) : this(contentManager)
		{
			spriteTextures = new Texture2D[1];

			// load the image
			spriteTextures[0] = this.Content.Load<Texture2D>(contentName);
		}

		public virtual void Update(GameTime gameTime)
		{
			// move the sprite based on the provided velocity
			this.Position += this.Velocity;
		}

		public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(spriteTextures[frameIndex], this.Position, Color.White);
		}

		public virtual int Width
		{
			get { return spriteTextures[0].Width; }
		}

		public virtual int Height
		{
			get { return spriteTextures[0].Height; }
		}

		public virtual Rectangle BoundingBox
		{
			get 
			{
				// only need to assign this once
				if(boundingBox == Rectangle.Empty)
				{
					boundingBox.Width = this.Width;
					boundingBox.Height = this.Height;
				}
				boundingBox.X = (int)this.Position.X;
				boundingBox.Y = (int)this.Position.Y;

				return boundingBox;
			}
		}
	}
}
