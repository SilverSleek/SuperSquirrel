using Microsoft.Xna.Framework;

namespace SuperSquirrel.Entities
{
	class BoundingCircle
	{
		public BoundingCircle(Vector2 center, int radius)
		{
			Center = center;
			Radius = radius;
		}

		public Vector2 Center { get; set; }

		public int Radius { get; private set; }

		public bool Intersects(BoundingCircle otherCircle)
		{
			return Vector2.Distance(Center, otherCircle.Center) <= Radius + otherCircle.Radius;
		}

		public bool ContainsPoint(Vector2 point)
		{
			return Vector2.DistanceSquared(point, Center) <= Radius * Radius;
		}
	}
}
