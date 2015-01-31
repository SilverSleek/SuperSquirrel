﻿using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using SuperSquirrel.Common;
using SuperSquirrel.Entities;
using SuperSquirrel.Entities.Planets;

namespace SuperSquirrel.Helpers
{
	class PlanetHelper
	{
		private List<Planet> planets;

		public PlanetHelper(List<Planet> planets)
		{
			this.planets = planets;
		}

		public Planet CheckCollision(BoundingCircle boundingCircle)
		{
			foreach (Planet planet in planets)
			{
				if (planet.BoundingCircle.Intersects(boundingCircle))
				{
					return planet;
				}
			}

			return null;
		}

		public ProximityData CheckCollision(Vector2 point)
		{
			foreach (Planet planet in planets)
			{
				if (planet.BoundingCircle.ContainsPoint(point))
				{
					Vector2 center = planet.BoundingCircle.Center;
					Vector2 direction = Vector2.Normalize(point - center);

					float distance = Vector2.Distance(center, point);

					return new ProximityData(planet, distance, direction);
				}
			}

			return null;
		}

		public Vector2 CalculateGravity(Vector2 position, float mass)
		{
			const int GRAVITY_FACTOR = 5;

			Vector2 gravity = Vector2.Zero;

			foreach (Planet planet in planets)
			{
				// taken from http://en.wikipedia.org/wiki/Newton%27s_law_of_universal_gravitation
				float distance = Vector2.Distance(position, planet.Center);
				float force = (planet.Mass * mass) / (distance * distance);

				Vector2 direction = Vector2.Normalize(planet.Center - position);

				gravity += force * direction;
			}

			return gravity * GRAVITY_FACTOR;
		}
	}
}
