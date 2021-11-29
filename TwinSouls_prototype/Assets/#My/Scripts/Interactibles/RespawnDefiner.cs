using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnDefiner : MonoBehaviour
{
	[SerializeField] private Vector3 _respawnA;
	[SerializeField] private Vector3 _respawnB;

	private void OnTriggerEnter(Collider other) => StageManager.Instance.Spawner.SetRespawns(_respawnA, _respawnB);

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawLine(transform.position, _respawnA);
		Gizmos.DrawWireSphere(_respawnA, 0.1f);
		Gizmos.DrawLine(transform.position, _respawnB);
		Gizmos.DrawWireSphere(_respawnB, 0.1f);
	}
}
