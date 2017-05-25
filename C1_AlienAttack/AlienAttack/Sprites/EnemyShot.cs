using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AlienAttack
{
	public class EnemyShot : Sprite
	{
		double lastTime;

		public EnemyShot(ContentManager contentManager) : base(contentManager)
		{
			spriteTextures = new Texture2D[2];

			for(int i = 0; i <= 1; i++)
				spriteTextures[i] = contentManager.Load<Texture2D>("gfx\\eshot\\eshot_" + i);

			this.Velocity.Y = 3;
		}

		public override void Update(GameTime gameTime)
		{
			if(gameTime.TotalGameTime.TotalMilliseconds - lastTime > 200)
			{
				frameIndex = frameIndex == 0 ? 1 : 0;
				lastTime = gameTime.TotalGameTime.TotalMilliseconds;
			}

			this.Position += this.Velocity;
		}
	}
}
