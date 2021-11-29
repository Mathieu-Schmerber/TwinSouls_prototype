using System.Collections;
using System.Collections.Generic;
using TwinSouls.Tools;
using UnityEngine;

namespace TwinSouls.Spells
{
	public class FreezeFx : MonoBehaviour
	{
		[SerializeField] private GameObject _iceblockPrefab;
		[SerializeField] private Vector3 _targetedScale;
		[SerializeField] private float _apparitionSpeed;

		private GameObject[] _iceblockInstances = new GameObject[2];

		private void Awake()
		{
			FXTimedDestruction.OnSmoothDestroyEvt += FXTimedDestruction_OnSmoothDestroyEvt;
		}

		private void OnDestroy()
		{
			FXTimedDestruction.OnSmoothDestroyEvt -= FXTimedDestruction_OnSmoothDestroyEvt;
		}

		private void Start()
		{
			_iceblockInstances[0] = Instantiate(_iceblockPrefab, transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0), transform);
			_iceblockInstances[1] = Instantiate(_iceblockPrefab, transform.position + new Vector3(0, _targetedScale.y / 2, 0), Quaternion.Euler(0, Random.Range(0, 360), 0), transform);

			foreach (GameObject iceblock in _iceblockInstances)
			{
				iceblock.transform.localScale = Vector3.zero;
				iTween.ScaleTo(iceblock, _targetedScale, _apparitionSpeed);
			}
		}

		private void FXTimedDestruction_OnSmoothDestroyEvt(GameObject obj, float time)
		{
			if (obj == gameObject)
			{
				foreach (GameObject iceblock in _iceblockInstances)
					iTween.ScaleTo(iceblock, Vector3.zero, time);
			}
		}
	}
}