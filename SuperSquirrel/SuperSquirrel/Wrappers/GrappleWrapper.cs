using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperSquirrel.Entities.Events;
using SuperSquirrel.Entities.RopePhysics;
using SuperSquirrel.Interfaces;

namespace SuperSquirrel.Wrappers
{
	class GrappleWrapper : ISimpleUpdateable, ISimpleDrawable, ISimpleEventListener
	{
		private List<Grapple> grapples;

		public GrappleWrapper()
		{
			grapples = new List<Grapple>();

			SimpleEvent.Queue.Enqueue(new SimpleEvent(EventTypes.LISTENER, new ListenerEventData(EventTypes.GRAPPLE, this)));
		}

		public void EventResponse(SimpleEvent simpleEvent)
		{
			grapples.Add((Grapple)simpleEvent.Data);
		}

		public void Update(float dt)
		{
			for (int i = 0; i < grapples.Count; i++)
			{
				Grapple grapple = grapples[i];

				if (grapple.Destroy)
				{
					grapples.RemoveAt(i);
				}

				grapple.Update(dt);
			}
		}

		public void Draw(SpriteBatch sb)
		{
			foreach (Grapple grapple in grapples)
			{
				grapple.Draw(sb);
			}
		}
	}
}
