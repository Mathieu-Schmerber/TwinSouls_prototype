using System.Collections;
using System.Collections.Generic;
using TwinSouls.Entity;
using UnityEngine;

namespace TwinSouls.Tools
{
	public class CustomJoint : MonoBehaviour
	{
		[SerializeField] private GameObject _jointTransform;
		[SerializeField] private float _intensityFactor = 1f;
		private Damageable _damageable;
		private Vector3 _initialRotation;

		private void Awake()
		{
			_initialRotation = _jointTransform.transform.rotation.eulerAngles;
			_damageable = GetComponent<Damageable>();
			_damageable.OnDamageTakenEvt += _damageable_OnDamageTakenEvt;
		}

		private void OnDestroy()
		{
			_damageable.OnDamageTakenEvt -= _damageable_OnDamageTakenEvt;
		}

		private void _damageable_OnDamageTakenEvt(GameObject arg1, float arg2, bool arg3)
		{
			if (arg3)
				Punch(arg1, arg2);
		}

		private void Punch(GameObject collision, float force)
		{
			Vector3 axis = new Vector3(-collision.transform.forward.x, 0, -collision.transform.forward.z).normalized;
			Vector3 rotation = axis * force * _intensityFactor;

			iTween.PunchRotation(_jointTransform, new Hashtable() {
				{ "x", rotation.x }, {"y", rotation.y}, {"z", rotation.z}, 
				{"time", 1f}, {"oncomplete", nameof(ResetRotation)}, {"oncompletetarget", gameObject}
			});
		}

		public void ResetRotation()
		{
			iTween.RotateTo(_jointTransform, _initialRotation, 0.3f);
		}
	}
}