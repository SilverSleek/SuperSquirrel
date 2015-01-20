using System.Collections.Generic;

using Microsoft.Xna.Framework;

using SuperSquirrel.Entities;
using SuperSquirrel.Entities.Display;
using SuperSquirrel.Entities.Enemies;
using SuperSquirrel.Entities.Events;
using SuperSquirrel.Entities.Planets;
using SuperSquirrel.Interfaces;
using SuperSquirrel.Wrappers;

namespace SuperSquirrel.Helpers
{
	class GamestateHelper : ISimpleEventListener
	{
		private UpdateHelper updateHelper;
		private DrawHelper drawHelper;

		public GamestateHelper(UpdateHelper updateHelper, DrawHelper drawHelper)
		{
			this.updateHelper = updateHelper;
			this.drawHelper = drawHelper;

			SimpleEvent.Queue.Enqueue(new SimpleEvent(EventTypes.LISTENER, new ListenerEventData(EventTypes.GAMESTATE, this)));
		}

		public void EventResponse(SimpleEvent simpleEvent)
		{
			switch ((Gamestates)simpleEvent.Data)
			{
				case Gamestates.SPLASH:
					CreateSplashEntities();
					break;

				case Gamestates.TITLE:
					CreateTitleEntities();
					break;

				case Gamestates.GAMEPLAY:
					CreateGameplayEntities();
					break;
			}
		}

		private void CreateSplashEntities()
		{
		}

		private void CreateTitleEntities()
		{
		}

		private void CreateGameplayEntities()
		{
			List<ISimpleUpdateable> updateables = new List<ISimpleUpdateable>();
			List<ISimpleDrawable> drawables = new List<ISimpleDrawable>();

			List<Laser> lasers = new List<Laser>();
			List<Enemy> enemies = new List<Enemy>();
			List<Planet> planets = new List<Planet>();
			planets.Add(new Planet(Vector2.Zero, PlanetSizes.MEDIUM));

			LaserHelper laserHelper = new LaserHelper(lasers, planets);
			SpawnHelper spawnHelper = new SpawnHelper(planets, enemies);
			PlanetHelper planetHelper = new PlanetHelper(planets);

			Hud hud = new Hud();
			Player player = new Player(planets[0], planetHelper);
			
			LaserWrapper laserWrapper = new LaserWrapper(lasers);
			EnemyWrapper enemyWrapper = new EnemyWrapper(player, enemies);
			PlanetWrapper planetWrapper = new PlanetWrapper(planets);

			updateables.Add(planetWrapper);
			updateables.Add(player);
			updateables.Add(Camera.Instance);
			updateables.Add(enemyWrapper);
			updateables.Add(laserWrapper);
			updateables.Add(laserHelper);
			updateables.Add(spawnHelper);
			updateables.Add(hud);

			drawables.Add(planetWrapper);
			drawables.Add(enemyWrapper);
			drawables.Add(player);
			drawables.Add(laserWrapper);
			drawables.Add(Camera.Instance);
			drawables.Add(hud);

			updateHelper.Updateables = updateables;
			drawHelper.Drawables = drawables;
		}
	}
}
