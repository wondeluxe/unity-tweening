using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using Wondeluxe.Tweening;

namespace WondeluxeEditor.Tweening
{
	[CustomPropertyDrawer(typeof(TweenMembers))]
	public class TweenMembersDrawer : WondeluxePropertyDrawer
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

			ChildrenHandled(property);
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
				reorderableList = new ReorderableList(null, property.FindPropertyRelative("serializedNames"), true, false, true, true);
				reorderableList.elementHeightCallback = OnGetListElementHeight;
				reorderableList.drawElementCallback = OnDrawListElement;
				reorderableList.onCanAddCallback = OnCanAddListElement;
				reorderableList.onAddCallback = OnAddListElement;
				reorderableList.onRemoveCallback = OnRemoveListElement;
				reorderableList.onReorderCallbackWithDetails = OnReorderListElement;
			}

			return reorderableList;
		}

		private float OnGetListElementHeight(int index)
		{
			return EditorGUIUtility.singleLineHeight;
		}

		private void OnDrawListElement(Rect rect, int index, bool isActive, bool isFocused)
		{
			TweenMembers currentTweenMembers = currentProperty.GetValue<TweenMembers>();

			SerializedProperty memberNameProperty = currentProperty.FindPropertyRelative("serializedNames").GetArrayElementAtIndex(index);
			SerializedProperty memberTypeProperty = currentProperty.FindPropertyRelative("serializedTypes").GetArrayElementAtIndex(index);
			SerializedProperty memberValueProperty = currentProperty.FindPropertyRelative("serializedValues").GetArrayElementAtIndex(index);

			List<string> optionNames = new List<string>();
			List<Type> optionTypes = new List<Type>();
			int optionIndex = 0;

			optionNames.Add("<Select Member>");
			optionTypes.Add(null);

			for (int i = 0; i < validMemberInfos.Count; i++)
			{
				string validMemberName = validMemberInfos[i].Name;
				Type validMemberType = validMemberInfos[i].Type;

				if (validMemberName == memberNameProperty.stringValue)
				{
					optionIndex = optionNames.Count;
					optionNames.Add(validMemberName);
					optionTypes.Add(validMemberType);
				}
				else if (!currentTweenMembers.Contains(validMemberName))
				{
					optionNames.Add(validMemberName);
					optionTypes.Add(validMemberType);
				}
			}

			float labelWidth = EditorGUIUtility.labelWidth - 20f;
			float valueSpacing = EditorGUIExtensions.SubLabelSpacing;

			Rect labelRect = new Rect(rect.x, rect.y, labelWidth - valueSpacing, rect.height);

			optionIndex = EditorGUI.Popup(labelRect, optionIndex, optionNames.ToArray());

			if (optionIndex > 0)
			{
				Rect valueRect = new Rect(rect.x + labelWidth, rect.y, rect.width - labelWidth, rect.height);

				string stringValue = (optionTypes[optionIndex].FullName == memberTypeProperty.stringValue) ? memberValueProperty.stringValue : null;

				memberNameProperty.stringValue = optionNames[optionIndex];
				memberTypeProperty.stringValue = optionTypes[optionIndex].FullName;
				memberValueProperty.stringValue = TweenEditorGUIUtility.ValueField(valueRect, optionTypes[optionIndex], stringValue);
			}
			else
			{
				memberNameProperty.stringValue = null;
				memberValueProperty.stringValue = null;
			}
		}

		private bool OnCanAddListElement(ReorderableList list)
		{
			return (currentProperty.FindPropertyRelative("serializedNames").arraySize < validMemberInfos.Count);
		}

		private void OnAddListElement(ReorderableList list)
		{
			SerializedProperty memberNamesProperty = currentProperty.FindPropertyRelative("serializedNames");
			SerializedProperty memberTypesProperty = currentProperty.FindPropertyRelative("serializedTypes");
			SerializedProperty memberValuesProperty = currentProperty.FindPropertyRelative("serializedValues");

			int newElementIndex = memberNamesProperty.arraySize;

			memberNamesProperty.InsertArrayElementAtIndex(newElementIndex);
			memberTypesProperty.InsertArrayElementAtIndex(newElementIndex);
			memberValuesProperty.InsertArrayElementAtIndex(newElementIndex);

			memberNamesProperty.GetArrayElementAtIndex(newElementIndex).stringValue = null;
			memberTypesProperty.GetArrayElementAtIndex(newElementIndex).stringValue = null;
			memberValuesProperty.GetArrayElementAtIndex(newElementIndex).stringValue = null;
		}

		private void OnRemoveListElement(ReorderableList list)
		{
			int index = list.index;

			currentProperty.FindPropertyRelative("serializedNames").DeleteArrayElementAtIndex(index);
			currentProperty.FindPropertyRelative("serializedTypes").DeleteArrayElementAtIndex(index);
			currentProperty.FindPropertyRelative("serializedValues").DeleteArrayElementAtIndex(index);
		}

		private void OnReorderListElement(ReorderableList list, int oldIndex, int newIndex)
		{
			currentProperty.FindPropertyRelative("serializedNames").MoveArrayElement(oldIndex, newIndex);
			currentProperty.FindPropertyRelative("serializedTypes").MoveArrayElement(oldIndex, newIndex);
			currentProperty.FindPropertyRelative("serializedValues").MoveArrayElement(oldIndex, newIndex);
		}

		#endregion

		#region Internal methods

		private void SetValidMemberInfos()
		{
			validMemberInfos.Clear();

			SerializedProperty tweenProperty = currentProperty.GetParentProperty();
			SerializedProperty targetProperty = tweenProperty.FindPropertyRelative("serializedTarget");

			Type type;

			if (targetProperty.objectReferenceValue != null)
			{
				type = targetProperty.objectReferenceValue.GetType();
			}
			else
			{
				Tween tween = tweenProperty.GetValue<Tween>();

				if (tween.Tartet == null)
				{
					// TODO Need to display info that target must be set.
					return;
				}

				type = tween.Tartet.GetType();
			}

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
			SerializedProperty memberNamesProperty = currentProperty.FindPropertyRelative("serializedNames");
			SerializedProperty memberTypesProperty = currentProperty.FindPropertyRelative("serializedTypes");
			SerializedProperty memberValuesProperty = currentProperty.FindPropertyRelative("serializedValues");

			for (int i = 0; i < memberNamesProperty.arraySize;)
			{
				string name = memberNamesProperty.GetArrayElementAtIndex(i).stringValue;
				string type = memberTypesProperty.GetArrayElementAtIndex(i).stringValue;

				if (ValidateMember(name, type))
				{
					i++;
				}
				else
				{
					memberNamesProperty.DeleteArrayElementAtIndex(i);
					memberTypesProperty.DeleteArrayElementAtIndex(i);
					memberValuesProperty.DeleteArrayElementAtIndex(i);
				}
			}
		}

		private bool ValidateMember(string name, string type)
		{
			if (string.IsNullOrEmpty(name))
			{
				return true;
			}

			foreach (TweenableMemberInfo memberInfo in validMemberInfos)
			{
				//if (name == memberInfo.Name && memberInfo.Type.AssemblyQualifiedName.Contains(type))
				if (name == memberInfo.Name && type == memberInfo.Type.FullName)
				{
					return true;
				}
			}

			return false;
		}

		#endregion
	}
}