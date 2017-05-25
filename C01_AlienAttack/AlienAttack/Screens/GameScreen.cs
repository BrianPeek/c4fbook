using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace AlienAttack
{
	public class GameScreen : IScreen
	{
		private ContentManager contentManager;

		private Player player;
		private Player livesIcon;
		private Explosion playerExplosion;
		private Texture2D bgScreen;
		private EnemyGroup enemyGroup;
		private SpriteFont arial;

		private List<PlayerShot> playerShots;

		private int score;
		private int lives;
		private double lastTime;
		private bool loseGame;
		private float PlayerVelocity = AlienAttackGame.ScreenWidth / 200.0f;

		public GameScreen(ContentManager cm)
		{
			contentManager = cm;

			AlienAttackGame.AudioManager.PlayCue(AudioManager.Cue.Theme);

			bgScreen = contentManager.Load<Texture2D>("gfx\\bgScreen");
			player = new Player(contentManager);

			playerShots = new List<PlayerShot>();

			arial = contentManager.Load<SpriteFont>("arial");

			// draw a lives status icon in the lower left
			livesIcon = new Player(contentManager);
#if ZUNE
			livesIcon.Position = new Vector2(0, AlienAttackGame.ScreenHeight-20);
#else
			livesIcon.Position = new Vector2(40, AlienAttackGame.ScreenHeight-60);
#endif
			enemyGroup = new EnemyGroup(contentManager);

			lives = 2;
		}

		public GameState Update(GameTime gameTime)
		{
			MovePlayer(gameTime);
			UpdatePlayerShots(gameTime);

			// as long as we're not in the lose state, update the enemies
			if(!loseGame)
				enemyGroup.Update(gameTime);

			HandleCollisions(gameTime);

			return GameState.GameScreen;
		}

		private void HandleCollisions(GameTime gameTime)
		{
			// see if a player shot hit an enemy
			for(int i = 0; i < playerShots.Count; i++)
			{
				PlayerShot playerShot = playerShots[i];
				// check the shot and see if it it collided with an enemy
				if(playerShot != null && enemyGroup.HandlePlayerShotCollision(playerShots[i]))
				{
					// remove the shot, add the score
					playerShots.RemoveAt(i);
					score += 100;
					AlienAttackGame.AudioManager.PlayCue(AudioManager.Cue.Explosion);
				}
			}

			// see if an enemy shot hit the player
			if(player != null && enemyGroup.HandleEnemyShotCollision(player))
			{
				// blow up the player
				playerExplosion = new Explosion(this.contentManager);
				playerExplosion.Position = player.Position;
				player = null;
				AlienAttackGame.AudioManager.PlayCue(AudioManager.Cue.Explosion);
			}

			// see if an enemy hit the player directly
			if(player != null && enemyGroup.HandleEnemyPlayerCollision(player))
			{
				// blow up the player
				playerExplosion = new Explosion(this.contentManager);
				playerExplosion.Position = player.Position;
				player = null;
				AlienAttackGame.AudioManager.PlayCue(AudioManager.Cue.Explosion);
				loseGame = true;
			}

			// if the player explosion animation is running, update it
			if(playerExplosion != null)
			{
				// if this is the last frame
				if(playerExplosion.Update(gameTime) && !loseGame)
				{
					// remove it
					playerExplosion = null;

					// we lose if we have no lives left
					if(lives == 0)
						loseGame = true;
					else
					{
						// subract 1 life and reset the board
						lives--;
						enemyGroup.Reset();
						playerShots.Clear();
						player = new Player(this.contentManager);
					}
				}
			}
		}

		private void UpdatePlayerShots(GameTime gameTime)
		{
			// if we are allowed to fire, add a shot to the list
			if(InputManager.ControlState.Fire && gameTime.TotalGameTime.TotalMilliseconds - lastTime > 500)
			{
				// create a new shot over the ship
				PlayerShot ps = new PlayerShot(this.contentManager);
				ps.Position.X = (player.Position.X + player.Width/2) - ps.Width/2;
				ps.Position.Y = player.Position.Y - ps.Height;
				playerShots.Add(ps);
				lastTime = gameTime.TotalGameTime.TotalMilliseconds;
				AlienAttackGame.AudioManager.PlayCue(AudioManager.Cue.PlayerShot);
			}

			// enumerate the player shots on the screen
			for(int i = 0; i < playerShots.Count; i++)
			{
				PlayerShot playerShot = playerShots[i];

				playerShot.Update(gameTime);

				// if it's off the top of the screen, remove it from the list
				if(playerShot.Position.Y + playerShot.Height < 0)
				{
					playerShots.RemoveAt(i);
					playerShot = null;
				}
			}
		}

		private void MovePlayer(GameTime gameTime)
		{
			if(player != null)
			{
				// move left
				if(InputManager.ControlState.Left && player.Position.X > 0)
					player.Position.X -= PlayerVelocity;

				// move right
				if(InputManager.ControlState.Right && player.Position.X + player.Width < AlienAttackGame.ScreenWidth)
					player.Position.X += PlayerVelocity;

				player.Update(gameTime);
			}
		}

		public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(bgScreen, Vector2.Zero, Color.White);

			// draw the player
			if(player != null)
				player.Draw(gameTime, spriteBatch);

			// draw the enemy board
			enemyGroup.Draw(gameTime, spriteBatch);

			// draw the player shots
			foreach(PlayerShot playerShot in playerShots)
				playerShot.Draw(gameTime, spriteBatch);

			// draw the explosion
			if(playerExplosion != null)
				playerExplosion.Draw(gameTime, spriteBatch);

#if ZUNE
			// draw the score
			spriteBatch.DrawString(arial, "Score", new Vector2(0, 0), Color.White);
			spriteBatch.DrawString(arial, score.ToString(), new Vector2(0, 20), Color.White);

			// draw the lives icon
			livesIcon.Draw(gameTime, spriteBatch);
			spriteBatch.DrawString(arial, "x" + lives.ToString(), new Vector2(livesIcon.Position.X + livesIcon.Width + 4, livesIcon.Position.Y), Color.White);
#else
			// draw the score
			spriteBatch.DrawString(arial, "Score", new Vector2(50, 50), Color.White);
			spriteBatch.DrawString(arial, score.ToString(), new Vector2(50, 80), Color.White);

			// draw the lives icon
			livesIcon.Draw(gameTime, spriteBatch);
			spriteBatch.DrawString(arial, "x" + lives.ToString(), new Vector2(livesIcon.Position.X + livesIcon.Width + 4, livesIcon.Position.Y+8), Color.White);
#endif

			// draw the proper text, if required
			if(enemyGroup.AllDestroyed())
			{
				Vector2 size = arial.MeasureString("You win!");
				spriteBatch.DrawString(arial, "You win!", new Vector2((AlienAttackGame.ScreenWidth - size.X) / 2, (AlienAttackGame.ScreenHeight - size.Y) / 2), Color.Green);
			}

			if(loseGame)
			{
				Vector2 size = arial.MeasureString("Game Over");
				spriteBatch.DrawString(arial, "Game Over", new Vector2((AlienAttackGame.ScreenWidth - size.X) / 2, (AlienAttackGame.ScreenHeight - size.Y) / 2), Color.Red);
			}
		}
	}
}
