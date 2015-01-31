using Microsoft.Xna.Framework;

namespace SuperSquirrel.Entities.Planets
{
	class ProximityData
	{
		public ProximityData(Planet planet, float distance, Vector2 direction)
		{
			Planet = planet;
			Distance = distance;
			Direction = direction;
		}

		public Planet Planet { get; private set; }

		public float Distance { get; private set; }

		public Vector2 Direction { get; private set; }
	}
}
