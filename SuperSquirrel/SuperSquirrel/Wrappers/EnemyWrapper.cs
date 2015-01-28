using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperSquirrel.Entities;
using SuperSquirrel.Entities.Enemies;

namespace SuperSquirrel.Wrappers
{
	class EnemyWrapper : Wrapper
	{
		public EnemyWrapper(Player player, List<Enemy> enemies)
		{
			Enemies = enemies;
		}

		public List<Enemy> Enemies { get; private set; }

		public override void Update(float dt)
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

		public override void Draw(SpriteBatch sb)
		{
			foreach (Enemy enemy in Enemies)
			{
				enemy.Draw(sb);
			}
		}
	}
}
