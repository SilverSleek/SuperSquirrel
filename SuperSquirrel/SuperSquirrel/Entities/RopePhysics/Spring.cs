using Microsoft.Xna.Framework;

namespace SuperSquirrel.Entities.RopePhysics
{
	class Spring
	{
		public const int MAX_SEGMENT_LENGTH = 25;

		private Mass mass1;
		private Mass mass2;
		private Rope rope;

		public Spring(Mass mass1, Mass mass2, Rope rope)
		{
			this.mass1 = mass1;
			this.mass2 = mass2;
			this.rope = rope;

			SegmentLength = MAX_SEGMENT_LENGTH;
		}

		public float SegmentLength { get; set; }

		// this is used to correct springs when purging sleeping masses
		public Mass Mass2
		{
			set { mass2 = value; }
		}

		public void Update(float dt)
		{
			if ((mass1.Fixed && mass2.Fixed) || (mass1.Asleep && mass2.Asleep))
			{
				return;
			}

			float distance = Vector2.Distance(mass1.Position, mass2.Position);
			float difference = distance - SegmentLength;

			if (difference > 0)
			{
				Vector2 direction = Vector2.Normalize(mass1.Position - mass2.Position);

				if (mass1.Fixed)
				{
					ApplyOffset(mass2, direction * difference);
				}
				else if (mass2.Fixed)
				{
					ApplyOffset(mass1, -direction * difference);
				}
				else
				{
					float totalMass = mass1.MassValue + mass2.MassValue;
					float amount1 = mass2.MassValue / totalMass;
					float amount2 = mass1.MassValue / totalMass;

					Vector2 offset1 = -direction * difference * amount1;
					Vector2 offset2 = direction * difference * amount2;

					ApplyOffset(mass1, offset1);
					ApplyOffset(mass2, offset2);
				}
			}
		}

		private void ApplyOffset(Mass mass, Vector2 offset)
		{
			mass.Position += offset;
			mass.Velocity += offset;
		}
	}
}
