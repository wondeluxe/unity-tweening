using System;
using UnityEngine;
using UnityEditor;
using Wondeluxe;

namespace WondeluxeEditor.Tweening
{
	public static class TweenEditorGUIUtility
	{
		public static string ValueField(Rect position, Type type, string value)
		{
			if (type == typeof(int))
				return EditorGUI.IntField(position, int.Parse(value)).ToString();

			if (type == typeof(float))
				return EditorGUI.FloatField(position, float.Parse(value)).ToString("G");

			if (type == typeof(double))
				return EditorGUI.DoubleField(position, double.Parse(value)).ToString("G");

			if (type == typeof(Vector2))
				return EditorGUIExtensions.Vector2Field(position, Vector2Extensions.Parse(value)).ToString("G");

			if (type == typeof(Vector3))
				return EditorGUIExtensions.Vector3Field(position, Vector3Extensions.Parse(value)).ToString("G");

			if (type == typeof(Vector4))
				return EditorGUIExtensions.Vector4Field(position, Vector4Extensions.Parse(value)).ToString("G");

			if (type == typeof(Vector2Int))
				return EditorGUIExtensions.Vector2IntField(position, Vector2IntExtensions.Parse(value)).ToString();

			if (type == typeof(Vector3Int))
				return EditorGUIExtensions.Vector3IntField(position, Vector3IntExtensions.Parse(value)).ToString();

			if (type == typeof(Color))
				return EditorGUI.ColorField(position, ColorExtensions.Parse(value)).ToString();

			if (type == typeof(Quaternion))
				return Quaternion.Euler(EditorGUIExtensions.Vector3Field(position, QuaternionExtensions.Parse(value).eulerAngles)).ToString("G");

			throw new Exception($"Object type ({type}) not supported.");
		}
	}
}