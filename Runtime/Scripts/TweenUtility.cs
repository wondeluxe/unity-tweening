using System;

namespace Wondeluxe.Tweening
{
	/// <summary>
	/// Helper methods for the Tweening API.
	/// </summary>

	public static class TweenUtility
	{
		/// <summary>
		/// Check if a type can be tweened.
		/// </summary>
		/// <param name="type">The Type to check.</param>
		/// <returns><c>true</c> if <c>type</c> can be tweened, <c>false</c> otherwise.</returns>

		public static bool IsTypeSupported(Type type)
		{
			if (type.IsValueType)
			{
				object defaultValue = Activator.CreateInstance(type);

				try
				{
					TweenValue.Create(type, defaultValue, defaultValue);
					return true;
				}
				catch (Exception) { }
			}

			return false;
		}
	}
}