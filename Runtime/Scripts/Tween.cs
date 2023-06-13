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
		private SerializableTweenMember[] testMembers;

		[SerializeField]
		[Label("Members")]// TODO Need to fix this for array/list fields.
		private SerializableTweenMembers serializedMembers;
		//private SerializableTweenMember[] serializedMembers;
		private object members;

		[SerializeField]
		[Tooltip("Initial delay before the tween starts.")]
		private float delay;

		[SerializeField]
		[Tooltip("Duration of an iteration of the tween, exluding Delay or RepeatDelay.")]
		private float duration;

		[SerializeField]
		[Tooltip("How many times the tween should repeat. Use a negative value to indicate the tween should repeat indefinitely. The number of times a tween iterates will be Repeat + 1 (where Repeat >= 0).")]
		private int repeat;

		[SerializeField]
		[Tooltip("Delay before repeat iterations of the tween begin.")]
		private float repeatDelay;

		[SerializeField]
		[Tooltip("If true, every other iteration of the tween will be performed in reverse, creating a back and forth effect.")]
		private bool yoyo;

		// TODO Serialize as AnimationCurve and provide methods for setting Penner curves.

		[SerializeField]
		[Label("Ease")]
		private SerializableEase serializedEase;
		private Ease ease;

		[SerializeField]
		[Tooltip("An optional identifier for the tween.")]
		private string tag;

		/// <summary>
		/// Indicates that <c>members</c> has been modified and <c>InitItems</c> should be called before the next update.
		/// </summary>

		private bool valuesDirty;

		/// <summary>
		/// Time for the current iteration.
		/// </summary>

		private float currentTime;

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

		private readonly List<TweenItem> items = new List<TweenItem>();

		#endregion

		#region Public API

		public Tween(object target = null, object members = null, float delay = 0f, float duration = 0f, int repeat = 0, float repeatDelay = 0f, bool yoyo = false, Ease ease = null, string tag = null)
		{
			this.target = target;
			this.members = members;
			this.delay = delay;
			this.duration = duration;
			this.repeat = repeat;
			this.repeatDelay = repeatDelay;
			this.yoyo = yoyo;
			this.ease = ease;
			this.tag = tag;

			valuesDirty = (members != null);
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
		/// <remarks>
		/// Any object may be used to supply the tween's members. Reflection will be used on the given object to retrieve any fields or properties present, which will then be applied to Target during Update.
		/// The supplied object should be treated as readonly. i.e. Modifying the the fields or properties of Values after it is assigned will have no effect on the Tween.
		/// To modify the tween's members, the property should be re-assigned.
		/// </remarks>

		public object Members
		{
			get => members;
			set
			{
				members = value;
				valuesDirty = true;
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
				if (iterations == 0 && currentTime <= delay)
				{
					currentTime = currentTime / delay * value;
				}

				delay = value;
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
				float currentDelay = (iterations == 0) ? delay : repeatDelay;

				if (currentTime > currentDelay)
				{
					currentTime = currentDelay + (currentTime - currentDelay) / duration * value;
				}

				duration = value;
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
				if (iterations > 0 && currentTime <= repeatDelay)
				{
					currentTime = currentTime / repeatDelay * value;
				}

				repeatDelay = value;
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
		/// The easing function to use for the tween.
		/// </summary>

		public Ease Ease
		{
			get => ease;
			set => ease = value;
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
					if (valuesDirty)
					{
						InitItems();
					}

					// For accuracy, the tween should only be progressed by the required time to finish the iteration.

					float tweenTime = currentTime - currentDelay;
					float tweenDelta = Mathf.Min(duration - tweenTime, deltaTime);

					tweenTime += tweenDelta;
					currentTime += tweenDelta;
					deltaTime -= tweenDelta;

					float normalizedTime = Mathf.Clamp(tweenTime / duration, 0f, 1f);

					UpdateItems(normalizedTime);

					OnUpdate?.Invoke(this);

					if (normalizedTime >= 1f)
					{
						iterations++;

						OnFinish?.Invoke(this);

						if (repeat < 0 || iterations <= repeat)
						{
							currentTime = 0f;
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

				float currentDelay = (iterations == 0) ? delay : repeatDelay;
				float normalizedTime = Mathf.Clamp(currentTime - currentDelay, 0f, duration);

				UpdateItems(normalizedTime);

				iterations++;
				completed = true;
			}
		}

		#endregion

		#region Internal methods

		/// <summary>
		/// Initializes items to be tweened.
		/// </summary>

		private void InitItems()
		{
			items.Clear();

			Type targetType = target.GetType();
			Type membersType = members.GetType();
			PropertyInfo[] membersPropertyInfos = membersType.GetProperties();

			foreach (PropertyInfo memberInfo in membersPropertyInfos)
			{
				FieldInfo fieldInfo = targetType.GetField(memberInfo.Name);

				if (fieldInfo != null)
				{
					items.Add(new FieldItem(fieldInfo, fieldInfo.GetValue(target), memberInfo.GetValue(members)));
					continue;
				}

				PropertyInfo propertyInfo = targetType.GetProperty(memberInfo.Name);

				if (propertyInfo != null)
				{
					items.Add(new PropertyItem(propertyInfo, propertyInfo.GetValue(target), memberInfo.GetValue(members)));
					continue;
				}

				throw new Exception($"Member '{memberInfo.Name}' not found on object of type '{targetType}'.");
			}

			valuesDirty = false;
		}

		/// <summary>
		/// Update the tweened items.
		/// </summary>
		/// <param name="normalizedTime">Normalized time of the tween.</param>

		private void UpdateItems(float normalizedTime)
		{
			bool reverse = (yoyo && iterations % 2 == 1);

			if (ease != null)
			{
				normalizedTime = reverse ? (1f - ease(normalizedTime)) : ease(normalizedTime);
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

		public void OnBeforeSerialize()
		{
			//Debug.Log($"Before serialize.");

			//serializedTargetObject = (Object)targetObject;
		}

		public void OnAfterDeserialize()
		{
			//Debug.Log($"After serialize.");

			//targetObject = serializedTargetObject;
		}

		#endregion
	}
}