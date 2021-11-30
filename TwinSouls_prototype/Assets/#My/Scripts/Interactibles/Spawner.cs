using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TwinSouls.Tools;
using System;
using Random = UnityEngine.Random;
using System.Linq;
using TwinSouls.Entity;
using TwinSouls.Spells;

namespace TwinSouls.Interactibles
{
	public class Spawner : Activatable
	{
		#region Properties

		#region Types

		[System.Serializable]
		public struct Area
		{
			public Vector3 center;
			public float radius;
		}

		[System.Serializable]
		public struct Wave
		{
			public int totalCapacity;
			[LabelText("Spawn / second")] public int spawnPerSecond;
		}

		#endregion

		public static event Action<int, int> OnWaveStartEvt;
		public static event Action OnSpawnerClearedEvt;

		[SerializeField] private Activatable _activationDevice;
		[SerializeField] private Area[] _spawnableAreas;
		[SerializeField] private Wave[] _waves;
		[SerializeField] private GameObject[] _spawnableEntities;
		[SerializeField] private Door[] _controlledDoors;

		private Cooldown _spawnCd;
		private int _currentWaveIndex = 0;
		[ReadOnly] public int _currentPopulation = 0;
		[ReadOnly] public int _totalSpawned = 0;

		private int WaveDisplayNumber => _currentWaveIndex + 1;

		public Area[] SpawnableAreas { get => _spawnableAreas; }

		#endregion

		#region Unity Builtins

		private void Awake()
		{
			_activationDevice.OnActivatedEvt += StartSpawnCycle;
		}

		private void OnDestroy()
		{
			_activationDevice.OnActivatedEvt -= StartSpawnCycle;
		}

		private void Update() => _spawnCd?.Run();

		#endregion

		private void StartSpawnCycle()
		{
			if (IsActive) return;

			_spawnCd = new Cooldown()
			{
				cooldownTime = 1f,
				automaticReset = true,
				readyOnStart = false
			};
			_spawnCd.Init();
			_spawnCd.IsOverEvent += SpawnEntities;
			OnWaveStartEvt?.Invoke(WaveDisplayNumber, _waves.Length);
			foreach (Door item in _controlledDoors)
				item.Close();
		}

		public Vector3 GetRandomSpawnablePosition()
		{
			Area spawn = _spawnableAreas.Random();
			Vector3 spawnOffset = new Vector3(Random.insideUnitCircle.x, 0, Random.insideUnitCircle.y) * spawn.radius;

			return spawn.center + spawnOffset + transform.position;
		}

		private void SpawnEntities(Cooldown actor)
		{
			Wave wave = _waves[_currentWaveIndex];

			for (int i = 0; i < wave.spawnPerSecond; i++)
			{
				if (_totalSpawned >= wave.totalCapacity) break;

				Vector3 spawnPos = GetRandomSpawnablePosition();
				GameObject instance = Instantiate(_spawnableEntities.Random(), spawnPos, Quaternion.identity, transform);

				instance.GetComponent<Damageable>().OnDeathEvt += (killer) => CheckWaveStatus();
				instance.GetComponentInChildren<WeaponHolder>().EquipWeapon(DataLoader.Instance.Weapons.ToList().Random());
				instance.GetComponent<AElementProcessor>().StartElement = DataLoader.Instance.Elements.ToList().Random().type;

				_totalSpawned++;
				_currentPopulation++;
			}
		}

		private void CheckWaveStatus()
		{
			Wave wave = _waves[_currentWaveIndex];

			_currentPopulation--;
			if (_currentPopulation == 0 && _totalSpawned == wave.totalCapacity)
			{
				_currentWaveIndex++;
				if (_currentWaveIndex >= _waves.Length)
					OnStateChanged();
				else
				{
					_totalSpawned = 0;
					_currentPopulation = 0;
					OnWaveStartEvt?.Invoke(WaveDisplayNumber, _waves.Length);
					_spawnCd.Reset();
				}
			}
		}

		protected override void OnStateChanged()
		{
			_spawnCd.IsOverEvent -= SpawnEntities;
			_spawnCd = null;
			SetActive(true);
			OnSpawnerClearedEvt?.Invoke();
			foreach (Door item in _controlledDoors)
				item.Open();
		}

		public void ResetSpawner()
		{
			foreach (Door item in _controlledDoors)
				item.OnStateChanged(); // Open the front door, keep the back door closed
			_totalSpawned = 0;
			_currentPopulation = 0;
			_currentWaveIndex = 0;
			_spawnCd = null;
			Transform[] children = GetComponentsInChildren<Transform>();
			foreach (Transform item in children)
			{
				if (item != transform)
					Destroy(item.gameObject);
			}
		}

		private void OnDrawGizmos()
		{
			if (_spawnableAreas?.Length > 0)
			{
				Gizmos.color = Color.red;
				foreach (Area item in _spawnableAreas)
					Gizmos.DrawWireSphere(transform.position + item.center, item.radius);
			}
		}
	}
}