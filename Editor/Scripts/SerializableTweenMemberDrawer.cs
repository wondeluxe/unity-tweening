using UnityEngine;
using UnityEditor;
using Wondeluxe.Tweening;

namespace WondeluxeEditor.Tweening
{
	//[CustomPropertyDrawer(typeof(SerializableTweenMember))]
	public class SerializableTweenMemberDrawer : WondeluxePropertyDrawer
	{
		public override bool HasCustomLayout => true;

		public override float GetCustomPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUI.GetPropertyHeight(property, label, false);
		}

		public override void OnCustomGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			// BeginProperty/EndProperty calls not required.
			// Called in WondeluxePropertyDrawer before/after OnCustomGUI.

			SerializableEase serializableEase = (SerializableEase)property.GetValue();

			int index = serializableEase.OptionIndex;

			EditorGUIExtensions.DrawOptionsField(label.text, index, SerializableEase.Options, ref position);

			serializableEase.SetOption(index);
			property.SetValue(serializableEase);
		}
	}
}