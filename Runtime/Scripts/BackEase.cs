namespace Wondeluxe.Tweening
{
	/// <summary>
	/// Easing functions using a curve to simulate anticipation and/or overshoot.
	/// </summary>

	public static class BackEase
	{
		/// <summary>
		/// Ease using a curve to simulate anticipation.
		/// </summary>
		/// <param name="t">Normalized time within the curve to evaluate.</param>
		/// <returns>The value on the curve at time <c>t</c>.</returns>

		public static float In(float t)
		{
			float s = 1.70158f;

			return t * t * ((s + 1f) * t - s);
		}

		/// <summary>
		/// Ease using a curve to simulate overshoot.
		/// </summary>
		/// <param name="t">Normalized time within the curve to evaluate.</param>
		/// <returns>The value on the curve at time <c>t</c>.</returns>

		public static float Out(float t)
		{
			float s = 1.70158f;

			return ((t = t - 1f) * t * ((s + 1f) * t + s) + 1f);
		}

		/// <summary>
		/// Ease using a curve to simulate anticipation and overshoot.
		/// </summary>
		/// <param name="t">Normalized time within the curve to evaluate.</param>
		/// <returns>The value on the curve at time <c>t</c>.</returns>

		public static float InOut(float t)
		{
			float s = 1.70158f;

			if ((t /= 0.5f) < 1f)
			{
				return 0.5f * (t * t * (((s *= (1.525f)) + 1f) * t - s));
			}

			float postFix = t -= 2f;

			return 0.5f * (postFix * t * (((s *= (1.525f)) + 1f) * t + s) + 2f);
		}
	}
}