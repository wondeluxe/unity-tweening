namespace Wondeluxe.Tweening
{
	/// <summary>
	/// Easing functions based on an approximated exponential curve (using t¹⁰).
	/// </summary>

	public static class ExponentialEase
	{
		/// <summary>
		/// Ease in using an approximated exponential curve (using t¹⁰).
		/// </summary>
		/// <param name="t">Normalized time within the curve to evaluate.</param>
		/// <returns>The value on the curve at time <c>t</c>.</returns>

		public static float In(float t)
		{
			return t * t * t * t * t * t * t * t * t * t;
		}

		/// <summary>
		/// Ease out using an approximated exponential curve (using t¹⁰).
		/// </summary>
		/// <param name="t">Normalized time within the curve to evaluate.</param>
		/// <returns>The value on the curve at time <c>t</c>.</returns>

		public static float Out(float t)
		{
			t -= 1f;

			return -(t * t * t * t * t * t * t * t * t * t - 1f);
		}

		/// <summary>
		/// Ease in and out using an approximated exponential curve (using t¹⁰).
		/// </summary>
		/// <param name="t">Normalized time within the curve to evaluate.</param>
		/// <returns>The value on the curve at time <c>t</c>.</returns>

		public static float InOut(float t)
		{
			t *= 2f;

			if (t <= 1f)
			{
				return 0.5f * t * t * t * t * t * t * t * t * t * t;
			}

			t -= 2f;

			return -0.5f * (t * t * t * t * t * t * t * t * t * t - 2f);
		}
	}
}