using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wondeluxe.Tweening
{
	// TODO Implement ISerializationCallbackReceiver

	/// <summary>
	/// Defines the set of members that will be affected by a <see cref="Tween"/>.
	/// </summary>

	[Serializable]
	public class TweenMembers : IEnumerable<TweenMember>
	{
		/// <summary>
		/// Names of the members to tween.
		/// </summary>

		//[OnModified("OnModified")]
		[SerializeField]
		private List<string> names = new List<string>();

		/// <summary>
		/// Types of the members to tween, required for deserialization.
		/// </summary>

		[SerializeField]
		private List<string> serializedTypes;

		/// <summary>
		/// String representations of the values to tween to.
		/// Values are serialized as strings as it's the simplest way to store variable data types.
		/// </summary>

		[SerializeField]
		private List<string> serializedValues;

		/// <summary>
		/// Values to tween to.
		/// </summary>

		private readonly List<object> values = new List<object>();

		/// <summary>
		/// Set to true when the set has been modified. Owning tween will reset to false.
		/// </summary>

		[SerializeField]
		private bool dirty;

		// TODO Change constructor to take IEnumerable<TweenMember>

		/// <summary>
		/// Initializes a new instance of TweenMembers that contains the members copied from an enumerable collection of <c>TweenMember</c>.
		/// </summary>
		/// <param name="members"></param>

		public TweenMembers(IEnumerable<KeyValuePair<string, object>> members)
		{
			foreach (KeyValuePair<string, object> member in members)
			{
				names.Add(member.Key);
				values.Add(member.Value);
			}

			dirty = true;
		}

		// TODO Might be better to throw custom exception for invalid indexers?

		public object this[string name]
		{
			get => values[names.IndexOf(name)];
			set
			{
				values[names.IndexOf(name)] = value;
				dirty = true;
			}
		}

		/// <summary>
		/// Indicates if the set of members to be tweened has been modified.
		/// </summary>

		internal bool Dirty
		{
			get => dirty;
			set => dirty = value;
		}

		/// <summary>
		/// Add the specified name and value to the set of members to be tweened.
		/// </summary>
		/// <param name="name">Name of the member to tween.</param>
		/// <param name="value">Value to tween to.</param>

		public void Add(string name, object value)
		{
			if (names.Contains(name))
			{
				throw new ArgumentException($"A member with name '{name}' already exists.");
			}

			names.Add(name);
			values.Add(value);
			dirty = true;
		}

		/// <summary>
		/// Attempts to add the specified name and value to the set of members to be tweened.
		/// </summary>
		/// <param name="name">The name of the member to tween.</param>
		/// <param name="value">The value to tween to.</param>
		/// <returns><c>true</c> if the name/value was added; otherwise, if a member with the specified name is already present in the set, <c>false</c>.</returns>

		public bool TryAdd(string name, object value)
		{
			if (names.Contains(name))
			{
				return false;
			}

			names.Add(name);
			values.Add(value);
			dirty = true;

			return true;
		}

		/// <summary>
		/// Removes all names/values from the set.
		/// </summary>

		public void Clear()
		{
			names.Clear();
			values.Clear();
			dirty = true;
		}

		/// <summary>
		/// Evaluate whether a member with a specified name is contained in the set.
		/// </summary>
		/// <param name="name">The name of the member to locate.</param>
		/// <returns><c>true</c> if a member with the specified name is contained in the set; otherwise <c>false</c>.</returns>

		public bool Contains(string name)
		{
			return names.Contains(name);
		}

		/// <summary>
		/// Remove a member with a specified name from the set of members to be tweened.
		/// </summary>
		/// <param name="name">Name of the member to remove.</param>
		/// <returns><c>true</c> if a member with the specified name was removed; otherwise, if the set doesn't contain any member with the specified name, <c>false</c>.</returns>

		public bool Remove(string name)
		{
			int index = names.IndexOf(name);

			if (index < 0)
			{
				return false;
			}

			names.RemoveAt(index);
			values.RemoveAt(index);
			dirty = true;

			return true;
		}

		/// <summary>
		/// Get the value a member will be tweened to.
		/// </summary>
		/// <typeparam name="T">The type of the value to be tweened.</typeparam>
		/// <param name="name">The name of the member to tween.</param>
		/// <returns>The value the member will be tweened to.</returns>

		public T GetValue<T>(string name)
		{
			return (T)values[names.IndexOf(name)];
		}

		/// <summary>
		/// Attempts to get the value a member will be tweened to.
		/// </summary>
		/// <typeparam name="T">The type of the value to be tweened.</typeparam>
		/// <param name="name">The name of the member to tween.</param>
		/// <param name="value">When this method returns, contains the value the member with the specified name will be tweened to; if no member with the specified name is found, the default value for the type <typeparamref name="T"/> will be used.</param>
		/// <returns><c>true</c> if a member with the specified name is contained in the set, otherwise <c>false</c>.</returns>

		public bool TryGetValue<T>(string name, out T value)
		{
			int index = names.IndexOf(name);

			if (index < 0)
			{
				value = default;
				return false;
			}

			value = (T)values[index];

			return true;
		}

		/// <summary>
		/// Invoked by the inspector when the set has been modified.
		/// </summary>

		private void OnModified()
		{
			dirty = true;
		}

		/// <summary>
		/// Returns an enumerator that iterates through the TweenMembers.
		/// </summary>
		/// <returns>A <see cref="TweenMembers.Enumerator"/> for the set.</returns>

		public IEnumerator<TweenMember> GetEnumerator()
		{
			return new Enumerator(names, values);
		}

		/// <summary>
		/// Not implemented. Throws <c>NotImplementedException</c> if invoked.
		/// </summary>

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Enumerates the members of a <see cref="TweenMembers"/>.
		/// </summary>

		public class Enumerator : IEnumerator<TweenMember>
		{
			private List<string> memberNames;
			private List<object> memberValues;
			private int position = -1;

			public Enumerator(List<string> memberNames, List<object> memberValues)
			{
				this.memberNames = memberNames;
				this.memberValues = memberValues;
			}

			public TweenMember Current
			{
				get
				{
					try
					{
						return new TweenMember(memberNames[position], memberValues[position]);
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
				return (++position < memberNames.Count);
			}

			public void Reset()
			{
				position = -1;
			}

			public void Dispose()
			{
				memberNames = null;
				memberValues = null;
			}
		}
	}
}