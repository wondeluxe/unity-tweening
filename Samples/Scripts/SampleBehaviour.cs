using System.Reflection;
using UnityEngine;

namespace Wondeluxe.Tweening.Samples
{
	public class SampleBehaviour : MonoBehaviour
	{
		[SerializeField]
		private Transform spriteTransform;

		private Tween tween;

		private void Awake()
		{
		}

		private void OnEnable()
		{
		}

		private void OnDisable()
		{
		}

		private void Start()
		{
			//PropertyInfo propertyInfo = spriteTransform.GetType().GetProperty("position");
			//ITweenableMemberInfo memberInfo = propertyInfo as ITweenableMemberInfo;

			//Debug.Log($"propertyInfo = {propertyInfo}");
			//Debug.Log($"memberInfo = {memberInfo}");

			//memberInfo.SetValue(spriteTransform, Vector3.up);

			//propertyInfo.SetValue(spriteTransform, Vector3.up);

			tween = new Tween(
				target: spriteTransform,
				members: new { position = Vector3.up },
				delay: 1f,
				duration: 3f,
				repeat: -1,
				yoyo: true
			);
		}

		private void Update()
		{
			tween.Update(Time.deltaTime);
		}

		private void FixedUpdate()
		{

		}
	}
}