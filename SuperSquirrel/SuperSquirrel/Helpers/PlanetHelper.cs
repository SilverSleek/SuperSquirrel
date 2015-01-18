using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using SuperSquirrel.Common;
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

		public List<ProximityData> GetProximityData(Vector2 playerPosition)
		{
			List<ProximityData> dataList = new List<ProximityData>();

			foreach (Planet planet in planets)
			{
				Vector2 center = planet.Center;
				Vector2 direction = Vector2.Normalize(center - playerPosition);

				float angle = Functions.ComputeAngle(playerPosition, center);
				float distance = Vector2.Distance(playerPosition, center);
				float surfaceDistance = distance - planet.Radius;

				dataList.Add(new ProximityData(planet, angle, distance, surfaceDistance, direction));
			}

			return dataList;
		}

		public Planet GetNearestPlanet(List<ProximityData> dataList)
		{
			List<ProximityData> nearestList = new List<ProximityData>();

			foreach (ProximityData data in dataList)
			{
				if (data.SurfaceDistance < Planet.CAMERA_LERP_RANGE)
				{
					nearestList.Add(data);
				}
			}

			nearestList.Sort(new Comparison<ProximityData>(CompareData));

			return nearestList[0].Planet;
		}

		private int CompareData(ProximityData data1, ProximityData data2)
		{
			if (data1.SurfaceDistance < data2.SurfaceDistance)
			{
				return -1;
			}

			return data2.SurfaceDistance < data1.SurfaceDistance ? 1 : 0;
		}

		public Vector2 CalculateGravity(Vector2 playerPosition, int playerMass, List<ProximityData> dataList)
		{
			const int GRAVITY_FACTOR = 45;

			Vector2 gravity = Vector2.Zero;

			foreach (ProximityData data in dataList)
			{
				// taken from http://en.wikipedia.org/wiki/Newton%27s_law_of_universal_gravitation
				float force = (data.Planet.Mass * playerMass) / (data.Distance * data.Distance);

				gravity += force * data.Direction;
			}

			return gravity * GRAVITY_FACTOR;
		}
	}
}
