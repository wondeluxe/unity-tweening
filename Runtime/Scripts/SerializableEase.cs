using System;
using UnityEngine;

namespace Wondeluxe.Tweening
{
	[Serializable]
	public class SerializableEase
	{
		public readonly static string[] Options = new string[]
		{
			"None",
			"Quadratic In",
			"Quadratic Out",
			"Quadratic In & Out",
			"Cubic In",
			"Cubic Out",
			"Cubic In & Out",
			"Quartic In",
			"Quartic Out",
			"Quartic In & Out",
			"Quintic In",
			"Quintic Out",
			"Quintic In & Out",
			"Sine In",
			"Sine Out",
			"Sine In & Out",
			"Circular In",
			"Circular Out",
			"Circular In & Out",
			"Exponential In",
			"Exponential Out",
			"Exponential In & Out",
			"Back In",
			"Back Out",
			"Back In & Out",
			"Bounce In",
			"Bounce Out",
			"Bounce In & Out",
			"Elastic In",
			"Elastic Out",
			"Elastic In & Out"
			//"Custom"// TODO Implement.
		};

		public readonly static Ease[] Values = new Ease[]
		{
			QuadraticEase.In,
			QuadraticEase.Out,
			QuadraticEase.InOut,
			CubicEase.In,
			CubicEase.Out,
			CubicEase.InOut,
			QuarticEase.In,
			QuarticEase.Out,
			QuarticEase.InOut,
			QuinticEase.In,
			QuinticEase.Out,
			QuinticEase.InOut,
			SineEase.In,
			SineEase.Out,
			SineEase.InOut,
			CircularEase.In,
			CircularEase.Out,
			CircularEase.InOut,
			ExponentialEase.In,
			ExponentialEase.Out,
			ExponentialEase.InOut,
			BackEase.In,
			BackEase.Out,
			BackEase.InOut,
			BounceEase.In,
			BounceEase.Out,
			BounceEase.InOut,
			ElasticEase.In,
			ElasticEase.Out,
			ElasticEase.InOut
		};

		[SerializeField]
		private string option;

		public int OptionIndex
		{
			get
			{
				for (int i = 1; i < Options.Length; i++)
				{
					if (option == Options[i])
					{
						return i;
					}
				}

				return 0;
			}
		}

		public void SetOption(int index)
		{
			option = Options[index];
		}

		public Ease GetValue()
		{
			int index = OptionIndex;

			if (index > 0)
			{
				return Values[index - 1];
			}

			return null;
		}

		public void SetValue(Ease ease)
		{
			for (int i = 0; i < Values.Length; i++)
			{
				if (ease == Values[i])
				{
					option = Options[i + 1];
					return;
				}
			}

			option = Options[0];
		}
	}
}