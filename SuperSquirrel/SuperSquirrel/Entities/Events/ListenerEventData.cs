using SuperSquirrel.Interfaces;

namespace SuperSquirrel.Entities.Events
{
	class ListenerEventData
	{
		public ListenerEventData(EventTypes eventType, ISimpleEventListener listener)
		{
			EventType = eventType;
			Listener = listener;
		}

		public EventTypes EventType { get; private set; }
		public ISimpleEventListener Listener { get; private set; }
	}
}
