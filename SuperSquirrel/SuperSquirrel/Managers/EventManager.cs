using System.Collections.Generic;

using SuperSquirrel.Entities.Events;
using SuperSquirrel.Interfaces;

namespace SuperSquirrel.Managers
{
	class EventManager : ISimpleEventListener
	{
		private Dictionary<EventTypes, List<ISimpleEventListener>> listenerMap;

		public EventManager()
		{
			listenerMap = new Dictionary<EventTypes, List<ISimpleEventListener>>();
			listenerMap.Add(EventTypes.LISTENER, new List<ISimpleEventListener>());
			listenerMap[EventTypes.LISTENER].Add(this);
		}

		public void EventResponse(SimpleEvent simpleEvent)
		{
			ListenerEventData data = (ListenerEventData)simpleEvent.Data;
			EventTypes eventType = data.EventType;

			if (!listenerMap.ContainsKey(eventType))
			{
				listenerMap.Add(eventType, new List<ISimpleEventListener>());
			}

			listenerMap[eventType].Add(data.Listener);
		}

		public void Update()
		{
			Queue<SimpleEvent> eventQueue = SimpleEvent.Queue;

			while (eventQueue.Count > 0)
			{
				SimpleEvent simpleEvent = eventQueue.Dequeue();

				if (listenerMap.ContainsKey(simpleEvent.Type))
				{
					foreach (ISimpleEventListener listener in listenerMap[simpleEvent.Type])
					{
						listener.EventResponse(simpleEvent);
					}
				}
			}
		}
	}
}
