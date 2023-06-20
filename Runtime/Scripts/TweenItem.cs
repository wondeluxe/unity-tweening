using System.Reflection;

namespace Wondeluxe.Tweening
{
	/// <summary>
	/// Object for applying updates to a member in a <see cref="Tween"/>.
	/// </summary>

	internal abstract class TweenItem
	{
		/// <summary>
		/// Name of the member this TweenItem updates.
		/// </summary>

		internal abstract string Name { get; }

		/// <summary>
		/// The values this TweenItem will udpate a member from and to.
		/// </summary>

		internal TweenValue Value { get; set; }

		/// <summary>
		/// Updates a member of an object between the values assigned to <see cref="Value"/>.
		/// </summary>
		/// <param name="obj">The object whose member to update.</param>
		/// <param name="normalizedTime">The normalised time to caluclate the result value with.</param>

		internal abstract void Udpate(object obj, float normalizedTime);

		/// <summary>
		/// Returns a string representation of the TweenItem.
		/// </summary>
		/// <returns>A string representation of the TweenItem.</returns>

		public override string ToString()
		{
			return $"(Name = {Name}, From = {Value.From}, To = {Value.To})";
		}
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

		internal override string Name
		{
			get => Info.Name;
		}

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

		internal override string Name
		{
			get => Info.Name;
		}

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