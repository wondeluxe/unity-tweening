using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

using Object = UnityEngine.Object;

namespace Wondeluxe.Tweening
{
	/// <summary>
	/// Object for tweening a target object's properties or fields.
	/// </summary>

	[Serializable]
	public class Tween : ISerializationCallbackReceiver
	{
		#region Events

		/// <summary>
		/// Invoked when the tween starts, after <see cref="member">Delay</see>.
		/// </summary>
		/// <remarks>
		/// Onstart is only ever invoked once during the tween's lifecycle, unless Reset is called.
		/// </remarks>

		public event TweenEventHandler OnStart;

		/// <summary>
		/// Invoked every frame members are updated (i.e. any Update call, after Delay or RepeatDelay have elapsed).
		/// </summary>

		public event TweenEventHandler OnUpdate;

		/// <summary>
		/// Invoked when a tween iteration ends.
		/// </summary>

		public event TweenEventHandler OnFinish;

		/// <summary>
		/// Invoked instead of OnStart on repeat iterations, after <see cref="member">RepeatDelay</see> elapses.
		/// </summary>

		public event TweenEventHandler OnRepeat;

		/// <summary>
		/// Invoked when the tween is complete, after all repeat iterations have played.
		/// </summary>
		/// <remarks>
		/// OnComplete is only ever invoked once during the tween's lifecycle, unless Reset is called.
		/// </remarks>

		public event TweenEventHandler OnComplete;

		#endregion

		#region Internal fields

		[SerializeField]
		[Component]
		[Label("Target")]
		private Object serializedTarget;

		private object target;

		[SerializeField]
		[OnModified("OnMembersModified", false, true)]
		private TweenMembers members;

		private bool membersProcessed;

		[SerializeField]
		[OnModified("OnDelayModified", false, true)]
		[Tooltip("Initial delay before the tween starts.")]
		private float delay;

		[SerializeField]
		[OnModified("OnDurationModified", false, true)]
		[Tooltip("Duration of an iteration of the tween, exluding Delay or RepeatDelay.")]
		private float duration;

		[SerializeField]
		[Tooltip("How many times the tween should repeat. Use a negative value to indicate the tween should repeat indefinitely. The number of times a tween iterates will be Repeat + 1 (where Repeat >= 0).")]
		private int repeat;

		[SerializeField]
		[OnModified("OnRepeatDelayModified", false, true)]
		[Tooltip("Delay before repeat iterations of the tween begin.")]
		private float repeatDelay;

		[SerializeField]
		[Tooltip("If true, every other iteration of the tween will be performed in reverse, creating a back and forth effect.")]
		private bool yoyo;

		[SerializeField]
		private AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		[SerializeField]
		[Tooltip("An optional identifier for the tween.")]
		private string tag;

		/// <summary>
		/// Time for the current iteration.
		/// </summary>

		private float currentTime;

		private float currentNormalizedDelayElapsed;

		private float currentNormalizedTime;

		/// <summary>
		/// The number of completed iterations.
		/// </summary>

		private int iterations;

		/// <summary>
		/// Should the tween's values be updated during Update?
		/// </summary>

		private bool paused;

		/// <summary>
		/// Should the tween continue to be updated?
		/// </summary>

		private bool done;

		/// <summary>
		/// Has the tween completed (finished all repeats)?
		/// </summary>

		private bool completed;

		/// <summary>
		/// Items representing the fields and properties to tween, along with their from and to values.
		/// </summary>

		private List<TweenItem> items;

		/// <summary>
		/// Set to <c>true</c> when the members object has been modified.
		/// Indicates that <see cref="items"/> should be reset during the next call to <see cref="Update(float)"/>.
		/// </summary>

		private bool itemsDirty;

		#endregion

		#region Public API

		public Tween(object target = null, TweenMembers members = null, float delay = 0f, float duration = 0f, int repeat = 0, float repeatDelay = 0f, bool yoyo = false, AnimationCurve curve = null, string tag = null)
		{
			this.target = target;
			this.members = members;
			this.delay = delay;
			this.duration = duration;
			this.repeat = repeat;
			this.repeatDelay = repeatDelay;
			this.yoyo = yoyo;
			this.curve = curve;
			this.tag = tag;
		}

		/// <summary>
		/// The target object whose values to tween.
		/// </summary>

		public object Tartet
		{
			get => target;
			set => target = value;
		}

		/// <summary>
		/// The members of Target to tween.
		/// </summary>

		public TweenMembers Members
		{
			get => members;
			set
			{
				if (members != null)
				{
					members.OnModified -= OnMembersModified;
				}

				members = value;
				OnMembersModified();
			}
		}

		/// <summary>
		/// Initial delay before the tween starts.
		/// </summary>

		public float Delay
		{
			get => delay;
			set
			{
				delay = value;
				OnDelayModified();
			}
		}

