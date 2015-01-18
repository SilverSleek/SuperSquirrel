using System;
using System.Collections.Generic;

namespace SuperSquirrel.Entities
{
	class Timer
	{
		private static List<Timer> timers;

		public static void Initialize(List<Timer> timers)
		{
			Timer.timers = timers;
		}

		public Timer(int duration, Action trigger, bool repeating)
		{
			Duration = duration;
			Trigger = trigger;
			Repeating = repeating;

			timers.Add(this);
		}

		public int Delay { get; set; }
		public int Duration { get; set; }

		public bool Repeating { get; private set; }
		public bool Paused { get; set; }
		public bool Destroy { get; set; }

		public float Progress { get; set; }

		public Action Trigger { get; set; }
	}
}
