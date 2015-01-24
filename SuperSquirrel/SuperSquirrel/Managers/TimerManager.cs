using System.Collections.Generic;

using Microsoft.Xna.Framework;

using SuperSquirrel.Entities;

namespace SuperSquirrel.Managers
{
	class TimerManager
	{
		private List<Timer> timers;

		public TimerManager(List<Timer> timers)
		{
			this.timers = timers;
		}

		public void Update(float dt)
		{
			int milliseconds = (int)(dt * 1000);

			for (int i = 0; i < timers.Count; i++)
			{
				Timer timer = timers[i];

				if (timer.Destroy)
				{
					timers.RemoveAt(i);
				}
				else if (!timer.Paused)
				{
					timer.Delay += milliseconds;
					timer.Progress = (float)timer.Delay / timer.Duration;

					if (timer.Delay >= timer.Duration)
					{
						timer.Trigger();

						if (timer.Repeating)
						{
							timer.Delay = timer.Duration - timer.Delay;
						}
						else
						{
							timers.RemoveAt(i);
						}
					}
				}
			}
		}
	}
}
