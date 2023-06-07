using System;
using UnityEngine;

namespace Wondeluxe.Tweening
{
	/// <summary>
	/// Holds from and to values for a tweened member.
	/// </summary>

	internal abstract class TweenValue
	{
		/// <summary>
		/// Create a new TweenValue for a given Type.
		/// </summary>
		/// <param name="type">The Type to create the TweenValue for.</param>
		/// <param name="from">Value to assign to the TweenValue's From property.</param>
		/// <param name="to">Value to assign to the TweenValue's To property.</param>
		/// <returns>A new TweenValue for the given Type.</returns>
		/// <exception cref="Exception">Throws an exception if the given type cannot be tweened.</exception>

		internal static TweenValue Create(Type type, object from, object to)
		{
			if (type == typeof(int))
				return new IntValue { From = from, To = to };

			if (type == typeof(float))
				return new FloatValue { From = from, To = to };

			if (type == typeof(double))
				return new DoubleValue { From = from, To = to };

			if (type == typeof(Vector2))
				return new Vector2Value { From = from, To = to };

			if (type == typeof(Vector3))
				return new Vector3Value { From = from, To = to };

			if (type == typeof(Vector4))
				return new Vector4Value { From = from, To = to };

			if (type == typeof(Vector2Int))
				return new Vector2IntValue { From = from, To = to };

			if (type == typeof(Vector3Int))
				return new Vector3IntValue { From = from, To = to };

			if (type == typeof(Color))
				return new ColorValue { From = from, To = to };

			if (type == typeof(Quaternion))
				return new QuaternionValue { From = from, To = to };

			throw new Exception($"Type '{type}' is not supported.");
		}

		/// <summary>
		/// The value to tween from.
		/// </summary>

		internal abstract object From { get; set; }

		/// <summary>
		/// The value to tween to.
		/// </summary>

		internal abstract object To { get; set; }

		/// <summary>
		/// Returns a value linearly interpolated between From and To.
		/// </summary>
		/// <param name="t">Normalized value between 0 and 1 to interpolate with.</param>
		/// <returns>The interpolated value between From and To.</returns>

		internal abstract object Evaluate(float t);
	}

	/// <summary>
	/// Holds from and to values for a tweened member.
	/// </summary>
	/// <typeparam name="T">The value type to be tweened.</typeparam>

	internal abstract class TweenValue<T> : TweenValue
	{
		protected T from;
		protected T to;

		internal override object From
		{
			get => from;
			set => from = (T)value;
		}

		internal override object To
		{
			get => to;
			set => to = (T)value;
		}
	}

	internal sealed class IntValue : TweenValue<int>
	{
		internal override object Evaluate(float t)
		{
			return from + (int)((to - from) * t);
		}
	}

	internal sealed class FloatValue : TweenValue<float>
	{
		internal override object Evaluate(float t)
		{
			return from + (to - from) * t;
		}
	}

	internal sealed class DoubleValue : TweenValue<double>
	{
		internal override object Evaluate(float t)
		{
			return from + (to - from) * t;
		}
	}

	internal sealed class Vector2Value : TweenValue<Vector2>
	{
		internal override object Evaluate(float t)
		{
			return Vector2.Lerp(from, to, t);
		}
	}

	internal sealed class Vector3Value : TweenValue<Vector3>
	{
		internal override object Evaluate(float t)
		{
			return Vector3.Lerp(from, to, t);
		}
	}

	internal sealed class Vector4Value : TweenValue<Vector4>
	{
		internal override object Evaluate(float t)
		{
			return Vector4.Lerp(from, to, t);
		}
	}

	internal sealed class Vector2IntValue : TweenValue<Vector2Int>
	{
		internal override object Evaluate(float t)
		{
			Vector2Int delta = to - from;
			return new Vector2Int(from.x + (int)(delta.x * t), from.y + (int)(delta.y * t));
		}
	}

	internal sealed class Vector3IntValue : TweenValue<Vector3Int>
	{
		internal override object Evaluate(float t)
		{
			Vector3Int delta = to - from;
			return new Vector3Int(from.x + (int)(delta.x * t), from.y + (int)(delta.y * t), from.z + (int)(delta.z * t));
		}
	}

	internal sealed class ColorValue : TweenValue<Color>
	{
		internal override object Evaluate(float t)
		{
			return Color.Lerp(from, to, t);
		}
	}

	internal sealed class QuaternionValue : TweenValue<Quaternion>
	{
		internal override object Evaluate(float t)
		{
			return Quaternion.Lerp(from, to, t);
		}
	}
}