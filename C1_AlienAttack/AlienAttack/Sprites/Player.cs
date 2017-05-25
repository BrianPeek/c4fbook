using Microsoft.Xna.Framework.Content;

namespace AlienAttack
{
	public class Player : Sprite
	{
		public Player(ContentManager contentManager) : base(contentManager, "gfx\\player")
		{
			this.Position.X = AlienAttackGame.ScreenWidth/2 - this.Width/2;
#if ZUNE
			this.Position.Y = AlienAttackGame.ScreenHeight - 40;
#else
			this.Position.Y = AlienAttackGame.ScreenHeight - 100;
#endif
		}
	}
}
