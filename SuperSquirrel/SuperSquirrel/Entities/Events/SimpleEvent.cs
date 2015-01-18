using System.Collections.Generic;

namespace SuperSquirrel.Entities.Events
{
	public enum EventTypes
	{
		LISTENER,
		KEYBOARD,
		MOUSE,
		GAMESTATE,
		POINTS,
		LASER,
		EXIT
	}

	class SimpleEvent
	{
		static SimpleEvent()
		{
			Queue = new Queue<SimpleEvent>();
		}

		public static Queue<SimpleEvent> Queue { get; private set; }

		public SimpleEvent(EventTypes type, object data)
		{
			Type = type;
			Data = data;
		}

		public EventTypes Type { get; private set; }

		public object Data { get; private set; }
	}
}
