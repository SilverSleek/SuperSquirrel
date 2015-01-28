using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using SuperSquirrel.Common;
using SuperSquirrel.Entities;
using SuperSquirrel.Entities.Events;
using SuperSquirrel.Helpers;
using SuperSquirrel.Interfaces;
using SuperSquirrel.Managers;

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
		CENTER,
		BOTTOM_CENTER
	}

	public enum Gamestates
	{
		SPLASH = 0,
		TITLE = 1,
		GAMEPLAY = 2
	}

	class Game1 : Game, ISimpleEventListener
	{
		private GraphicsDeviceManager graphics;
		private SpriteBatch spriteBatch;

		private InputManager inputManager;
		private EventManager eventManager;
		private TimerManager timerManager;

		private UpdateHelper updateHelper;
		private DrawHelper drawHelper;

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
			List<Timer> timers = new List<Timer>();

			ContentLoader.Initialize(Content);
			Timer.Initialize(timers);

			inputManager = new InputManager();
			eventManager = new EventManager();
			timerManager = new TimerManager(timers);

			updateHelper = new UpdateHelper();
			drawHelper = new DrawHelper();

			GamestateHelper gamestateHelper = new GamestateHelper(updateHelper, drawHelper);

			SimpleEvent.Queue.Enqueue(new SimpleEvent(EventTypes.GAMESTATE, Gamestates.GAMEPLAY));

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
			float dt = (float)gameTime.ElapsedGameTime.Milliseconds / 1000 / Constants.TIME_FACTOR;

			inputManager.Update();
			eventManager.Update();
			timerManager.Update(dt);

			updateHelper.Update(dt);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.White);

			drawHelper.Draw(spriteBatch);
		}
	}
}
