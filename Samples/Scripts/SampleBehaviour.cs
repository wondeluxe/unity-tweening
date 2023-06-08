using UnityEngine;

namespace Wondeluxe.Tweening.Samples
{
	public class SampleBehaviour : MonoBehaviour
	{
		[SerializeField]
		private Transform spriteTransform;

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
	}
}