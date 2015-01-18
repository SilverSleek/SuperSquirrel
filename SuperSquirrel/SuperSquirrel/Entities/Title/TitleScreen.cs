using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperSquirrel.Common;

namespace SuperSquirrel.Entities.Title
{
	class TitleScreen
	{
		private MenuPlanet[] menuPlanets;

		public TitleScreen()
		{
			Vector2[] positions = new Vector2[4];
			positions[0] = new Vector2(475, 200);
			positions[1] = new Vector2(650, 300);
			positions[2] = new Vector2(225, 350);
			positions[3] = new Vector2(875, 575);

			menuPlanets = new MenuPlanet[4];
			menuPlanets[0] = new MenuPlanet("New Game", positions[0], Planets.PlanetSizes.LARGE);
			menuPlanets[1] = new MenuPlanet("Continue", positions[1], Planets.PlanetSizes.MEDIUM);
			menuPlanets[2] = new MenuPlanet("Options", positions[2], Planets.PlanetSizes.MEDIUM);
			menuPlanets[3] = new MenuPlanet("Exit", positions[3], Planets.PlanetSizes.SMALL);
		}

		public void Update(float dt)
		{
			foreach (MenuPlanet menuPlanet in menuPlanets)
			{
				menuPlanet.Update(dt);
			}
		}

		public void Draw(SpriteBatch sb)
		{
			foreach (MenuPlanet menuPlanet in menuPlanets)
			{
				menuPlanet.Draw(sb);
			}
		}
	}
}
