using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wondeluxe.Tweening
{
	/// <summary>
	/// Defines the set of members that will be affected by a <see cref="Tween"/>.
	/// </summary>

	[Serializable]
	public class TweenMembers : IEnumerable<TweenMember>, ISerializationCallbackReceiver
	{
		/// <summary>
		/// Dispatched (at runtime) when the set of members has been modified.
		/// </summary>

		internal event TweenMembersEventHandler OnModified;

		/// <summary>
		/// Names of the members to tween.
		/// </summary>

		[SerializeField]
		private List<string> serializedNames = new List<string>();

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
		/// Names of the members to tween.
		/// </summary>

		private List<string> names;

		/// <summary>
		/// Values to tween the members to.
		/// </summary>

		private List<object> values;

		/// <summary>
		/// Initializes a new instance of <see cref="TweenMembers"/> that is empty.
		/// </summary>

		public TweenMembers()
		{
			names = new List<string>();
			values = new List<object>();
		}

		/// <summary>
		/// Initializes a new instance of <see cref="TweenMembers"/> that contains the members copied from an enumerable collection of <see cref="TweenMember"/>.
		/// </summary>
		/// <param name="members">The enumerable collection of <see cref="TweenMember"/> that is copied to the the new <see cref="TweenMembers"/>.</param>

		public TweenMembers(IEnumerable<TweenMember> members) : this()
		{
			foreach (TweenMember member in members)
			{
				names.Add(member.Name);
				values.Add(member.Value);
			}
		}

		/// <summary>
		/// Indexer allowing client code to access a members value using [] notation.
		/// </summary>
		/// <param name="name">Name of the member whose value to access.</param>
		/// <returns>The value of the member with name <paramref name="name"/>.</returns>

		public object this[string name]
		{
			get => values[names.IndexOf(name)];
			set
			{
				values[names.IndexOf(name)] = value;
				OnModified?.Invoke(this);
			}
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
			OnModified?.Invoke(this);
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
			OnModified?.Invoke(this);

			return true;
		}

		/// <summary>
		/// Removes all names/values from the set.
		/// </summary>

		public void Clear()
		{
			names.Clear();
			values.Clear();
			OnModified?.Invoke(this);
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
			OnModified?.Invoke(this);

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
		/// Invoked before Unity serializes the object. Ensure serialized fields match the runtime data. Not for general use.
		/// </summary>

		public void OnBeforeSerialize()
		{
			if (names == null || values == null)
			{
				return;
			}

			if (serializedNames == null)
			{
				serializedNames = new List<string>();
			}

			if (serializedTypes == null)
			{
				serializedTypes = new List<string>();
			}

			if (serializedValues == null)
			{
				serializedValues = new List<string>();
			}

			List<string> namesToProcess = new List<string>(names);
			List<object> valuesToProcess = new List<object>(values);

			int serializedIndex = 0;

			while (serializedIndex < serializedNames.Count)
			{
				if (string.IsNullOrWhiteSpace(serializedNames[serializedIndex]))
				{
					serializedIndex++;
					continue;
				}

				int processIndex = namesToProcess.IndexOf(serializedNames[serializedIndex]);

				if (processIndex < 0)
				{
					serializedNames.RemoveAt(serializedIndex);
					serializedTypes.RemoveAt(serializedIndex);
					serializedValues.RemoveAt(serializedIndex);
					continue;
				}

				serializedNames[serializedIndex] = namesToProcess[processIndex];
				serializedTypes[serializedIndex] = TweenUtility.SerializeType(valuesToProcess[processIndex].GetType());
				serializedValues[serializedIndex] = TweenUtility.SerializeValue(valuesToProcess[processIndex]);

				namesToProcess.RemoveAt(processIndex);
				valuesToProcess.RemoveAt(processIndex);

				serializedIndex++;
			}

			for (int i = 0; i < namesToProcess.Count; i++)
			{
				serializedNames.Add(namesToProcess[i]);
				serializedTypes.Add(TweenUtility.SerializeType(valuesToProcess[i].GetType()));
				serializedValues.Add(TweenUtility.SerializeValue(valuesToProcess[i]));
			}
		}

		/// <summary>
		/// Invoked after Unity deserializes the object. Ensure runtime data matches the serialized fields. Not for general use.
		/// </summary>

		public void OnAfterDeserialize()
		{
			if (serializedNames == null || serializedTypes == null || serializedValues == null)
			{
				return;
			}

			if (names == null)
			{
				names = new List<string>();
			}
			else
			{
				names.Clear();
			}

			if (values == null)
			{
				values = new List<object>();
			}
			else
			{
				values.Clear();
			}

			for (int i = 0; i < serializedNames.Count; i++)
			{
				if (!string.IsNullOrWhiteSpace(serializedNames[i]) && !string.IsNullOrWhiteSpace(serializedTypes[i]) && !string.IsNullOrWhiteSpace(serializedValues[i]))
				{
					names.Add(serializedNames[i]);
					values.Add(TweenUtility.Deserialize(serializedTypes[i], serializedValues[i]));
				}
			}
		}

		/// <summary>
		/// Returns an enumerator that iterates through the TweenMembers.
		/// </summary>
		/// <returns>A <see cref="Enumerator"/> for the set.</returns>

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