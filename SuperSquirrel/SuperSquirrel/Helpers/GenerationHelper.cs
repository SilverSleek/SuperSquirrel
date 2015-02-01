using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using SuperSquirrel.Common;
using SuperSquirrel.Entities;
using SuperSquirrel.Entities.Enemies;
using SuperSquirrel.Entities.Planets;
using SuperSquirrel.Interfaces;

namespace SuperSquirrel.Helpers
{
	class GenerationHelper : ISimpleUpdateable
	{
		private const int MIN_STARTING_PLANETS = 3;
		private const int MAX_STARTING_PLANETS = 5;
		private const int MIN_PLANET_SEPARATION = 200;
		private const int MIN_PLANET_GENERATION_RANGE = 300;
		private const int MAX_PLANET_GENERATION_RANGE = 600;

		private List<Planet> planets;
		private List<Enemy> enemies;

		private Random random;

		public GenerationHelper(List<Planet> planets, List<Enemy> enemies)
		{
			this.planets = planets;
			this.enemies = enemies;

			random = new Random();

			GenerateStartingPlanets();
		}

		private void GenerateStartingPlanets()
		{
			Rectangle visibleArea = Camera.Instance.VisibleArea;

			int numStartingPlanets = random.Next(MIN_STARTING_PLANETS, MAX_STARTING_PLANETS + 1);

			for (int i = 0; i < numStartingPlanets; i++)
			{
				PlanetSizes size = (PlanetSizes)random.Next(3);
				Vector2 position = Vector2.Zero;

				while (!CheckValidPlanetPosition(position))
				{
					float x = visibleArea.X + (float)random.NextDouble() * visibleArea.Width;
					float y = visibleArea.Y + (float)random.NextDouble() * visibleArea.Height;

					position = new Vector2(x, y);
				}

				planets.Add(new Planet(position, size));
			}
		}

		private bool CheckValidPlanetPosition(Vector2 position)
		{
			foreach (Planet planet in planets)
			{
				if (planet.BoundingCircle.ContainsPoint(position))
				{
					return false;
				}
			}

			foreach (Planet planet in planets)
			{
				Vector2 direction = Vector2.Normalize(position - planet.Center);
				Vector2 surfacePosition = planet.Center + direction * planet.Radius;

				if (Vector2.Distance(position, surfacePosition) < MIN_PLANET_SEPARATION)
				{
					return false;
				}
			}

			return true;
		}

		public void Update(float dt)
		{
		}
	}
}
