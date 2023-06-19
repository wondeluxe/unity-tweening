using System;
using UnityEngine;

namespace Wondeluxe.Tweening
{
	/// <summary>
	/// Data store for a member to tween.
	/// </summary>

	[Serializable]
	public class SerializableTweenMember
	{
		[SerializeField]
		[Tooltip("Name of the member to tween.")]
		private string name;

		[SerializeField]
		[Tooltip("The member's type.")]
		private string type;

		[SerializeField]
		[Tooltip("Value to tween the member to.")]
		private string value;

		/// <summary>
		/// Name of the member to tween.
		/// </summary>

		public string Name
		{
			get => name;
			set => name = value;
		}

		/// <summary>
		/// The member's type.
		/// </summary>

		public string Type
		{
			get => type;
			set => type = value;
		}

		/// <summary>
		/// Value to tween the member to.
		/// </summary>

		public string Value
		{
			get => value;
			set => this.value = value;
		}

		public object ParseValue()
		{
			//Type type = System.Type.GetType(this.type);

			//typeof(int).AssemblyQualifiedName.Contains(type);

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

			throw new Exception($"Object type ({type}) not supported.");
		}
	}
}