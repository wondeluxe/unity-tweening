using UnityEngine;

namespace Wondeluxe.Tweening
{
	/// <summary>
	/// Easing functions based on the curve of a sine wave.
	/// </summary>

	public static class SineEase
	{
		private const float HalfPi = Mathf.PI * 0.5f;

		/// <summary>
		/// Ease in using the curve of a sine wave.
		/// </summary>
		/// <param name="t">Normalized time within the curve to evaluate.</param>
		/// <returns>The value on the curve at time <c>t</c>.</returns>

		public static float In(float t)
		{
			return 1f - Mathf.Cos(t * HalfPi);
		}

		/// <summary>
		/// Ease out using the curve of a sine wave.
		/// </summary>
		/// <param name="t">Normalized time within the curve to evaluate.</param>
		/// <returns>The value on the curve at time <c>t</c>.</returns>

		public static float Out(float t)
		{
			return Mathf.Sin(t * HalfPi);
		}

		/// <summary>
		/// Ease in and out using the curve of a sine wave.
		/// </summary>
		/// <param name="t">Normalized time within the curve to evaluate.</param>
		/// <returns>The value on the curve at time <c>t</c>.</returns>

		public static float InOut(float t)
		{
			return -0.5f * (Mathf.Cos(Mathf.PI * t) - 1f);
		}
	}
}