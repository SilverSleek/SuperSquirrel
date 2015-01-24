using Microsoft.Xna.Framework;

namespace SuperSquirrel.Entities
{
	class Mass
	{
		private float mass;

		public Mass(float mass, Vector2 position, Vector2 velocity)
		{
			this.mass = mass;

			Position = position;
			Velocity = velocity;
		}

		public Vector2 Position { get; set; }
		public Vector2 Velocity { get; private set; }

		public bool Fixed { get; set; }

		public void ApplyForce(Vector2 force)
		{
			Velocity += force / mass;
		}
	}
}