		/// <summary>
		/// Duration of an iteration of the tween, exluding Delay or RepeatDelay.
		/// </summary>

		public float Duration
		{
			get => duration;
			set
			{
				duration = value;
				OnDurationModified();
			}
		}

		/// <summary>
		/// How many times the tween should repeat. Use a negative value to indicate the tween should repeat indefinitely.
		/// </summary>
		/// <remarks>
		/// The number of times a tween iterates will be <c>Repeat + 1</c> (where <c>Repeat >= 0</c>).
		/// </remarks>

		public int Repeat
		{
			get => repeat;
			set => repeat = value;
		}

		/// <summary>
		/// Delay before repeat iterations of the tween begin.
		/// </summary>

		public float RepeatDelay
		{
			get => repeatDelay;
			set
			{
				repeatDelay = value;
				OnRepeatDelayModified();
			}
		}

		/// <summary>
		/// If <c>true</c>, every other iteration of the tween will be performed in reverse, creating a back and forth effect.
		/// </summary>

		public bool Yoyo
		{
			get => yoyo;
			set => yoyo = value;
		}

		/// <summary>
		/// The curve to use for the tween.
		/// </summary>

		public AnimationCurve Curve
		{
			get => curve;
			set => curve = value;
		}

		/// <summary>
		/// An optional identifier for the tween.
		/// </summary>

		public string Tag
		{
			get => tag;
			set => tag = value;
		}

		/// <summary>
		/// Should the tween's values be updated during Update?
		/// </summary>

		public bool Paused
		{
			get => paused;
			set => paused = value;
		}

		/// <summary>
		/// Should the tween continue to be updated?
		/// </summary>

		public bool Done
		{
			get => done;
		}

		/// <summary>
		/// Has the tween completed (finished all repeats)?
		/// </summary>

		public bool Completed
		{
			get => completed;
		}

		/// <summary>
		/// Update this tween.
		/// </summary>
		/// <param name="deltaTime">Delta time to advance the Tween by.</param>

		public void Update(float deltaTime)
		{
			if (paused || done)
			{
				return;
			}

			// TODO Handle negative deltaTime?

			// Each iteration of the loop will only spend the required deltaTime to finish the iteration.
			// If not all deltaTime is needed, then the loop will continue until it's spent or the Tween is complete.

			while (deltaTime > 0f)
			{
				float currentDelay = (iterations == 0) ? delay : repeatDelay;

				if (currentTime < currentDelay)
				{
					// For accuracy, the tween should only be advanced by the time remaining after the delay has elapsed.

					float delayDelta = Mathf.Min(currentDelay - currentTime, deltaTime);

					currentTime += delayDelta;
					deltaTime -= delayDelta;

					currentNormalizedDelayElapsed = currentTime / currentDelay;
				}

				if (currentTime == currentDelay)
				{
					if (iterations == 0)
					{
						OnStart?.Invoke(this);
					}
					else
					{
						OnRepeat?.Invoke(this);
					}
				}

				if (deltaTime > 0f)
				{
					if (!membersProcessed)
					{
						members.OnModified += OnMembersModified;
						membersProcessed = true;
						ReadMembers();
					}
					else if (itemsDirty)
					{
						ReadMembers();
					}

					// For accuracy, the tween should only be progressed by the required time to finish the iteration.

					if (duration > 0f)
					{
						float tweenTime = currentTime - currentDelay;
						float tweenDelta = Mathf.Min(duration - tweenTime, deltaTime);

						tweenTime += tweenDelta;
						currentTime += tweenDelta;
						deltaTime -= tweenDelta;

						currentNormalizedTime = Mathf.Clamp(tweenTime / duration, 0f, 1f);
					}
					else
					{
						currentNormalizedTime = 1f;
					}

					UpdateItems(currentNormalizedTime);

					OnUpdate?.Invoke(this);

					if (currentNormalizedTime >= 1f)
					{
						iterations++;

						OnFinish?.Invoke(this);

						if (repeat < 0 || iterations <= repeat)
						{
							currentTime = 0f;
							currentNormalizedTime = 0f;

							if (duration <= 0f)
							{
								deltaTime = 0f;
							}
						}
						else
						{
							deltaTime = 0f;// Ensure the loop ends.
							done = true;
							completed = true;

							OnComplete?.Invoke(this);
						}
					}
				}
			}
		}

		/// <summary>
		/// Cancel the tween.
		/// </summary>
		/// <param name="complete">If <c>true</c>, the Tween's target's values will be set to their final values.</param>

		public void Cancel(bool complete)
		{
			done = true;

			if (complete)
			{
				if (repeat > 0)
				{
					iterations = repeat;
				}

				currentTime = duration;
				currentNormalizedTime = 1f;

				UpdateItems(currentNormalizedTime);

				iterations++;
				completed = true;
			}
		}

