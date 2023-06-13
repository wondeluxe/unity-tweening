using System;
using UnityEngine;

namespace Wondeluxe.Tweening
{
	/// <summary>
	/// Data store for what member a Tween should tween.
	/// </summary>

	[Serializable]
	public class SerializableTweenMembers
	{
		[SerializeField]
		[HideInInspector]
		private SerializableTweenMember[] members;

		/// <summary>
		/// Check if the collection of members contains a given member.
		/// </summary>
		/// <param name="name">The name of the member to check for.</param>
		/// <returns><c>true</c> if a member with the name <c>name</c> is found, <c>false</c> otherwise.</returns>

		public bool ContainsMember(string name)
		{
			if (members != null)
			{
				foreach (SerializableTweenMember member in members)
				{
					if (member.Name == name)
					{
						return true;
					}
				}
			}

			return false;
		}
	}
}