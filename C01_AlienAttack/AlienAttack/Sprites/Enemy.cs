using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AlienAttack
{
	public class Enemy : Sprite
	{
		// which direction are we moving through the animation set?
		private int direction = 1;

		double lastTime;

		public Enemy(ContentManager contentManager)
		{
			spriteTextures = new Texture2D[10];

			// load the spriteTextures
			for(int i = 0; i <= 9; i++)
				spriteTextures[i] = contentManager.Load<Texture2D>("gfx\\enemy1\\enemy1_" + i);

			this.Velocity.X = 1;
		}

		public override void Update(GameTime gameTime)
		{
			// if we're at the end of the animation, reverse direction
			if(frameIndex == 9)
				direction = -1;

			// if we're at the start of the animation, reverse direction
			if(frameIndex == 0)
				direction = 1;

			// every 70ms, move to the next frame
			if(gameTime.TotalGameTime.TotalMilliseconds - lastTime > 70)
			{
				frameIndex += direction;
				lastTime = gameTime.TotalGameTime.TotalMilliseconds;
			}
		}
	}
}