		#endregion

		#region Internal methods

		/// <summary>
		/// Invoked when <see cref="serializedTarget"/> is modiefied from the Inspector.
		/// </summary>

		private void OnSerializedTargetModified()
		{
			// TODO Need to validate existing members.
			//if (Application.isPlaying)
			//{
			//}
		}

		/// <summary>
		/// Invoked (at runtime) when <see cref="members"/> is modified from the Inspector.
		/// </summary>

		private void OnMembersModified()
		{
			itemsDirty = true;
		}

		/// <summary>
		/// Invoked (at runtime) when <see cref="members"/> is modified from code.
		/// </summary>

		private void OnMembersModified(TweenMembers tweenMembers)
		{
			itemsDirty = true;
		}

		/// <summary>
		/// Invoked when <see cref="delay"/> is modified from the Inspector, of from the <see cref="Delay"/> setter.
		/// </summary>

		private void OnDelayModified()
		{
			if (iterations == 0 && currentNormalizedDelayElapsed <= 1f)
			{
				currentTime = delay * currentNormalizedDelayElapsed;
			}
		}

		/// <summary>
		/// Invoked when <see cref="duration"/> is modified from the Inspector, of from the <see cref="Duration"/> setter.
		/// </summary>

		private void OnDurationModified()
		{
			float currentDelay = (iterations == 0) ? delay : repeatDelay;

			if (currentTime > currentDelay)
			{
				currentTime = currentDelay + duration * currentNormalizedTime;
			}
		}

		/// <summary>
		/// Invoked when <see cref="repeatDelay"/> is modified from the Inspector, of from the <see cref="RepeatDelay"/> setter.
		/// </summary>

		private void OnRepeatDelayModified()
		{
			if (iterations > 0 && currentNormalizedDelayElapsed <= 1f)
			{
				currentTime = repeatDelay * currentNormalizedDelayElapsed;
			}
		}

		/// <summary>
		/// Initializes tween items using the <see cref="members"/> field.
		/// </summary>

		private void ReadMembers()
		{
			List<TweenItem> existingItems;

			if (items == null)
			{
				items = new List<TweenItem>();
				existingItems = new List<TweenItem>();
			}
			else
			{
				existingItems = new List<TweenItem>(items);
				items.Clear();
			}

			Type targetType = target.GetType();

			foreach (TweenMember member in members)
			{
				int itemIndex = existingItems.FindIndex((TweenItem i) => { return (i.Name == member.Name); });

				if (itemIndex > -1)
				{
					existingItems[itemIndex].Value.To = member.Value;
					items.Add(existingItems[itemIndex]);
					existingItems.RemoveAt(itemIndex);
					continue;
				}

				FieldInfo fieldInfo = targetType.GetField(member.Name);

				if (fieldInfo != null)
				{
					items.Add(new FieldItem(fieldInfo, fieldInfo.GetValue(target), member.Value));
					continue;
				}

				PropertyInfo propertyInfo = targetType.GetProperty(member.Name);

				if (propertyInfo != null)
				{
					items.Add(new PropertyItem(propertyInfo, propertyInfo.GetValue(target), member.Value));
					continue;
				}

				throw new ArgumentException($"Member '{member.Name}' not found on object of type '{targetType}'.");
			}

			itemsDirty = false;
		}

		/// <summary>
		/// Update the tweened items.
		/// </summary>
		/// <param name="normalizedTime">Normalized time of the tween.</param>

		private void UpdateItems(float normalizedTime)
		{
			bool reverse = (yoyo && iterations % 2 == 1);

			if (curve != null)
			{
				normalizedTime = curve.Evaluate(reverse ? (1f - normalizedTime) : normalizedTime);
			}
			else if (reverse)
			{
				normalizedTime = 1f - normalizedTime;
			}

			foreach (TweenItem item in items)
			{
				item.Udpate(target, normalizedTime);
			}
		}

		#endregion

		#region Serialization

		/// <summary>
		/// Invoked before Unity serializes the object. Ensure serialized fields match the runtime data. Not for general use.
		/// </summary>

		public void OnBeforeSerialize()
		{
			//Debug.Log($"Tween.OnBeforeSerialize");

			if (target is Object targetObject)
			{
				serializedTarget = targetObject;
			}
		}

		/// <summary>
		/// Invoked after Unity deserializes the object. Ensure runtime data matches the serialized fields. Not for general use.
		/// </summary>

		public void OnAfterDeserialize()
		{
			//Debug.Log($"Tween.OnAfterDeserialize");

			if (target == null)
			{
				target = serializedTarget;
				Debug.Log($"null target assigned");
			}
			else if (target is Object)
			{
				target = serializedTarget;
				Debug.Log($"Object target assigned");
			}
		}

		#endregion
	}
}