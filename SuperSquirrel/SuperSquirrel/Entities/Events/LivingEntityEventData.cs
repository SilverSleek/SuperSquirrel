namespace SuperSquirrel.Entities.Events
{
	public enum ActionTypes
	{
		ADD,
		REMOVE
	}

	class LivingEntityEventData
	{
		public LivingEntityEventData(LivingEntity entity, ActionTypes action)
		{
			Entity = entity;
			Action = action;
		}

		public LivingEntity Entity { get; private set; }
		public ActionTypes Action { get; private set; }
	}
}
