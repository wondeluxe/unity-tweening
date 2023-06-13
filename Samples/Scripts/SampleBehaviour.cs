using UnityEngine;

namespace Wondeluxe.Tweening.Samples
{
	public class SampleBehaviour : MonoBehaviour
	{
		[SerializeField]
		private Transform spriteTransform;

		[SerializeField]
		private Tween tween;

		//private void Awake()
		//{
		//}

		//private void OnEnable()
		//{
		//}

		//private void OnDisable()
		//{
		//}

		private void Start()
		{
			tween = new Tween(
				target: spriteTransform,
				members: new { position = Vector3.up, rotation = Quaternion.Euler(0f, 0f, 90f) },
				delay: 1f,
				duration: 2f,
				repeat: 5,
				yoyo: true,
				ease: SineEase.InOut,
				tag: "Awesome"
			);
			tween.OnRepeat += OnTweenRepeat;
		}

		private void Update()
		{
			tween.Update(Time.deltaTime);
		}

		private void OnTweenRepeat(Tween t)
		{
			Debug.Log($"Tween '{t.Tag}' repeat.");
		}

		[Button]
		private void TestTypes()
		{
			int intValue = 3;
			float floatValue = -17.315489652134845632136452236f;
			double doubleValue = .31597561245678128264552686543456456;
			Vector2 vector2Value = new Vector2(5.8f, -9f);
			Vector3 vector3Value = new Vector3(12f, -4.9876541683548764548f, 26f);
			Vector4 vector4Value = new Vector4(1.1f, 60.05f, -10f, 0.049f);
			Vector2Int vector2IntValue = new Vector2Int(5, -9);
			Vector3Int vector3IntValue = new Vector3Int(12, -4, 26);
			Color colorValue = new Color(0.402f, 0.084f, 0.91f, 0.5f);
			Quaternion quaternionValue = Quaternion.Euler(30.688104f, 12.072f, 180f);

			string intString = intValue.ToString();
			string floatString = floatValue.ToString("G");
			string doubleString = doubleValue.ToString("G");
			string vector2String = vector2Value.ToString("G");
			string vector3String = vector3Value.ToString("G");
			string vector4String = vector4Value.ToString("G");
			string vector2IntString = vector2IntValue.ToString();
			string vector3IntString = vector3IntValue.ToString();
			string colorString = colorValue.ToString();
			string quaternionString = quaternionValue.ToString("G");

			Debug.Log($"int (Value = {intValue}, ToString = {intString}, Parse = {int.Parse(intString)})");
			Debug.Log($"float (Value = {floatValue}, ToString = {floatString}, Parse = {float.Parse(floatString)})");
			Debug.Log($"double (Value = {doubleValue}, ToString = {doubleString}, Parse = {double.Parse(doubleString)})");
			Debug.Log($"Vector2 (Value = {vector2Value:G}, ToString = {vector2String}, Parse = {Vector2Extensions.Parse(vector2String):G})");
			Debug.Log($"Vector3 (Value = {vector3Value:G}, ToString = {vector3String}, Parse = {Vector3Extensions.Parse(vector3String):G})");
			Debug.Log($"Vector4 (Value = {vector4Value:G}, ToString = {vector4String}, Parse = {Vector4Extensions.Parse(vector4String):G})");
			Debug.Log($"Vector2Int (Value = {vector2IntValue}, ToString = {vector2IntString}, Parse = {Vector2IntExtensions.Parse(vector2IntString)})");
			Debug.Log($"Vector3Int (Value = {vector3IntValue}, ToString = {vector3IntString}, Parse = {Vector3IntExtensions.Parse(vector3IntString)})");
			Debug.Log($"Color (Value = {colorValue}, ToString = {colorString}, Parse = {ColorExtensions.Parse(colorString)})");
			Debug.Log($"Quaternion (Value = {quaternionValue:G}, ToString = {quaternionString}, Parse = {QuaternionExtensions.Parse(quaternionString):G})");
		}
	}
}