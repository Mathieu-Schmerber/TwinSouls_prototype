using System;
using System.Collections;
using System.Collections.Generic;
using TwinSouls.Data;
using TwinSouls.Entity;
using TwinSouls.Interactibles;
using UnityEngine;

namespace TwinSouls.Player
{
    public class PlayerWeaponHolder : WeaponHolder
    {
		private AController _mainParent;
		private InputHandler _inputs;

		public event Action<WeaponData> OnSuggestionChangedEvt;

		private WeaponItem _suggestion;
		public WeaponItem Suggestion
		{
			get => _suggestion; set
			{
				if (value != _suggestion)
					OnSuggestionChangedEvt?.Invoke(value == null ? null : value.Data);
				_suggestion = value;
			}
		}

		protected override void Awake()
		{
			base.Awake();
			_inputs = GetComponentInParent<InputHandler>();
			_mainParent = GetComponentInParent<AController>();

			_inputs.OnInteractInputEvt += PickupSuggestion;
		}

		private void OnDestroy()
		{
			_inputs.OnInteractInputEvt -= PickupSuggestion;
		}

		private void PickupSuggestion()
		{
			if (Suggestion != null)
			{
				EquipWeapon(Suggestion.Data);
				Destroy(Suggestion.gameObject);
				Suggestion = null;
			}
		}

		public void SuggestWeapon(WeaponItem data) => Suggestion = data;

		public void UnSuggestWeapon(WeaponItem data)
		{
			if (Suggestion == data)
				Suggestion = null;
		}

		/// <summary>
		/// Equips a weapon.
		/// </summary>
		/// <param name="data"></param>
		public override void EquipWeapon(WeaponData data)
		{
			DropCurrentlyEquipped(_weaponScript.Data);
			base.EquipWeapon(data);
		}

		private void DropCurrentlyEquipped(WeaponData data)
		{
			WeaponItem.Spawn(data, _mainParent.transform.position);
		}
	}
}