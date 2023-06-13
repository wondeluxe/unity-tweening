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
	}
}