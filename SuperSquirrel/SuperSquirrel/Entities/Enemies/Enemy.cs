using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperSquirrel.Common;
using SuperSquirrel.Entities.Events;

namespace SuperSquirrel.Entities.Enemies
{
	public enum EnemyTypes
	{
		PLANET_WALKER,
		TARGET_FINDER
	}

	abstract class Enemy
	{
		private static Dictionary<EnemyTypes, string> nameMap;

		static Enemy()
		{
			nameMap = new Dictionary<EnemyTypes, string>();
			nameMap.Add(EnemyTypes.PLANET_WALKER, "PlanetWalker");
			nameMap.Add(EnemyTypes.TARGET_FINDER, "TargetFinder");
		}

		private int health;
		private int points;

		public Enemy(EnemyTypes type, Vector2 position, int health, int points, int circleRadius)
		{
			this.health = health;
			this.points = points;

			Sprite = new Sprite(ContentLoader.LoadTexture("Enemies/" + nameMap[type]), position, OriginLocations.CENTER);
			Position = position;
			BoundingCircle = new BoundingCircle(position, circleRadius);
		}

		protected Sprite Sprite { get; set; }
		protected Vector2 Position { get; set; }
		protected Vector2 Velocity { get; set; }

		public BoundingCircle BoundingCircle { get; private set; }

		public bool Destroy { get; private set; }

		protected abstract void AI(float dt);

		public void ApplyDamage(int damage)
		{
			health -= damage;

			if (health <= 0)
			{
				Destroy = true;

				SimpleEvent.Queue.Enqueue(new SimpleEvent(EventTypes.POINTS, points));
			}
		}

		public virtual void Update(float dt)
		{
			AI(dt);

			Position += Velocity * dt;
			Sprite.Position = Position;
			BoundingCircle.Center = Position;
		}

		public virtual void Draw(SpriteBatch sb)
		{
			Sprite.Draw(sb);
		}
	}
}
