using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TwinSouls.Player;
using System;
using System.Linq;
using UnityEngine.InputSystem;
using TwinSouls.Tools;
using TwinSouls.Entity;
using TwinSouls.Interactibles;
using PixelsoftGames.PixelUI;

public class StageManager : Singleton<StageManager>
{
	#region Properties

	public static event Action<GameObject> OnPlayerSpawnedEvt;
	private List<ElementDriver> _twins = new List<ElementDriver>();
    private PlayerSpawner _playerSpawner;
	private PlayerInputManager _playerInputManager;
	private UIStatBar _playerHealthBar;
	[SerializeField] private float _twinHealth = 100;

	public int PlayerNumber { get => _twins.Count; }
	public List<Transform> Players { get => _twins.Select(p => p.transform).ToList(); }
	public PlayerSpawner Spawner { get => _playerSpawner; }
	public float TwinHealth { get => _twinHealth; }

	#endregion

	private void Awake()
	{
		_playerSpawner = GetComponentInChildren<PlayerSpawner>();
		_playerInputManager = _playerSpawner.GetComponent<PlayerInputManager>();
		_playerHealthBar = GameObject.FindObjectOfType<UIStatBar>();
		_playerInputManager.onPlayerJoined += OnPlayerJoinedEvt;
	}

	private void OnPlayerJoinedEvt(PlayerInput obj)
	{
		_twins.Add(obj.GetComponent<ElementDriver>());
		_playerSpawner.SpawnPlayer(obj.gameObject, _twins.Count - 1);
		OnPlayerSpawnedEvt?.Invoke(obj.gameObject);

		obj.GetComponent<Damageable>().OnDamageTakenEvt += StageManager_OnDamageTakenEvt;
		if (_twins.Count == 2)
		{
			_twins[0].endPoint = _twins[1];
			_twins[1].endPoint = _twins[0];
		}
	}

	private void StageManager_OnDamageTakenEvt(GameObject arg1, float arg2, bool arg3)
	{
		_twinHealth -= arg2;
		_playerHealthBar.SetValue((int)_twinHealth, 100);
		if (_twinHealth <= 0)
			OnTwinDeath();
	}

	private void OnTwinDeath()
	{
		GameObject.FindObjectsOfType<Spawner>().ToList().ForEach(sp => sp.ResetSpawner());
		_twinHealth = 100;
		Spawner.RespawnPlayers(_twins[0].transform, _twins[1].transform);
		_playerHealthBar.SetValue((int)_twinHealth, 100);
	}
}
