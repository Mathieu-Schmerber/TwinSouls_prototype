using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TwinSouls.Entity;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
	private Vector3[] _respawnPoints;
	private Transform[] _spawnPoints;

	private float _twinHealth;

	public float TwinHealth
	{
		get { return _twinHealth; }
		private set { _twinHealth = value; }
	}


	private void Awake()
	{
		var transforms = new HashSet<Transform>(GetComponentsInChildren<Transform>());
		transforms.Remove(transform);

		_spawnPoints = transforms.ToArray();
		SetRespawns(_spawnPoints[0].position, _spawnPoints[1].position);
	}

	/// <summary>
	/// Spawns and loads a player skin and equipment
	/// </summary>
	/// <param name="player"></param>
	/// <param name="index"></param>
	public void SpawnPlayer(GameObject player, int index)
	{
		// TODO: Load equipments & spells
		player.transform.position = _spawnPoints[index].position;
	}

	public void SetRespawns(Vector3 A, Vector3 B) => _respawnPoints = new Vector3[] { A, B };

	public void RespawnPlayers(Transform playerA, Transform playerB)
	{
		playerA.position = _respawnPoints[0];
		playerB.position = _respawnPoints[1];
	}
}
