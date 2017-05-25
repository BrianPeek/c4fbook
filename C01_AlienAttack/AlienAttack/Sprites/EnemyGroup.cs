using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AlienAttack
{
	public class EnemyGroup : Sprite
	{
		// grid of enemies
		private Enemy[,] enemies;

		// all enemy shots
		private List<EnemyShot> enemyShots;

		// all enemy explosions
		private List<Explosion> explosions;
		private Random random;

		// width of single enemy
		private int enemyWidth;

#if ZUNE
		private const int EnemyRows = 4;	// number of rows in grid
		private const int EnemyCols = 6;	// number of cols in grid
		private const int EnemyVerticalJump = 3;	// number of pixels to jump vertically after hitting edge
		private const int EnemyStartPosition = 10;	// vertical position of grid
		private const int ScreenEdge = 3;	// virtual edge of screen to change direction
		private Vector2 EnemySpacing = new Vector2(2, 2);	// space between sprites
		private const float EnemyVelocity = 0.5f;	// speed at which grid moves per frame

#else
		private const int EnemyRows = 4;	// number of rows in grid
		private const int EnemyCols = 8;	// number of cols in grid
		private const int EnemyVerticalJump = 10;	// number of pixels to jump vertically after hitting edge
		private const int EnemyStartPosition = 100;	// vertical position of grid
		private const int ScreenEdge = 20;	// virtual edge of screen to change direction
		private Vector2 EnemySpacing = new Vector2(4, 4);	// space between sprites
		private const float EnemyVelocity = 1.5f;	// speed at which grid moves per frame
#endif

		public EnemyGroup(ContentManager contentManager) : base(contentManager)
		{
			random = new Random();

			enemyShots = new List<EnemyShot>();
			explosions = new List<Explosion>();

			enemies = new Enemy[EnemyRows,EnemyCols];

			// create a grid of enemies
			for(int y = 0; y < EnemyRows; y++)
			{
				for(int x = 0; x < EnemyCols; x++)
				{
					Enemy enemy = new Enemy(contentManager);
					enemy.Position.X = x * enemy.Width + EnemySpacing.X;
					enemy.Position.Y = y * enemy.Height + EnemySpacing.Y;
					enemies[y,x] = enemy;
				}
			}

			enemyWidth = enemies[0,0].Width;

			// position the grid centered at the vertical position specified above
			this.Position.X = AlienAttackGame.ScreenWidth/2 - ((EnemyCols * (enemyWidth + EnemySpacing.X)) / 2);
			this.Position.Y = EnemyStartPosition;
			this.Velocity.X = EnemyVelocity;
		}

		public override void Update(GameTime gameTime)
		{
			MoveEnemies(gameTime);
			EnemyFire(gameTime);

			for(int i = 0; i < explosions.Count; i++)
			{
				// update all explosions, remove those whose animations are over
				if(explosions[i].Update(gameTime))
					explosions.RemoveAt(i);
			}
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			// draw all active enemies
			foreach(Enemy enemy in enemies)
			{
				if(enemy != null)
					enemy.Draw(gameTime, spriteBatch);
			}

			// draw all enemy shots
			foreach(EnemyShot enemyShot in enemyShots)
				enemyShot.Draw(gameTime, spriteBatch);

			// draw all explosions
			foreach(Explosion explosion in explosions)
				explosion.Draw(gameTime, spriteBatch);
		}

		public void Reset()
		{
			enemyShots.Clear();
		}

		private Enemy FindRightMostEnemy()
		{
			// find the enemy in the right-most position in the grid
			for(int x = EnemyCols-1; x > -1; x--)
			{
				for(int y = 0; y < EnemyRows; y++)
				{
					if(enemies[y,x] != null)
						return enemies[y,x];
				}
			}
			return null;
		}

		private Enemy FindLeftMostEnemy()
		{
			// find the enemy in the left-most position in the grid
			for(int x = 0; x < EnemyCols; x++)
			{
				for(int y = 0; y < EnemyRows; y++)
				{
					if(enemies[y,x] != null)
						return enemies[y,x];
				}
			}
			return null;
		}

		private void MoveEnemies(GameTime gameTime)
		{
			Enemy enemy = FindRightMostEnemy();

			// if the right-most enemy hit the screen edge, change directions
			if(enemy != null)
			{
				if(enemy.Position.X + enemy.Width > AlienAttackGame.ScreenWidth - ScreenEdge)
				{
					this.Position.Y += EnemyVerticalJump;
					this.Velocity.X = -EnemyVelocity;	// move left
				}
			}

			enemy = FindLeftMostEnemy();

			// if the left-most enemy hit the screen edge, change direction
			if(enemy != null)
			{
				if(enemy.Position.X < ScreenEdge)
				{
					this.Position.Y += EnemyVerticalJump;
					this.Velocity.X = EnemyVelocity;	// move right
				}
			}

			// update the positions of all enemies
			for(int y = 0; y < EnemyRows; y++)
			{
				for(int x = 0; x < EnemyCols; x++)
				{
					if(enemies[y,x] != null)
					{
						// X = position of the whole grid + (X grid position * width of enemy) + padding
						// Y = position of the whole grid + (Y grid position * width of enemy) + padding
						enemies[y,x].Position.X = (this.Position.X + (x * (enemyWidth + EnemySpacing.X)));
						enemies[y,x].Position.Y = (this.Position.Y + (y * (enemyWidth + EnemySpacing.X)));
						enemies[y,x].Update(gameTime);
					}
				}
			}

			this.Position += this.Velocity;
		}

		public bool AllDestroyed()
		{
			// we won if we can't find any enemies at all
			return (FindLeftMostEnemy() == null);
		}

		public bool HandlePlayerShotCollision(PlayerShot playerShot)
		{
			for(int y = 0; y < EnemyRows; y++)
			{
				for(int x = 0; x < EnemyCols; x++)
				{
					// if a player shot hit an enemy, destroy the enemy
					if(enemies[y,x] != null && CheckCollision(playerShot, enemies[y,x]))
					{
						Explosion explosion = new Explosion(this.Content);
						explosion.Position = enemies[y,x].Position;
						explosions.Add(explosion);
						enemies[y,x] = null;
						return true;
					}
				}
			}
			return false;
		}

		private void EnemyFire(GameTime gameTime)
		{
			// at random times, drop an enemy shot
			if(random.NextDouble() > 0.99f)
			{
				int x, y;

				// find an enemy that hasn't been destroyed
				do
				{
					x = (int)(random.NextDouble() * EnemyCols);
					y = (int)(random.NextDouble() * EnemyRows);
				}
				while(enemies[y,x] == null);

				// create a shot for that enemy and add it to the list
				EnemyShot enemyShot = new EnemyShot(this.Content);
				enemyShot.Position = enemies[y,x].Position;
				enemyShot.Position.Y += enemies[y,x].Height;
				enemyShots.Add(enemyShot);

				AlienAttackGame.AudioManager.PlayCue(AudioManager.Cue.EnemyShot);
			}

			for(int i = 0; i < enemyShots.Count; i++)
			{
				// update all shots
				enemyShots[i].Update(gameTime);

				// remove those that are off the screen
				if(enemyShots[i].Position.Y > AlienAttackGame.ScreenHeight)
					enemyShots.RemoveAt(i);
			}
		}

		public bool HandleEnemyShotCollision(Player player)
		{
			for(int i = 0; i < enemyShots.Count; i++)
			{
				// if an enemy shot hit the player, destroy the player
				if(CheckCollision(enemyShots[i], player))
				{
					enemyShots.RemoveAt(i);
					return true;
				}
			}
			return false;
		}

		public bool HandleEnemyPlayerCollision(Player player)
		{
			for(int y = 0; y < EnemyRows; y++)
			{
				for(int x = 0; x < EnemyCols; x++)
				{
					// if an enemy hit the player, destroy the enemy
					if(enemies[y,x] != null && CheckCollision(enemies[y,x], player))
					{
						Explosion explosion = new Explosion(this.Content);
						explosion.Position = enemies[y,x].Position;
						explosions.Add(explosion);
						enemies[y,x] = null;
						return true;
					}
				}
			}
			return false;
		}

		public bool CheckCollision(Sprite s1, Sprite s2)
		{
			// simple bounding box collision detection
			return s1.BoundingBox.Intersects(s2.BoundingBox);
		}
	}
}
