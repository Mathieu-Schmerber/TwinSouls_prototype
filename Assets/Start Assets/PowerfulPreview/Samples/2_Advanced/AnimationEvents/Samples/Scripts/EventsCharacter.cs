using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StartAssets.AnimationEvents
{
	[RequireComponent(typeof(Animator))]
	public class EventsCharacter : MonoBehaviour
	{
		public Transform burnTarget;
		public GameObject burnPrefab;

		public void IgniteTarget()
		{
			if (burnTarget != null && burnPrefab != null)
			{
				Instantiate(burnPrefab, burnTarget.position, Quaternion.Euler(Vector3.zero));
			}
		}

		private void Start()
		{
			mAnimator = GetComponent<Animator>();
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				mAnimator.SetTrigger("Kick");
			}
			else if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				mAnimator.SetTrigger("Spell");
			}
			else if (Input.GetKeyDown(KeyCode.Alpha3))
			{
				mAnimator.SetTrigger("Spell2");
			}
		}

		private Animator mAnimator;
	}
}