using Microsoft.Xna.Framework;

namespace SuperSquirrel.Entities.RopePhysics
{
	class Mass
	{
		public Mass(float mass, Vector2 position, Vector2 velocity)
		{
			MassValue = mass;
			Position = position;
			Velocity = velocity;
		}

		public float MassValue { get; private set; }

		public Vector2 Position { get; set; }
		public Vector2 Velocity { get; protected set; }

		public bool Fixed { get; set; }

		public void ApplyForce(Vector2 force)
		{
			Velocity += force / MassValue;
		}
	}
}
