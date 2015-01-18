using Microsoft.Xna.Framework;

namespace SuperSquirrel.Entities.Events
{
	class LaserEventData
	{
		public LaserEventData(Vector2 position, Vector2 velocity, float rotation, LivingEntity owner)
		{
			Position = position;
			Velocity = velocity;
			Rotation = rotation;
			Owner = owner;
		}

		public Vector2 Position { get; private set; }
		public Vector2 Velocity { get; private set; }

		public float Rotation { get; private set; }

		public LivingEntity Owner { get; private set; }
	}
}
