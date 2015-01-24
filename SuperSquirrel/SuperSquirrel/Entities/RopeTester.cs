using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperSquirrel.Entities.Events;
using SuperSquirrel.Interfaces;

namespace SuperSquirrel.Entities
{
	class RopeTester : ISimpleEventListener
	{
		private Rope rope;

		public RopeTester()
		{
			SimpleEvent.Queue.Enqueue(new SimpleEvent(EventTypes.LISTENER, new ListenerEventData(EventTypes.MOUSE, this)));

			Vector2 start = new Vector2(0, 0);
			Vector2 end = new Vector2(300, 300);

			rope = new Rope(start, end);
		}

		public void EventResponse(SimpleEvent simpleEvent)
		{
			MouseEventData data = (MouseEventData)simpleEvent.Data;

			rope.SetFirstPoint(data.NewWorldPosition);
		}

		public void Update(float dt)
		{
			rope.Update(dt);
		}

		public void Draw(SpriteBatch sb)
		{
			rope.Draw(sb);
		}
	}
}
