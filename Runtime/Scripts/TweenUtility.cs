using System;
using UnityEngine;

namespace Wondeluxe.Tweening
{
	/// <summary>
	/// Helper methods for the Tweening API.
	/// </summary>

	public static class TweenUtility
	{
		// TODO Add internal CreateValue method.
		// TODO Add editor methods here?

		/// <summary>
		/// Check if a type can be tweened.
		/// </summary>
		/// <param name="type">The Type to check.</param>
		/// <returns><c>true</c> if <c>type</c> can be tweened, <c>false</c> otherwise.</returns>

		public static bool IsTypeSupported(Type type)
		{
			if (type == typeof(int))
				return true;

			if (type == typeof(float))
				return true;

			if (type == typeof(double))
				return true;

			if (type == typeof(Vector2))
				return true;

			if (type == typeof(Vector3))
				return true;

			if (type == typeof(Vector4))
				return true;

			if (type == typeof(Vector2Int))
				return true;

			if (type == typeof(Vector3Int))
				return true;

			if (type == typeof(Color))
				return true;

			if (type == typeof(Quaternion))
				return true;

			return false;
		}

		public static string SerializeType(Type type)
		{
			return type.FullName;
		}

		/// <summary>
		/// Returns a string representation of a value.
		/// </summary>
		/// <param name="value">The value to return the string representation of.</param>
		/// <returns>A string representation of <paramref name="value"/>.</returns>

		public static string SerializeValue(object value)
		{
			Type type = value.GetType();

			if (type == typeof(int))
				return value.ToString();

			if (type == typeof(float))
				return ((float)value).ToString("G");

			if (type == typeof(double))
				return ((double)value).ToString("G");

			if (type == typeof(Vector2))
				return ((Vector2)value).ToString("G");

			if (type == typeof(Vector3))
				return ((Vector3)value).ToString("G");

			if (type == typeof(Vector4))
				return ((Vector4)value).ToString("G");

			if (type == typeof(Vector2Int))
				return ((Vector2Int)value).ToString("G");

			if (type == typeof(Vector3Int))
				return ((Vector3Int)value).ToString("G");

			if (type == typeof(Color))
				return value.ToString();

			if (type == typeof(Quaternion))
				return ((Quaternion)value).ToString("G");

			throw new ArgumentException($"Object type ({type}) not supported.");
		}

		/// <summary>
		/// Converts the string representation of a value to its object equivalent.
		/// </summary>
		/// <param name="type">The name of the type to convert to.</param>
		/// <param name="value">The string representation of the value to convert.</param>
		/// <returns><c>value</c> converted to the type with name <c>type</c>.</returns>

		public static object Deserialize(string type, string value)
		{
			if (type == typeof(int).FullName)
				return int.Parse(value);

			if (type == typeof(float).FullName)
				return float.Parse(value);

			if (type == typeof(double).FullName)
				return double.Parse(value);

			if (type == typeof(Vector2).FullName)
				return Vector2Extensions.Parse(value);

			if (type == typeof(Vector3).FullName)
				return Vector3Extensions.Parse(value);

			if (type == typeof(Vector4).FullName)
				return Vector4Extensions.Parse(value);

			if (type == typeof(Vector2Int).FullName)
				return Vector2IntExtensions.Parse(value);

			if (type == typeof(Vector3Int).FullName)
				return Vector3IntExtensions.Parse(value);

			if (type == typeof(Color).FullName)
				return ColorExtensions.Parse(value);

			if (type == typeof(Quaternion).FullName)
				return QuaternionExtensions.Parse(value);

			throw new ArgumentException($"Object type ({type}) not supported.");
		}
	}
}