using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using Wondeluxe.Tweening;

namespace WondeluxeEditor.Tweening
{
	[CustomPropertyDrawer(typeof(SerializableTweenMembers))]
	public class SerializableTweenMembersDrawer : WondeluxePropertyDrawer
	{
		#region Internal fields

		private readonly List<TweenableMemberInfo> validMemberInfos = new List<TweenableMemberInfo>();
		private SerializedProperty currentProperty;
		private ReorderableList reorderableList;

		#endregion

		#region WondeluxePropertyDrawer implementation

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

			SetValidMemberInfos();
			ValidateMembers();

			property.isExpanded = EditorGUIExtensions.DrawFoldout(label.text, property.isExpanded, ref position);

			if (property.isExpanded)
			{
				GetReorderableList(property).DoList(EditorGUI.IndentedRect(position));
			}
		}

		#endregion

		#region ReorderableList implementation

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
			SerializableTweenMembers currentTweenMembers = currentProperty.GetValue<SerializableTweenMembers>();

			float labelWidth = EditorGUIUtility.labelWidth - 20f;
			float valueSpacing = EditorGUIExtensions.SubLabelSpacing;

			Rect labelRect = new Rect(rect.x, rect.y, labelWidth - valueSpacing, rect.height);
			Rect valueRect = new Rect(rect.x + labelWidth, rect.y, rect.width - labelWidth, rect.height);

			SerializedProperty memberProperty = currentProperty.FindPropertyRelative("members").GetArrayElementAtIndex(index);
			SerializedProperty nameProperty = memberProperty.FindPropertyRelative("name");
			SerializedProperty typeProperty = memberProperty.FindPropertyRelative("type");
			SerializedProperty valueProperty = memberProperty.FindPropertyRelative("value");

			List<string> optionNames = new List<string>();
			List<Type> optionTypes = new List<Type>();
			int optionIndex = 0;

			optionNames.Add("<Select Member>");
			optionTypes.Add(null);

			for (int i = 0; i < validMemberInfos.Count; i++)
			{
				string validMemberName = validMemberInfos[i].Name;
				Type validMemberType = validMemberInfos[i].Type;

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
			return (currentProperty.FindPropertyRelative("members").arraySize < validMemberInfos.Count);
		}

		private void OnAddListElement(ReorderableList list)
		{
			SerializedProperty membersProperty = currentProperty.FindPropertyRelative("members");

			int newElementIndex = membersProperty.arraySize;

			membersProperty.InsertArrayElementAtIndex(newElementIndex);

			SerializedProperty newElementProperty = membersProperty.GetArrayElementAtIndex(newElementIndex);
			newElementProperty.FindPropertyRelative("name").stringValue = null;
			newElementProperty.FindPropertyRelative("type").stringValue = null;
			newElementProperty.FindPropertyRelative("value").stringValue = null;
		}

		#endregion

		#region Internal methods

		private void SetValidMemberInfos()
		{
			validMemberInfos.Clear();

			SerializedProperty tweenProperty = currentProperty.GetParentProperty();
			SerializedProperty targetProperty = tweenProperty.FindPropertyRelative("serializedTarget");
			Type type = targetProperty.objectReferenceValue.GetType();

			FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Instance | BindingFlags.Public);

			foreach (FieldInfo fieldInfo in fieldInfos)
			{
				if (TweenUtility.IsTypeSupported(fieldInfo.FieldType))
				{
					validMemberInfos.Add(new TweenableMemberInfo(fieldInfo.Name, fieldInfo.FieldType));
				}
			}

			PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

			foreach (PropertyInfo propertyInfo in propertyInfos)
			{
				if (TweenUtility.IsTypeSupported(propertyInfo.PropertyType) && propertyInfo.GetGetMethod() != null && propertyInfo.GetSetMethod() != null)
				{
					validMemberInfos.Add(new TweenableMemberInfo(propertyInfo.Name, propertyInfo.PropertyType));
				}
			}
		}

		private void ValidateMembers()
		{
			SerializedProperty membersProperty = currentProperty.FindPropertyRelative("members");

			for (int i = 0; i < membersProperty.arraySize; )
			{
				if (ValidateMember(membersProperty.GetArrayElementAtIndex(i)))
				{
					i++;
				}
				else
				{
					membersProperty.DeleteArrayElementAtIndex(i);
				}
			}
		}

		private bool ValidateMember(SerializedProperty memberProperty)
		{
			string nameValue = memberProperty.FindPropertyRelative("name").stringValue;

			if (string.IsNullOrEmpty(nameValue))
			{
				return true;
			}

			string typeValue = memberProperty.FindPropertyRelative("type").stringValue;

			foreach (TweenableMemberInfo memberInfo in validMemberInfos)
			{
				if (nameValue == memberInfo.Name && typeValue == memberInfo.Type.FullName)
				{
					return true;
				}
			}

			return false;
		}

		#endregion
	}
}