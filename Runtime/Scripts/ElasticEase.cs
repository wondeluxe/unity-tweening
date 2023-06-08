using UnityEngine;

namespace Wondeluxe.Tweening
{
	/// <summary>
	/// Easing functions using a curve to simulate an elastic effect.
	/// </summary>

	public static class ElasticEase
	{
		private const float TwoPi = Mathf.PI * 2f;

		/// <summary>
		/// Ease using a curve to simulate an elastic in effect.
		/// </summary>
		/// <param name="t">Normalized time within the curve to evaluate.</param>
		/// <returns>The value on the curve at time <c>t</c>.</returns>

		public static float In(float t)
		{
			if (t <= 0f)
			{
				return 0f;
			}

			if (t >= 1f)
			{
				return 1f;
			}

			float p = 0.3f;
			float s = p / 4f;

			t -= 1f;

			float postFix = Mathf.Pow(2f, 10f * t);

			return -(postFix * Mathf.Sin((t - s) * TwoPi / p));
		}

		/// <summary>
		/// Ease using a curve to simulate an elastic out effect.
		/// </summary>
		/// <param name="t">Normalized time within the curve to evaluate.</param>
		/// <returns>The value on the curve at time <c>t</c>.</returns>

		public static float Out(float t)
		{
			if (t <= 0f)
			{
				return 0f;
			}

			if (t >= 1f)
			{
				return 1f;
			}

			float p = 0.3f;
			float s = p / 4f;

			return Mathf.Pow(2f, -10f * t) * Mathf.Sin((t - s) * TwoPi / p) + 1f;
		}

		/// <summary>
		/// Ease using a curve to simulate an elastic in and out effect.
		/// </summary>
		/// <param name="t">Normalized time within the curve to evaluate.</param>
		/// <returns>The value on the curve at time <c>t</c>.</returns>

		public static float InOut(float t)
		{
			if (t <= 0f)
			{
				return 0f;
			}

			if (t >= 1f)
			{
				return 1f;
			}

			float p = 0.3f * 1.5f;
			float s = p / 4f;
			float postFix;

			t /= 0.5f;

			if (t < 1f)
			{
				postFix = Mathf.Pow(2f, 10f * (t -= 1f));

				return -0.5f * (postFix * Mathf.Sin((t - s) * (2f * Mathf.PI) / p));
			}

			postFix = Mathf.Pow(2f, -10f * (t -= 1f));

			return postFix * Mathf.Sin((t - s) * (2f * Mathf.PI) / p) * 0.5f + 1f;
		}
	}
}