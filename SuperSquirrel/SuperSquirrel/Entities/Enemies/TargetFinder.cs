﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperSquirrel.Common;
using SuperSquirrel.Entities.Events;

namespace SuperSquirrel.Entities.Enemies
{
	class TargetFinder : Enemy
	{
		private const int HEALTH = 10;
		private const int POINT_VALUE = 25;
		private const int TIME_VALUE = 10000;
		private const int CIRCLE_RADIUS = 30;

		private static SpriteFont debugFont;
		private static Vector2 debugOffset;

		static TargetFinder()
		{
			const int OFFSET = 40;

			if (Constants.DEBUG)
			{
				debugFont = ContentLoader.LoadFont("Debug");
				debugOffset = new Vector2(0, -OFFSET);
			}
		}

		private enum AIStates
		{
			IDLE,
			TARGET,
			SEARCH,
			LOCK,
			FIRE,
			COOLDOWN
		}

		private Vector2 targetPosition;
		private Vector2 laserVelocity;

		private float laserRotation;

		private AIStates aiState;
		private Timer targetTimer;
		private Timer otherTimer;

		private int shotCount;

		public TargetFinder(Vector2 position) :
			base(EnemyTypes.TARGET_FINDER, position, HEALTH, POINT_VALUE, TIME_VALUE, CIRCLE_RADIUS)
		{
			aiState = AIStates.IDLE;
		}

		protected override void AI(float dt)
		{
			const int RANGE = 300;
			const int TRACKING_DURATION = 1750;
			const int SEARCH_DURATION = 3000;

			bool inRange = false;

			if (aiState == AIStates.IDLE || aiState == AIStates.SEARCH || aiState == AIStates.TARGET)
			{
				inRange = Vector2.Distance(Position, Player.Position) <= RANGE;
			}

			if (inRange)
			{
				if (aiState == AIStates.IDLE)
				{
					targetTimer = new Timer(TRACKING_DURATION, LockTarget, false);
				}
				else if (aiState == AIStates.SEARCH)
				{
					otherTimer.Destroy = true;
					otherTimer = null;
					targetTimer.Paused = false;
				}

				targetPosition = Player.Position;
				aiState = AIStates.TARGET;
			}
			else if (aiState == AIStates.TARGET)
			{
				targetTimer.Paused = true;
				otherTimer = new Timer(SEARCH_DURATION, AbandonSearch, false);
				aiState = AIStates.SEARCH;
			}
		}

		private void LockTarget()
		{
			const int LOCK_DURATION = 1500;
			const int LASER_SPEED = 500;

			laserVelocity = Vector2.Normalize(targetPosition - Position) * LASER_SPEED;
			laserRotation = Functions.ComputeAngle(Position, targetPosition);

			targetTimer = null;
			otherTimer = new Timer(LOCK_DURATION, BeginFiring, false);
			aiState = AIStates.LOCK;
		}

		private void BeginFiring()
		{
			const int SHOT_DELAY = 250;

			otherTimer = new Timer(SHOT_DELAY, FireNextLaser, true);
			aiState = AIStates.FIRE;
		}

		private void FireNextLaser()
		{
			const int TOTAL_SHOTS = 3;
			const int COOLDOWN = 2500; 

			SimpleEvent.Queue.Enqueue(new SimpleEvent(EventTypes.LASER, new LaserEventData(Position, laserVelocity, laserRotation, this)));

			shotCount++;

			if (shotCount == TOTAL_SHOTS)
			{
				shotCount = 0;
				otherTimer.Destroy = true;
				otherTimer = new Timer(COOLDOWN, () => { aiState = AIStates.IDLE; }, false);
				aiState = AIStates.COOLDOWN;
			}
		}

		private void AbandonSearch()
		{
			targetTimer.Destroy = true;
			targetTimer = null;
			otherTimer = null;
			aiState = AIStates.IDLE;
		}

		protected override void OnDeath()
		{
			if (targetTimer != null)
			{
				targetTimer.Destroy = true;
			}

			if (otherTimer != null)
			{
				otherTimer.Destroy = true;
			}

			base.OnDeath();
		}

		public override void Draw(SpriteBatch sb)
		{
			if (aiState == AIStates.TARGET || aiState == AIStates.SEARCH || aiState == AIStates.LOCK)
			{
				DrawingFunctions.DrawLine(sb, Position, targetPosition, Color.Red);
			}

			if (Constants.DEBUG)
			{
				string text = aiState.ToString();

				Vector2 position = Position + debugOffset;
				Vector2 origin = debugFont.MeasureString(text) / 2;

				sb.DrawString(debugFont, text, position, Color.Gray, 0, origin, 1, SpriteEffects.None, 0);
			}

			base.Draw(sb);
		}
	}
}
