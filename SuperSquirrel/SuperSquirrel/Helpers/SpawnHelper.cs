using System.Collections.Generic;

using SuperSquirrel.Entities.Enemies;
using SuperSquirrel.Entities.Planets;
using SuperSquirrel.Interfaces;

namespace SuperSquirrel.Helpers
{
	class SpawnHelper : ISimpleUpdateable
	{
		private List<Planet> planets;
		private List<Enemy> enemies;

		public SpawnHelper(List<Planet> planets, List<Enemy> enemies)
		{
			this.planets = planets;
			this.enemies = enemies;
		}

		public void Update(float dt)
		{
		}
	}
}
