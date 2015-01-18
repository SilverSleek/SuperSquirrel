using System.Collections.Generic;

using SuperSquirrel.Entities;
using SuperSquirrel.Entities.Events;
using SuperSquirrel.Interfaces;

namespace SuperSquirrel.Helpers
{
	class DamageHelper : ISimpleEventListener
	{
		private List<Laser> lasers;
		private List<LivingEntity> entities;

		public DamageHelper(List<Laser> lasers)
		{
			this.lasers = lasers;

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
				foreach (LivingEntity entity in entities)
				{
					if (laser.Owner != entity && entity.BoundingCircle.ContainsPoint(laser.Tip))
					{
						laser.Destroy = true;
						entity.ApplyDamage(LASER_DAMAGE);
					}
				}
			}
		}
	}
}
