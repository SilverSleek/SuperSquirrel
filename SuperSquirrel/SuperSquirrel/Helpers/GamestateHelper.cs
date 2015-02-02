using System.Collections.Generic;

using Microsoft.Xna.Framework;

using SuperSquirrel.Entities;
using SuperSquirrel.Entities.Display;
using SuperSquirrel.Entities.Enemies;
using SuperSquirrel.Entities.Events;
using SuperSquirrel.Entities.Planets;
using SuperSquirrel.Entities.RopePhysics;
using SuperSquirrel.Entities.Title;
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
			List<ISimpleUpdateable> updateables = new List<ISimpleUpdateable>();
			List<ISimpleDrawable> drawables = new List<ISimpleDrawable>();

			switch ((Gamestates)simpleEvent.Data)
			{
				case Gamestates.SPLASH:
					CreateSplashEntities();
					break;

				case Gamestates.TITLE:
					CreateTitleEntities(updateables, drawables);
					break;

				case Gamestates.GAMEPLAY:
					CreateGameplayEntities(updateables, drawables);
					break;
			}

			updateHelper.Updateables = updateables;
			drawHelper.Drawables = drawables;
		}

		private void CreateSplashEntities()
		{
		}

		private void CreateTitleEntities(List<ISimpleUpdateable> updateables, List<ISimpleDrawable> drawables)
		{
			TitleScreen titleScreen = new TitleScreen();

			updateables.Add(titleScreen);
			drawables.Add(titleScreen);
		}

		private void CreateGameplayEntities(List<ISimpleUpdateable> updateables, List<ISimpleDrawable> drawables)
		{
			List<Laser> lasers = new List<Laser>();
			List<Enemy> enemies = new List<Enemy>();
			List<Planet> planets = new List<Planet>();

			// this the player's temporary starting planet
			planets.Add(new Planet(Vector2.Zero, PlanetSizes.SMALL));

			LaserHelper laserHelper = new LaserHelper(lasers, planets);
			PlanetHelper planetHelper = new PlanetHelper(planets);
			//GenerationHelper generationHelper = new GenerationHelper(planets, enemies);

			Hud hud = new Hud();
			//Stars stars = new Stars();
			Player player = new Player(planets[0], planetHelper);
			//RopeTester ropeTester = new RopeTester();
			TutorialIcons icons = new TutorialIcons(updateables, drawables);
		
			LaserWrapper laserWrapper = new LaserWrapper(lasers);
			EnemyWrapper enemyWrapper = new EnemyWrapper(player, enemies);
			PlanetWrapper planetWrapper = new PlanetWrapper(planets);
			GrappleWrapper grappleWrapper = new GrappleWrapper();

			Rope.Initialize(planetHelper);

			updateables.Add(planetWrapper);
			//updateables.Add(ropeTester);
			updateables.Add(player);
			updateables.Add(icons);
			updateables.Add(Camera.Instance);
			updateables.Add(enemyWrapper);
			updateables.Add(laserWrapper);
			updateables.Add(grappleWrapper);
			updateables.Add(laserHelper);
			//updateables.Add(generationHelper);
			updateables.Add(hud);
			//updateables.Add(stars);

			//drawables.Add(stars);
			drawables.Add(planetWrapper);
			drawables.Add(icons);
			drawables.Add(grappleWrapper);
			drawables.Add(enemyWrapper);
			drawables.Add(player);
			//drawables.Add(ropeTester);
			drawables.Add(laserWrapper);
			drawables.Add(Camera.Instance);
			drawables.Add(hud);
		}
	}
}
