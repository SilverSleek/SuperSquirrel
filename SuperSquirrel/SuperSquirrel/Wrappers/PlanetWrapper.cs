using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperSquirrel.Entities.Planets;

namespace SuperSquirrel.Wrappers
{
	class PlanetWrapper : Wrapper
	{
		private List<Planet> planets;

		public PlanetWrapper(List<Planet> planets)
		{
			this.planets = planets;
		}

		public override void Update(float dt)
		{
			for (int i = 0; i < planets.Count; i++)
			{
				planets[i].Update(dt);
			}
		}

		public override void Draw(SpriteBatch sb)
		{
			foreach (Planet Planet in planets)
			{
				Planet.Draw(sb);
			}
		}
	}
}
