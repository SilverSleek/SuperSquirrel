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

			planets.Add(new Planet(new Vector2(300, -400), PlanetSizes.MEDIUM));
			planets.Add(new Planet(new Vector2(100, 600), PlanetSizes.LARGE));
			planets.Add(new Planet(new Vector2(-400, 50), PlanetSizes.SMALL));
			planets.Add(new Planet(new Vector2(-300, -300), PlanetSizes.MEDIUM));
			planets.Add(new Planet(new Vector2(500, -50), PlanetSizes.SMALL));
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
