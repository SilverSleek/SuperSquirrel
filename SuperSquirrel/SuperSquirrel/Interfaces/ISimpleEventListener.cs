using SuperSquirrel.Entities.Events;

namespace SuperSquirrel.Interfaces
{
	interface ISimpleEventListener
	{
		void EventResponse(SimpleEvent simpleEvent);
	}
}
