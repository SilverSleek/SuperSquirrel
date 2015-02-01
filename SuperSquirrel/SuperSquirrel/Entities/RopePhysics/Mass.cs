using Microsoft.Xna.Framework;

using SuperSquirrel.Common;

namespace SuperSquirrel.Entities.RopePhysics
{
	class Mass
	{
		public const int DEFAULT_MASS = 1;

		public Mass(float mass, Vector2 position, Vector2 velocity)
		{
			MassValue = mass;
			Position = position;
			Velocity = velocity;
		}

		public float MassValue { get; set; }

		public Vector2 Position { get; set; }
		public Vector2 Velocity { get; set; }

		public bool Fixed { get; set; }
		public bool Asleep { get; set; }

		public void ApplyForce(Vector2 force)
		{
			Velocity += force / MassValue / Constants.TIME_FACTOR;
		}
	}
}
