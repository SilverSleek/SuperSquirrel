using System.Collections.Generic;

using SuperSquirrel.Interfaces;

namespace SuperSquirrel.Helpers
{
	class UpdateHelper
	{
		public UpdateHelper()
		{
			Updateables = new List<ISimpleUpdateable>();
		}

		public List<ISimpleUpdateable> Updateables { get; set; }

		public void Update(float dt)
		{
			foreach (ISimpleUpdateable updateable in Updateables)
			{
				updateable.Update(dt);
			}
		}
	}
}
