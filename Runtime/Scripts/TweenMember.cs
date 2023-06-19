namespace Wondeluxe.Tweening
{
	/// <summary>
	/// Defines a member name and value to be tweened.
	/// </summary>

	public struct TweenMember
	{
		/// <summary>
		/// Name of the member to tween.
		/// </summary>

		public string Name { get; set; }

		/// <summary>
		/// Value to tween to.
		/// </summary>

		public object Value { get; set; }

		/// <summary>
		/// Initializes a new instance of TweenMember with the specified name and value.
		/// </summary>
		/// <param name="name">Name of the member to tween.</param>
		/// <param name="value">Value to tween to.</param>

		public TweenMember(string name, object value)
		{
			Name = name;
			Value = value;
		}
	}
}