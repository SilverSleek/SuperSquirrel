using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperSquirrel.Entities.Planets;

namespace SuperSquirrel.Wrappers
{
	class PlanetWrapper
	{
		private List<Planet> planets;

		public PlanetWrapper(List<Planet> planets)
		{
			this.planets = planets;

			planets.Add(new Planet(new Vector2(Common.Constants.SCREEN_WIDTH, Common.Constants.SCREEN_HEIGHT) / 2, PlanetSizes.SMALL));
			planets.Add(new Planet(new Vector2(750, 250), PlanetSizes.SMALL));
			planets.Add(new Planet(new Vector2(300, 150), PlanetSizes.MEDIUM));
			planets.Add(new Planet(new Vector2(800, 575), PlanetSizes.LARGE));
			planets.Add(new Planet(new Vector2(200, 400), PlanetSizes.MEDIUM));
			planets.Add(new Planet(new Vector2(100, 600), PlanetSizes.SMALL));
			planets.Add(new Planet(new Vector2(300, -75), PlanetSizes.SMALL)); 
		}

		public void Update(float dt)
		{
			for (int i = 0; i < planets.Count; i++)
			{
				planets[i].Update(dt);
			}
		}

		public void Draw(SpriteBatch sb)
		{
			foreach (Planet Planet in planets)
			{
				Planet.Draw(sb);
			}
		}
	}
}
