namespace Wondeluxe.Tweening
{
	/// <summary>
	/// Encapsulates an easing function - a method that evaluates a curve over time.
	/// </summary>
	/// <param name="t">Normalised time between 0 and 1.</param>
	/// <returns>t modified by the implemented easing function.</returns>

	public delegate float Ease(float t);
}