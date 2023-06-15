using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wondeluxe.Tweening
{
	/// <summary>
	/// Data store for what member a Tween should tween.
	/// </summary>

	[Serializable]
	public class SerializableTweenMembers : IEnumerable<SerializableTweenMember>
	{
		[SerializeField]
		[HideInInspector]
		private SerializableTweenMember[] members;

		public SerializableTweenMember[] Members
		{
			get => members;
		}

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

		public IEnumerator<SerializableTweenMember> GetEnumerator()
		{
			return new Enumerator(members);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Enumerates the elements of a <see cref="SerializableTweenMembers"/>.
		/// </summary>

		public class Enumerator : IEnumerator<SerializableTweenMember>
		{
			private SerializableTweenMember[] members;
			private int position = -1;

			public Enumerator(SerializableTweenMember[] members)
			{
				this.members = members;
			}

			public SerializableTweenMember Current
			{
				get
				{
					try
					{
						return members[position];
					}
					catch (IndexOutOfRangeException)
					{
						throw new InvalidOperationException();
					}
				}
			}

			object IEnumerator.Current
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public bool MoveNext()
			{
				//return (++position < members.Length);

				position++;
				return position < members.Length;
			}

			public void Reset()
			{
				position = -1;
			}

			public void Dispose()
			{
				members = null;
			}
		}
	}
}