using System;
using UnityEngine;

namespace Wondeluxe.Tweening
{
	/// <summary>
	/// Lightweight structure for handling a tween between two Quaternion values.
	/// </summary>
	/// <remarks>
	/// <see cref="QuaternionTween"/> provides efficient logic for tweening between two Quaternion values at the expense of features provided by <see cref="Tween"/>.
	/// Clients are expected to manage the update cycle of <see cref="QuaternionTween"/> and apply updates to target objects where required.
	/// </remarks>

	[Serializable]
	public struct QuaternionTween
	{
		[SerializeField]
		[Tooltip("Start value; value to tween from.")]
		private Quaternion from;

		[SerializeField]
		[Tooltip("End value; value to tween to.")]
		private Quaternion to;

		[SerializeField]
		[Tooltip("Time (unit agnostic) the tween will take to complete.")]
		private float duration;

		[SerializeField]
		[Tooltip("Curve defining how From will change to To over time.")]
		private AnimationCurve curve;

		private float time;

		/// <summary>
		/// Initialize a new QuaternionTween ready for updating.
		/// </summary>
		/// <param name="from">Start value; value to tween from.</param>
		/// <param name="to">End value; value to tween to.</param>
		/// <param name="duration">Time (unit agnostic) the tween will take to complete.</param>
		/// <param name="curve">Curve defining how <paramref name="from"/> will change to <paramref name="to"/> over time.</param>

		public QuaternionTween(Quaternion from, Quaternion to, float duration, AnimationCurve curve)
		{
			this.from = from;
			this.to = to;
			this.duration = Mathf.Max(duration, 0f);
			this.curve = curve;
			this.time = 0f;
		}

		/// <summary>
		/// Start value; value to tween from.
		/// </summary>

		public Quaternion From
		{
			get => from;
			set => from = value;
		}

		/// <summary>
		/// End value; value to tween to.
		/// </summary>

		public Quaternion To
		{
			get => to;
			set => to = value;
		}

		/// <summary>
		/// Time (unit agnostic) the tween will take to complete.
		/// </summary>

		public float Duration
		{
			get => duration;
			set
			{
				if (duration > 0f)
				{
					time = (time / duration) * Mathf.Max(value, 0f);
				}

				duration = Mathf.Max(value, 0f);
			}
		}

		/// <summary>
		/// Curve defining how <see cref="From"/> will change to <see cref="To"/> over time.
		/// </summary>

		public AnimationCurve Curve
		{
			get => curve;
			set => curve = value;
		}

		/// <summary>
		/// Current time of the tween.
		/// </summary>

		public float Time
		{
			get => time;
			set => time = Mathf.Max(value, 0f);
		}

		/// <summary>
		/// Normalized time of the tween.
		/// </summary>
		/// <remarks>
		/// A value of 0 indicates the tween is at the start (or hasn't started), 0.5 indicates the tween is halfway through, and 1 indicates the tween is at the end (completed).
		/// </remarks>

		public float NormalizedTime
		{
			get => (time / duration);
			set => time = (value < 0f) ? 0f : (value > 1f) ? duration : (duration * value);
		}

		/// <summary>
		/// Current value of the tween, between <see cref="From"/> and <see cref="To"/>.
		/// </summary>

		public Quaternion Value
		{
			get => Quaternion.Lerp(from, to, curve.Evaluate(NormalizedTime));
		}

		/// <summary>
		/// Indicates if the tween has completed (<c>Time >= Duration</c>).
		/// </summary>

		public bool Completed
		{
			get => (time >= duration);
		}

		/// <summary>
		/// Advance the tween by a specified amount of time.
		/// </summary>
		/// <param name="deltaTime">The time to advance the tween by.</param>

		public void Update(float deltaTime)
		{
			time = Mathf.Min(time + deltaTime, duration);
		}

		/// <summary>
		/// Set the <see cref="QuaternionTween"/> with new values and reset <see cref="Time"/> to 0.
		/// </summary>
		/// <param name="from">Start value; value to tween from.</param>
		/// <param name="to">End value; value to tween to.</param>
		/// <param name="duration">Time (unit agnostic) the tween will take to complete.</param>
		/// <param name="curve">Curve defining how <paramref name="from"/> will change to <paramref name="to"/> over time.</param>

		public void Set(Quaternion from, Quaternion to, float duration, AnimationCurve curve)
		{
			this.from = from;
			this.to = to;
			this.duration = Mathf.Max(duration, 0f);
			this.curve = curve;
			this.time = 0f;
		}

		/// <summary>
		/// Resets <see cref="Time"/> to 0.
		/// </summary>

		public void Reset()
		{
			time = 0f;
		}

		/// <summary>
		/// Returns a string representation of the tween.
		/// </summary>
		/// <returns>A string representation of the tween.</returns>

		public override string ToString()
		{
			return $"(From = {from.eulerAngles:G}, To = {to.eulerAngles:G}, Duration = {duration:G})";
		}
	}
}