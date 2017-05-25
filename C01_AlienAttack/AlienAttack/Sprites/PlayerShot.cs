using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AlienAttack
{
	public class PlayerShot : Sprite
	{
		// all frames of animation
		private double lastTime;

		public PlayerShot(ContentManager contentManager) : base(contentManager)
		{
			spriteTextures = new Texture2D[2];

			// load the frames
			for(int i = 0; i <= 1; i++)
				spriteTextures[i] = contentManager.Load<Texture2D>("gfx\\pshot\\pshot_" + i);
		}

		public override void Update(GameTime gameTime)
		{
			// draw a new frame very 200ms...seems to be a good value
			if(gameTime.TotalGameTime.TotalMilliseconds - lastTime > 200)
			{
				// toggle between frames
				frameIndex = frameIndex == 0 ? 1 : 0;
				lastTime = gameTime.TotalGameTime.TotalMilliseconds;
			}

			this.Position.Y -= 5;
		}
	}
}
