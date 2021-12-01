using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwinSouls.UI
{
    public class JoinGroupCanvas : MonoBehaviour
    {
		private void Awake()
		{
			StageManager.OnPlayerSpawnedEvt += StageManager_OnPlayerSpawnedEvt;
		}

		private void StageManager_OnPlayerSpawnedEvt(GameObject obj)
		{
			gameObject.SetActive(StageManager.Instance.PlayerNumber < 2);
		}
	}
}