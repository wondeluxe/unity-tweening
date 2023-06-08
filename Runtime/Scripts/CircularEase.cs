using UnityEngine;

namespace Wondeluxe.Tweening
{
	/// <summary>
	/// Easing functions based on the curve of a half circle.
	/// </summary>

	public static class CircularEase
	{
		/// <summary>
		/// Ease in using the curve of a half circle.
		/// </summary>
		/// <param name="t">Normalized time within the curve to evaluate.</param>
		/// <returns>The value on the curve at time <c>t</c>.</returns>

		public static float In(float t)
		{
			return -(Mathf.Sqrt(1f - t * t) - 1f);
		}

		/// <summary>
		/// Ease out using the curve of a half circle.
		/// </summary>
		/// <param name="t">Normalized time within the curve to evaluate.</param>
		/// <returns>The value on the curve at time <c>t</c>.</returns>

		public static float Out(float t)
		{
			t -= 1f;

			return Mathf.Sqrt(1f - t * t);
		}

		/// <summary>
		/// Ease in and out using the curve of a half circle.
		/// </summary>
		/// <param name="t">Normalized time within the curve to evaluate.</param>
		/// <returns>The value on the curve at time <c>t</c>.</returns>

		public static float InOut(float t)
		{
			t *= 2f;

			if (t < 1f)
			{
				return -0.5f * (Mathf.Sqrt(1f - t * t) - 1f);
			}

			t -= 2f;

			return 0.5f * (Mathf.Sqrt(1f - t * t) + 1f);
		}
	}
}