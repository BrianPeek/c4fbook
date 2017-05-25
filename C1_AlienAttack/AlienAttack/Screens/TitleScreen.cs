using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace AlienAttack
{
	public class TitleScreen : IScreen
	{
		private Texture2D titleScreen;
		private Texture2D bgScreen;
		private SpriteFont arialFont;

		public TitleScreen(ContentManager contentManager)
		{
			titleScreen = contentManager.Load<Texture2D>("gfx\\titleScreen");
			bgScreen = contentManager.Load<Texture2D>("gfx\\bgScreen");
			arialFont = contentManager.Load<SpriteFont>("Arial");
		}

		public GameState Update(GameTime gameTime)
		{
			if(InputManager.ControlState.Start)
				return GameState.GameScreen;
			return GameState.TitleScreen;
		}

		public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(bgScreen, Vector2.Zero, Color.White);
			spriteBatch.Draw(titleScreen, Vector2.Zero, Color.White);
#if WINDOWS
			spriteBatch.DrawString(arialFont, "Press Enter or Start to Play", new Vector2(600, 680), Color.White);
#endif

#if XBOX
			spriteBatch.DrawString(arialFont, "Press Start to Play", new Vector2(600, 680), Color.White);
#endif

#if ZUNE
			spriteBatch.DrawString(arialFont, "Press Play to Play", new Vector2(80, 290), Color.White);
#endif
		}
	}
}
