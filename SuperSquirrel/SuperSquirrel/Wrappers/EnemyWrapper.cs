using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperSquirrel.Entities;
using SuperSquirrel.Entities.Enemies;

namespace SuperSquirrel.Wrappers
{
	class EnemyWrapper
	{
		public EnemyWrapper(Player player, List<Enemy> enemies)
		{
			Enemies = enemies;
			enemies.Add(new TargetFinder(player, new Vector2(600, 150)));
			enemies.Add(new TargetFinder(player, new Vector2(500, 700)));
			enemies.Add(new TargetFinder(player, new Vector2(400, 400)));
		}

		public List<Enemy> Enemies { get; private set; }

		public void Update(float dt)
		{
			for (int i = 0; i < Enemies.Count; i++)
			{
				Enemy enemy = Enemies[i];

				if (enemy.Destroy)
				{
					Enemies.RemoveAt(i);
				}
				else
				{
					enemy.Update(dt);
				}
			}
		}

		public void Draw(SpriteBatch sb)
		{
			foreach (Enemy enemy in Enemies)
			{
				enemy.Draw(sb);
			}
		}
	}
}
