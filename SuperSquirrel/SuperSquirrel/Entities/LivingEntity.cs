using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperSquirrel.Entities.Events;

namespace SuperSquirrel.Entities
{
	abstract class LivingEntity
	{
		protected LivingEntity(Vector2 position, int circleRadius, int health)
		{
			Position = position;
			BoundingCircle = new BoundingCircle(Position, circleRadius);
			Health = health;

			SimpleEvent.Queue.Enqueue(new SimpleEvent(EventTypes.LIVING_ENTITY, new LivingEntityEventData(this, ActionTypes.ADD)));
		}

		public Vector2 Position { get; set; }
		public Vector2 Velocity { get; set; }

		public BoundingCircle BoundingCircle { get; private set; }

		protected int Health { get; private set; }

		public void ApplyDamage(int damage)
		{
			OnDamage(damage);

			if (Health <= 0)
			{
				SimpleEvent.Queue.Enqueue(new SimpleEvent(EventTypes.LIVING_ENTITY, new LivingEntityEventData(this, ActionTypes.REMOVE)));

				OnDeath();
			}
		}

		protected virtual void OnDamage(int damage)
		{
			Health -= damage;
		}

		protected abstract void OnDeath();

		public virtual void Update(float dt)
		{
			Position += Velocity * dt;
			BoundingCircle.Center = Position;
		}

		public abstract void Draw(SpriteBatch sb);
	}
}
