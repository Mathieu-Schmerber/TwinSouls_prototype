using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TwinSouls.Data;
using TwinSouls.Tools;
using TwinSouls.Spells;

namespace TwinSouls.Entity 
{
	/// <summary>
	/// Handles weapon mechanic. <br></br>
	/// </summary>
    public class ElementalWeapon : MonoBehaviour, IElementModulable
    {
		#region Types

		#endregion

		#region Properties

		[SerializeField] private WeaponData _data;
		public WeaponData Data { get => _data; set => _data = value; }

		protected AElementProcessor _processor;
		protected EffectPool _casterPool;
		protected Collider _hitbox; // null, if the weapon should not apply on hit damage
		protected Stats _stats;
		protected AController _controller;
		protected int _lastAttackIndex = -1;
		protected ElementData.ElementType CurrentElement { get => _processor.EmittedElement; }

		/// <summary>
		/// Returns the current attack
		/// </summary>
		public WeaponData.WeaponAttack CurrentWeaponAttack
		{
			get => _data.attackCombos[_lastAttackIndex];
		}

		/// <summary>
		/// Returns the time to input between attack animations to perform a combo
		/// </summary>
		public float ComboIntervalTime { get => _data.comboIntervalTime; }

		#endregion

		#region Unity builtins

		// Get references
		private void Awake()
		{
			_processor = GetComponentInParent<AElementProcessor>();
			_hitbox = GetComponent<Collider>();
			_stats = GetComponentInParent<Stats>();
			_controller = GetComponentInParent<AController>();
			_casterPool = _stats.GetComponent<EffectPool>();
			EndHitBoxCheck();

			_processor.OnEmittedElementChangedEvt += UpdateElement;
		}

		private void OnDestroy()
		{
			_processor.OnEmittedElementChangedEvt -= UpdateElement;
		}

		#endregion

		#region Melee Hit & Combos

		/// <summary>
		/// Returns the attack animation to play
		/// </summary>
		/// <param name="continueCombo">Should it get the next combo part ?</param>
		/// <returns></returns>
		public virtual AnimationClip GetNextAttackAnimation(bool continueCombo = true)
		{
			if (_lastAttackIndex + 1 < _data.attackCombos.Count && continueCombo)
			{
				_lastAttackIndex++;
				return CurrentWeaponAttack.attackAnimation;
			}
			_lastAttackIndex = 0;
			return _data.attackCombos.First().attackAnimation;
		}

		/// <summary>
		/// Melee hit check start
		/// </summary>
		public void BeginHitboxCheck()
		{
			if (_hitbox)
				_hitbox.enabled = true;
		}

		/// <summary>
		/// Melee hit check end
		/// </summary>
		public void EndHitBoxCheck()
		{
			if (_hitbox)
				_hitbox.enabled = false;
		}

		/// <summary>
		/// Melee hit event
		/// </summary>
		/// <param name="other"></param>
		private void OnTriggerEnter(Collider other)
		{
			// Prevents friendly fire
			if (other.gameObject.layer != gameObject.layer)
				other.gameObject.GetComponent<Damageable>()?.ApplyDamage(gameObject, CurrentWeaponAttack.onHitDamage * (_stats.Power.Value / 100));
		}

		#endregion

		#region Spell casting

		/// <summary>
		/// Cast a spell if at least one exists for the given element
		/// </summary>
		public virtual void CastSpell()
		{
			if (CurrentWeaponAttack.spellToCast == null || 
				CurrentWeaponAttack.spellToCast.spellPrefab == null || 
				_processor.EmittedElement == ElementData.ElementType.NONE) return;

			GameObject spellToCast = CurrentWeaponAttack.spellToCast.spellPrefab;
			ProjectileSpell instance = GameObject.Instantiate(spellToCast, 
				_controller.transform.position + Vector3.up + _controller.GetAimNormal(), 
				Quaternion.identity).GetComponent<ProjectileSpell>();

			instance.Initialize(CurrentWeaponAttack.spellToCast.descriptor, _stats);
			instance.transform.forward = _controller.GetAimNormal();
		}

		#endregion

		/// <summary>
		/// Enhance weapon with a given element
		/// </summary>
		/// <param name="elementType"></param>
		public void UpdateElement(ElementData.ElementType elementType)
		{
			//ParticleSystem.ColorOverLifetimeModule colorOverLifetime = _particle.colorOverLifetime;

			//colorOverLifetime.enabled = true;
			//colorOverLifetime.color = DataLoader.GetElementOfType(elementType)?.color ?? DataLoader.Instance.Constants.noneColor;
		}
	}
}