using System.Collections.Generic;

using SuperSquirrel.Entities;
using SuperSquirrel.Entities.Events;
using SuperSquirrel.Entities.Planets;
using SuperSquirrel.Interfaces;

namespace SuperSquirrel.Helpers
{
	class LaserHelper : ISimpleEventListener
	{
		private List<Laser> lasers;
		private List<Planet> planets;
		private List<LivingEntity> entities;

		public LaserHelper(List<Laser> lasers, List<Planet> planets)
		{
			this.lasers = lasers;
			this.planets = planets;

			entities = new List<LivingEntity>();

			SimpleEvent.Queue.Enqueue(new SimpleEvent(EventTypes.LISTENER, new ListenerEventData(EventTypes.LIVING_ENTITY, this)));
		}

		public void EventResponse(SimpleEvent simpleEvent)
		{
			LivingEntityEventData data = (LivingEntityEventData)simpleEvent.Data;

			if (data.Action == ActionTypes.ADD)
			{
				entities.Add(data.Entity);
			}
			else
			{
				entities.Remove(data.Entity);
			}
		}

		public void Update()
		{
			const int LASER_DAMAGE = 1;

			foreach (Laser laser in lasers)
			{
				foreach (Planet planet in planets)
				{
					if (planet.BoundingCircle.ContainsPoint(laser.Tip))
					{
						laser.Destroy = true;

						break;
					}
				}

				if (!laser.Destroy)
				{
					foreach (LivingEntity entity in entities)
					{
						if (laser.Owner != entity && entity.BoundingCircle.ContainsPoint(laser.Tip))
						{
							laser.Destroy = true;
							entity.ApplyDamage(LASER_DAMAGE);

							break;
						}
					}
				}
			}
		}
	}
}
