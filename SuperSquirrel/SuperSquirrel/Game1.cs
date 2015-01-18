using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using SuperSquirrel.Common;
using SuperSquirrel.Entities;
using SuperSquirrel.Entities.Display;
using SuperSquirrel.Entities.Enemies;
using SuperSquirrel.Entities.Events;
using SuperSquirrel.Entities.Planets;
using SuperSquirrel.Entities.Title;
using SuperSquirrel.Helpers;
using SuperSquirrel.Interfaces;
using SuperSquirrel.Managers;
using SuperSquirrel.Wrappers;

namespace SuperSquirrel
{
	public enum ButtonStates
	{
		HELD,
		PRESSED_THIS_FRAME,
		RELEASED_THIS_FRAME,
		RELEASED
	}

	public enum OriginLocations
	{
		TOP_LEFT,
		CENTER
	}

	public enum Gamestates
	{
		SPLASH = 0,
		TITLE = 1,
		START = 2,
		GAMEPLAY = 3,
		PAUSED = 4,
		END = 5
	}

	class Game1 : Game, ISimpleEventListener
	{
		private GraphicsDeviceManager graphics;
		private SpriteBatch spriteBatch;

		private Player player;
		private Camera camera;
		private Hud hud;

		private InputManager inputManager;
		private EventManager eventManager;
		private TimerManager timerManager;

		private PlanetWrapper planetWrapper;
		private LaserWrapper laserWrapper;
		private EnemyWrapper enemyWrapper;
		
		private PlanetHelper planetHelper;
		private LaserHelper laserHelper;
		private SpawnHelper spawnHelper;

		private TitleScreen titleScreen;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			graphics.PreferredBackBufferWidth = Constants.SCREEN_WIDTH;
			graphics.PreferredBackBufferHeight = Constants.SCREEN_HEIGHT;

			Content.RootDirectory = "Content";
			Window.Title = "Super Squirrel v1.0";
			IsMouseVisible = true;

			SimpleEvent.Queue.Enqueue(new SimpleEvent(EventTypes.LISTENER, new ListenerEventData(EventTypes.EXIT, this)));
		}

		protected override void Initialize()
		{
			ContentLoader.Initialize(Content);

			List<Planet> planets = new List<Planet>();
			List<Laser> lasers = new List<Laser>();
			List<Enemy> enemies = new List<Enemy>();
			List<Timer> timers = new List<Timer>();

			Timer.Initialize(timers);

			planetWrapper = new PlanetWrapper(planets);
			laserWrapper = new LaserWrapper(lasers);
			planetHelper = new PlanetHelper(planets);
			laserHelper = new LaserHelper(lasers, planets);
			spawnHelper = new SpawnHelper(planets, enemies);

			hud = new Hud();
			camera = new Camera();
			player = new Player(planets[0], planetHelper, laserWrapper, camera);
			enemyWrapper = new EnemyWrapper(player, enemies);

			inputManager = new InputManager(camera);
			eventManager = new EventManager();
			timerManager = new TimerManager(timers);

			//titleScreen = new TitleScreen();

			base.Initialize();
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
		}

		protected override void UnloadContent()
		{
		}

		public void EventResponse(SimpleEvent simpleEvent)
		{
			Exit();
		}

		protected override void Update(GameTime gameTime)
		{
			float dt = (float)gameTime.ElapsedGameTime.Milliseconds / 1000;

			inputManager.Update();
			eventManager.Update();
			timerManager.Update(gameTime);

			planetWrapper.Update(dt);
			player.Update(dt);
			camera.Update(dt);
			enemyWrapper.Update(dt);
			laserWrapper.Update(dt);
			laserHelper.Update();
			spawnHelper.Update();
			hud.Update(dt);

			//titleScreen.Update(dt);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.White);

			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default,
				RasterizerState.CullCounterClockwise, null, camera.Transform);

			planetWrapper.Draw(spriteBatch);
			laserWrapper.Draw(spriteBatch);
			enemyWrapper.Draw(spriteBatch);
			player.Draw(spriteBatch);
			hud.Draw(spriteBatch);

			//titleScreen.Draw(spriteBatch);

			spriteBatch.End();
		}
	}
}
