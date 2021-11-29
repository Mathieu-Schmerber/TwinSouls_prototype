using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace TwinSouls
{
    public class Stats : MonoBehaviour
    {
		#region Types

		[System.Serializable]
		public class StatLine
		{
			public float baseValue;
			public float bonusValue;
			public float temporaryValue;

			/// <summary>
			/// Total value, temporary buffs included
			/// </summary>
			public float Value { get => baseValue + bonusValue + temporaryValue; }

			/// <summary>
			/// Static value, temporary buffs excluded
			/// </summary>
			public float StaticValue { get => baseValue + bonusValue; }
		}

		#endregion

		#region Properties

		public StatLine MaxHealth;
		public StatLine Speed;
		public StatLine Defense;
		public StatLine Power;

		[SerializeField] private bool _isStunned = false;

		public bool IsStunned { get => _isStunned; set => _isStunned = value; }

		private float _currentHealth;
		public float CurrentHealth
		{
			get => _currentHealth;
			set
			{
				// Cannot be negative
				_currentHealth = Mathf.Max(0, value);
				// Cannot exceed MaxHealth
				_currentHealth = Mathf.Min(MaxHealth.Value, _currentHealth);
			}
		}

		#endregion

		#region Unity builtins

		private void Awake() => CurrentHealth = MaxHealth.Value;

		#endregion
	}
}