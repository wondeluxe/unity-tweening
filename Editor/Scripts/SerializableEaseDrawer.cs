using UnityEngine;
using UnityEditor;
using Wondeluxe.Tweening;

namespace WondeluxeEditor.Tweening
{
	[CustomPropertyDrawer(typeof(SerializableEase))]
	public class SerializableEaseDrawer : WondeluxePropertyDrawer
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

			// TODO Fix assignment issue in the inspector.

			SerializableEase serializableEase = (SerializableEase)property.GetValue();

			int index = serializableEase.OptionIndex;

			//Debug.Log($"index = {index}");

			index = EditorGUIExtensions.DrawOptionsField(label.text, index, SerializableEase.Options, ref position);

			//serializableEase.SetOption(index);
			//property.SetValue(serializableEase);

			property.FindPropertyRelative("option").stringValue = SerializableEase.Options[index];
		}
	}
}