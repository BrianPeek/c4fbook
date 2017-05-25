using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AlienAttack
{
	public class Explosion : Sprite
	{
		double lastTime;

		public Explosion(ContentManager contentManager)
		{
			spriteTextures = new Texture2D[10];

			for(int i = 0; i <= 9; i++)
				spriteTextures[i] = contentManager.Load<Texture2D>("gfx\\explosion\\explosion_" + i);
		}

		public new bool Update(GameTime gameTime)
		{
			// if it's the final frame, return true to let the other side know we're done
			if(frameIndex == 9)
				return true;

			// new frame every 70ms
			if(gameTime.TotalGameTime.TotalMilliseconds - lastTime > 70)
			{
				frameIndex++;
				lastTime = gameTime.TotalGameTime.TotalMilliseconds;
			}

			return false;
		}
	}
}
