using System;

namespace WondeluxeEditor.Tweening
{
	internal class TweenableMemberInfo
	{
		public string Name { get; set; }
		public Type Type { get; set; }

		internal TweenableMemberInfo(string name, Type type)
		{
			Name = name;
			Type = type;
		}
	}
}