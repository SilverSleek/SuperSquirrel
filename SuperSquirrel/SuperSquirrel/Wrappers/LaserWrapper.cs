using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperSquirrel.Entities;
using SuperSquirrel.Entities.Events;
using SuperSquirrel.Interfaces;

namespace SuperSquirrel.Wrappers
{
	class LaserWrapper : ISimpleEventListener
	{
		public LaserWrapper(List<Laser> lasers)
		{
			Lasers = lasers;

			SimpleEvent.Queue.Enqueue(new SimpleEvent(EventTypes.LISTENER, new ListenerEventData(EventTypes.LASER, this)));
		}

		public List<Laser> Lasers { get; private set; }

		public void EventResponse(SimpleEvent simpleEvent)
		{
			LaserEventData data = (LaserEventData)simpleEvent.Data;

			Lasers.Add(new Laser(data.Position, data.Velocity, data.Rotation, data.Owner));
		}

		public void Update(float dt)
		{
			for (int i = 0; i < Lasers.Count; i++)
			{
				Laser laser = Lasers[i];

				if (laser.Destroy)
				{
					Lasers.RemoveAt(i);
				}
				else
				{
					laser.Update(dt);
				}
			}
		}

		public void Draw(SpriteBatch sb)
		{
			foreach (Laser laser in Lasers)
			{
				laser.Draw(sb);
			}
		}
	}
}
