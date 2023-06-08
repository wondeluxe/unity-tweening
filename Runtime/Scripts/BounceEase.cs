namespace Wondeluxe.Tweening
{
	/// <summary>
	/// Easing functions using a curve to provide a bounce effect.
	/// </summary>

	public static class BounceEase
	{
		/// <summary>
		/// Ease using a curve to provide a bounce in effect.
		/// </summary>
		/// <param name="t">Normalized time within the curve to evaluate.</param>
		/// <returns>The value on the curve at time <c>t</c>.</returns>

		public static float In(float t)
		{
			return 1f - Ease(1f - t);
		}

		/// <summary>
		/// Ease using a curve to provide a bounce out effect.
		/// </summary>
		/// <param name="t">Normalized time within the curve to evaluate.</param>
		/// <returns>The value on the curve at time <c>t</c>.</returns>

		public static float Out(float t)
		{
			return Ease(t);
		}

		/// <summary>
		/// Ease using a curve to provide a bounce in and out effect.
		/// </summary>
		/// <param name="t">Normalized time within the curve to evaluate.</param>
		/// <returns>The value on the curve at time <c>t</c>.</returns>

		public static float InOut(float t)
		{
			if (t < 0.5f)
			{
				return 1f - Ease(1f - t * 2f) * 0.5f - 0.5f;
			}

			return Ease(t * 2f - 1f) * 0.5f + 0.5f;
		}

		private static float Ease(float t)
		{
			if (t < (1f / 2.75f))
			{
				return 7.5625f * t * t;
			}

			float postFix;

			if (t < (2f / 2.75f))
			{
				postFix = t -= (1.5f / 2.75f);

				return 7.5625f * postFix * t + 0.75f;
			}

			if (t < (2.5f / 2.75f))
			{
				postFix = t -= (2.25f / 2.75f);

				return 7.5625f * postFix * t + 0.9375f;
			}

			postFix = t -= (2.625f / 2.75f);

			return 7.5625f * postFix * t + 0.984375f;
		}
	}
}