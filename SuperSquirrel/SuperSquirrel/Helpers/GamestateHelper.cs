using System;

using Microsoft.Xna.Framework;

using SuperSquirrel.Entities.Events;
using SuperSquirrel.Interfaces;

namespace SuperSquirrel.Helpers
{
	class GamestateHelper : ISimpleEventListener
	{
		private Action[] creationFunctions;

		public GamestateHelper()
		{
			creationFunctions = new Action[6];
			creationFunctions[0] = CreateSplashEntities;
		}

		public void EventResponse(SimpleEvent simpleEvent)
		{
			creationFunctions[(int)(Gamestates)simpleEvent.Data]();
		}

		private void CreateSplashEntities()
		{
		}

		private void CreateTitleEntities()
		{
		}

		private void CreateStartEntities()
		{
		}

		private void CreateGameplayEntities()
		{
		}

		private void CreatePauseEntities()
		{
		}

		private void CreateEndEntities()
		{
		}
	}
}
