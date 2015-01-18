using System.Collections.Generic;

using SuperSquirrel.Entities;
using SuperSquirrel.Entities.Enemies;

namespace SuperSquirrel.Helpers
{
	class DamageHelper
	{
		private List<Laser> lasers;
		private List<Enemy> enemies;

		public DamageHelper(List<Laser> lasers, List<Enemy> enemies)
		{
			this.lasers = lasers;
			this.enemies = enemies;
		}

		public void Update()
		{
			const int LASER_DAMAGE = 1;

			for (int i = 0; i < lasers.Count; i++)
			{
				for (int j = 0; j < enemies.Count; j++)
				{
					if (enemies[j].BoundingCircle.ContainsPoint(lasers[i].Tip))
					{
						lasers[i].Destroy = true;
						enemies[j].ApplyDamage(LASER_DAMAGE);
					}
				}
			}
		}
	}
}
