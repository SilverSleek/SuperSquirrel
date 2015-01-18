using Microsoft.Xna.Framework;

namespace SuperSquirrel.Entities.Planets
{
	class ProximityData
	{
		public ProximityData(Planet planet, float angle, float distance, float surfaceDistance, Vector2 direction)
		{
			Planet = planet;
			Angle = angle;
			Distance = distance;
			SurfaceDistance = surfaceDistance;
			Direction = direction;
		}

		public Planet Planet { get; private set; }

		public float Angle { get; private set; }
		public float Distance { get; private set; }
		public float SurfaceDistance { get; private set; }

		public Vector2 Direction { get; private set; }
	}
}
