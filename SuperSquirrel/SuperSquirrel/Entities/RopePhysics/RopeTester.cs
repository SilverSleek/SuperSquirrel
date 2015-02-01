using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperSquirrel.Entities.Events;
using SuperSquirrel.Interfaces;

namespace SuperSquirrel.Entities.RopePhysics
{
	class RopeTester : ISimpleUpdateable, ISimpleDrawable, ISimpleEventListener
	{
		private const int TAIL_MASS = 1;
		private const int ROPE_LENGTH = 250;

		private Mass mass;
		private Rope rope;

		public RopeTester()
		{
			Mass tailMass = new Mass(TAIL_MASS, Vector2.One * 2, Vector2.Zero);

			mass = new Mass(1, Vector2.One, Vector2.Zero);
			mass.Fixed = true;

			//rope = new Rope(Vector2.Zero, new Vector2(300, 0), mass);

			SimpleEvent.Queue.Enqueue(new SimpleEvent(EventTypes.LISTENER, new ListenerEventData(EventTypes.MOUSE, this)));
		}

		public void EventResponse(SimpleEvent simpleEvent)
		{
			mass.Position = ((MouseEventData)simpleEvent.Data).NewWorldPosition;
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
