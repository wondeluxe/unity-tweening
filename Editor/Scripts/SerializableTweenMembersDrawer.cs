using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using Wondeluxe.Tweening;

using Object = UnityEngine.Object;

namespace WondeluxeEditor.Tweening
{
	[CustomPropertyDrawer(typeof(SerializableTweenMembers))]
	public class SerializableTweenMembersDrawer : WondeluxePropertyDrawer
	{
		private readonly List<TweenableMemberInfo> validMembers = new List<TweenableMemberInfo>();
		private SerializedProperty membersProperty;
		private SerializedProperty currentProperty;
		private ReorderableList reorderableList;

		public override bool HasCustomLayout => true;

		public override float GetCustomPropertyHeight(SerializedProperty property, GUIContent label)
		{
			float height = EditorGUIUtility.singleLineHeight;

			if (property.isExpanded)
			{
				height += EditorGUIUtility.standardVerticalSpacing + GetReorderableList(property).GetHeight();
			}

			return height;
		}

		public override void OnCustomGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			// BeginProperty/EndProperty calls not required.
			// Called in WondeluxePropertyDrawer before/after OnCustomGUI.

			currentProperty = property;
			membersProperty = property.FindPropertyRelative("members");

			validMembers.Clear();

			SerializedProperty tweenProperty = property.GetParentProperty();
			Object tweenTarget = tweenProperty.FindPropertyRelative("serializedTarget").objectReferenceValue;
			Type tweenTargetType = tweenTarget.GetType();

			FieldInfo[] fieldInfos = tweenTargetType.GetFields(BindingFlags.Instance | BindingFlags.Public);

			foreach (FieldInfo fieldInfo in fieldInfos)
			{
				if (TweenUtility.IsTypeSupported(fieldInfo.FieldType))
				{
					validMembers.Add(new TweenableMemberInfo(fieldInfo.Name, fieldInfo.FieldType));
				}
			}

			PropertyInfo[] propertyInfos = tweenTargetType.GetProperties(BindingFlags.Instance | BindingFlags.Public);

			foreach (PropertyInfo propertyInfo in propertyInfos)
			{
				if (TweenUtility.IsTypeSupported(propertyInfo.PropertyType) && propertyInfo.GetGetMethod() != null && propertyInfo.GetSetMethod() != null)
				{
					validMembers.Add(new TweenableMemberInfo(propertyInfo.Name, propertyInfo.PropertyType));
				}
			}

			property.isExpanded = EditorGUIExtensions.DrawFoldout(label.text, property.isExpanded, ref position);

			if (property.isExpanded)
			{
				GetReorderableList(property).DoList(position);
			}
		}

		private ReorderableList GetReorderableList(SerializedProperty property)
		{
			// https://blog.terresquall.com/2020/03/creating-reorderable-lists-in-the-unity-inspector/
			// https://github.com/Unity-Technologies/UnityCsReference/blob/master/Editor/Mono/GUI/ReorderableList.cs

			// https://docs.unity3d.com/ScriptReference/EditorStyles.html
			// https://docs.unity3d.com/ScriptReference/GUIStyle.html
			// https://docs.unity3d.com/ScriptReference/GUIStyleState.html

			if (reorderableList == null)
			{
				reorderableList = new ReorderableList(null, property.FindPropertyRelative("members"), true, false, true, true);
				reorderableList.drawElementCallback = OnDrawListElement;
				reorderableList.elementHeightCallback = OnGetListElementHeight;
				reorderableList.onCanAddCallback = OnCanAddListElement;
				reorderableList.onAddCallback = OnAddListElement;
			}

			return reorderableList;
		}

		private float OnGetListElementHeight(int index)
		{
			return EditorGUIUtility.singleLineHeight;
		}

		private void OnDrawListElement(Rect rect, int index, bool isActive, bool isFocused)
		{
			//Debug.Log($"OnDrawListElement (rect = {rect}, index = {index})");

			// Rect needs to be shifted over so that the control doesn't overlap the element's handle.

			//rect.x += 8f;
			//rect.width -= 8f;

			SerializableTweenMembers currentTweenMembers = currentProperty.GetValue<SerializableTweenMembers>();

			Rect labelRect = new Rect(rect.x, rect.y, EditorGUIUtility.labelWidth - EditorGUIExtensions.SubLabelSpacing * 2f, rect.height);
			Rect valueRect = new Rect(rect.x + EditorGUIUtility.labelWidth, rect.y, rect.width - EditorGUIUtility.labelWidth, rect.height);

			SerializedProperty memberProperty = currentProperty.FindPropertyRelative("members").GetArrayElementAtIndex(index);
			SerializedProperty nameProperty = memberProperty.FindPropertyRelative("name");
			SerializedProperty typeProperty = memberProperty.FindPropertyRelative("type");
			SerializedProperty valueProperty = memberProperty.FindPropertyRelative("value");

			List<string> optionNames = new List<string>();
			List<Type> optionTypes = new List<Type>();
			int optionIndex = 0;

			optionNames.Add("<select>");
			optionTypes.Add(null);

			for (int i = 0; i < validMembers.Count; i++)
			{
				string validMemberName = validMembers[i].Name;
				Type validMemberType = validMembers[i].Type;

				if (validMemberName == nameProperty.stringValue)
				{
					optionIndex = optionNames.Count;
					optionNames.Add(validMemberName);
					optionTypes.Add(validMemberType);
				}
				else if (!currentTweenMembers.ContainsMember(validMemberName))
				{
					optionNames.Add(validMemberName);
					optionTypes.Add(validMemberType);
				}
			}

			optionIndex = EditorGUI.Popup(labelRect, optionIndex, optionNames.ToArray());

			if (optionIndex > 0)
			{
				string stringValue = (optionTypes[optionIndex].FullName == typeProperty.stringValue) ? valueProperty.stringValue : null;

				nameProperty.stringValue = optionNames[optionIndex];
				typeProperty.stringValue = optionTypes[optionIndex].FullName;
				valueProperty.stringValue = TweenEditorGUIUtility.ValueField(valueRect, optionTypes[optionIndex], stringValue);
			}
			else
			{
				nameProperty.stringValue = null;
				valueProperty.stringValue = null;
			}
		}

		private bool OnCanAddListElement(ReorderableList list)
		{
			return (currentProperty.FindPropertyRelative("members").arraySize < validMembers.Count);
		}

		private void OnAddListElement(ReorderableList list)
		{
			int newElementIndex = membersProperty.arraySize;

			membersProperty.InsertArrayElementAtIndex(newElementIndex);

			SerializedProperty newElementProperty = membersProperty.GetArrayElementAtIndex(newElementIndex);
			newElementProperty.FindPropertyRelative("name").stringValue = null;
			newElementProperty.FindPropertyRelative("value").stringValue = null;
		}
	}
}