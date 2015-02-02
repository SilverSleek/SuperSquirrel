using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperSquirrel.Entities;
using SuperSquirrel.Entities.Enemies;
using SuperSquirrel.Entities.Planets;

namespace SuperSquirrel.Wrappers
{
	class EnemyWrapper : Wrapper
	{
		public EnemyWrapper(List<Enemy> enemies, List<Planet> planets)
		{
			Enemies = enemies;

			AddTestEnemies(planets);
		}

		private void AddTestEnemies(List<Planet> planets)
		{
			Enemies.Add(new TargetFinder(new Vector2(200, -200)));
			Enemies.Add(new TargetFinder(new Vector2(-100, 300)));
			Enemies.Add(new TargetFinder(new Vector2(-300, -100)));
			Enemies.Add(new TargetFinder(new Vector2(450, 250)));
			Enemies.Add(new TargetFinder(new Vector2(450, 250)));

			Enemies.Add(new PlanetWalker(planets[1]));
			Enemies.Add(new PlanetWalker(planets[3]));
			Enemies.Add(new PlanetWalker(planets[2]));
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
