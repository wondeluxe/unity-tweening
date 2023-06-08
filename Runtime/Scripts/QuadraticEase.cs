namespace Wondeluxe.Tweening
{
	/// <summary>
	/// Easing functions based on a quadratic curve (t²).
	/// </summary>

	public static class QuadraticEase
	{
		/// <summary>
		/// Ease in using a quadratic curve (t²).
		/// </summary>
		/// <param name="t">Normalized time within the curve to evaluate.</param>
		/// <returns>The value on the curve at time <c>t</c>.</returns>

		public static float In(float t)
		{
			return t * t;
		}

		/// <summary>
		/// Ease out using a quadratic curve (t²).
		/// </summary>
		/// <param name="t">Normalized time within the curve to evaluate.</param>
		/// <returns>The value on the curve at time <c>t</c>.</returns>

		public static float Out(float t)
		{
			t -= 1f;

			return -(t * t - 1f);
		}

		/// <summary>
		/// Ease in and out using a quadratic curve (t²).
		/// </summary>
		/// <param name="t">Normalized time within the curve to evaluate.</param>
		/// <returns>The value on the curve at time <c>t</c>.</returns>

		public static float InOut(float t)
		{
			t *= 2f;

			if (t <= 1f)
			{
				return 0.5f * t * t;
			}

			t -= 2f;

			return -0.5f * (t * t - 2f);
		}
	}
}