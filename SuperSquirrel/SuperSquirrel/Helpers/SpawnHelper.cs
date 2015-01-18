using System.Collections.Generic;

using SuperSquirrel.Entities.Enemies;
using SuperSquirrel.Entities.Planets;

namespace SuperSquirrel.Helpers
{
	class SpawnHelper
	{
		private List<Planet> planets;
		private List<Enemy> enemies;

		public SpawnHelper(List<Planet> planets, List<Enemy> enemies)
		{
			this.planets = planets;
			this.enemies = enemies;
		}

		public void Update()
		{
		}
	}
}
