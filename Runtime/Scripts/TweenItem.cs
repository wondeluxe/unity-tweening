using System.Reflection;

namespace Wondeluxe.Tweening
{
	/// <summary>
	/// Object holding the member info and to/from values used by a Tween.
	/// </summary>

	internal abstract class TweenItem
	{
		/// <summary>
		/// The to/from values used by a Tween.
		/// </summary>

		internal TweenValue Value { get; set; }

		/// <summary>
		/// Updates a member of an object between the values assigned to Value.
		/// </summary>
		/// <param name="obj">The object whose member to update.</param>
		/// <param name="normalizedTime">The normalised time to caluclate the result value with.</param>

		internal abstract void Udpate(object obj, float normalizedTime);
	}

	/// <summary>
	/// Object holding the field info and to/from values used by a Tween.
	/// </summary>

	internal sealed class FieldItem : TweenItem
	{
		/// <summary>
		/// FieldInfo used by a Tween.
		/// </summary>

		internal FieldInfo Info { get; private set; }

		/// <summary>
		/// Instantiate a new instance of FieldItem.
		/// </summary>
		/// <param name="info">The FieldInfo representing a field to be tweened.</param>
		/// <param name="fromValue">The value to tween from.</param>
		/// <param name="toValue">The value to tween to.</param>

		internal FieldItem(FieldInfo info, object fromValue, object toValue)
		{
			Info = info;
			Value = TweenValue.Create(info.FieldType, fromValue, toValue);
		}

		internal override void Udpate(object obj, float normalizedTime)
		{
			Info.SetValue(obj, Value.Evaluate(normalizedTime));
		}
	}

	/// <summary>
	/// Object holding the property info and to/from values used by a Tween.
	/// </summary>

	internal sealed class PropertyItem : TweenItem
	{
		/// <summary>
		/// PropertyInfo used by a Tween.
		/// </summary>

		internal PropertyInfo Info { get; private set; }

		/// <summary>
		/// Instantiate a new instance of PropertyItem.
		/// </summary>
		/// <param name="info">The PropertyInfo representing a property to be tweened.</param>
		/// <param name="fromValue">The value to tween from.</param>
		/// <param name="toValue">The value to tween to.</param>

		internal PropertyItem(PropertyInfo info, object fromValue, object toValue)
		{
			Info = info;
			Value = TweenValue.Create(info.PropertyType, fromValue, toValue);
		}

		internal override void Udpate(object obj, float normalizedTime)
		{
			Info.SetValue(obj, Value.Evaluate(normalizedTime));
		}
	}
}