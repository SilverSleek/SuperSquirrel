using Microsoft.Xna.Framework;

namespace SuperSquirrel.Entities.Planets
{
	class ProximityData
	{
		public ProximityData(Planet planet, float angle, float distance, Vector2 direction)
		{
			Angle = angle;
			Planet = planet;
			Distance = distance;
			Direction = direction;
		}

		public Planet Planet { get; private set; }

		public float Angle { get; private set; }
		public float Distance { get; private set; }

		public Vector2 Direction { get; private set; }
	}
}
