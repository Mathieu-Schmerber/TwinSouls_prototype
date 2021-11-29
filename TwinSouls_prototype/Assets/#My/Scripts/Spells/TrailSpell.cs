using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using TwinSouls.Tools;



namespace TwinSouls.Spells 
{
	/// <summary>
	/// Component that is beiing attached to the player dynamically.<br></br>
	/// Can emit spells from the player position given time intervals.
	/// </summary>
    public class TrailSpell : ASpell<ASpellDescriptor>
    {
		#region Properties

		private GameObject _spellAreaPrefab;
		private Cooldown _spawnAreaCdr;
		private Stats _stats;
		private Rigidbody _rb;

		#endregion

		#region Unity builtins

		protected override void Awake()
		{
			_stats = GetComponent<Stats>();
			_rb = GetComponent<Rigidbody>();
		}

		private void OnDisable()
		{
			_spawnAreaCdr.IsOverEvent -= _spawnAreaCdr_IsOverEvent;
		}

		private void Update()
		{
			if (_spawnAreaCdr != null)
				_spawnAreaCdr.Run();
		}

		#endregion

		public void StartFollowing(GameObject spellAreaPrefab, float spawnInterval)
		{
			_spellAreaPrefab = spellAreaPrefab;
			_spawnAreaCdr = new Cooldown()
			{
				readyOnStart = true,
				automaticReset = true,
				cooldownTime = spawnInterval
			};
			_spawnAreaCdr.IsOverEvent += _spawnAreaCdr_IsOverEvent;
			_spawnAreaCdr.Init();
		}

		private void _spawnAreaCdr_IsOverEvent(Cooldown actor)
		{
			AreaSpell[] spells = Instantiate(_spellAreaPrefab, transform.position, Quaternion.Euler(90, 0, 0)).GetComponents<AreaSpell>();

			foreach (AreaSpell spell in spells)
				spell.CastArea(_stats);
		}
	}
}