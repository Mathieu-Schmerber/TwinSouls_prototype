using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinSouls.Player;
using TwinSouls.Tools;
using UnityEngine;

namespace TwinSouls.Entity
{
    /// <summary>
    /// Defines the ability to attack with a weapon
    /// </summary>
	public abstract class AKit : MonoBehaviour
	{
        #region Properties

        public static event Action<GameObject, float> OnAttackAbilityStartEvt;

        protected Stats _stats;
        protected Animator _anim;
        private ElementalWeapon _weapon;

        private Cooldown _attackCooldown = new Cooldown()
        {
            readyOnStart = true,
            automaticReset = false,
            cooldownTime = 0
        };

		#endregion

		#region Unity builtins

		// Get references
		protected virtual void Awake()
        {
            _anim = GetComponentInChildren<Animator>();
            _weapon = GetComponentInChildren<ElementalWeapon>();
            _stats = GetComponent<Stats>();
        }

        // Initialization
        protected virtual void Start()
        {
            _attackCooldown.Init();
        }

		protected virtual void Update()
        {
            if (WantsToAttack())
                TryAttack();
        }

        #endregion

        /// <summary>
        /// Determines if the entity is willing to try attacking <br></br>
        /// Determined by an input beiing pressed for the player for exemple.
        /// </summary>
        /// <returns></returns>
        protected abstract bool WantsToAttack();

        protected virtual void TryAttack()
		{
            if (_attackCooldown.IsOver() && !_stats.IsStunned)
            {
                OnMeleeAttack();
                _attackCooldown.Reset();
            }
        }

        /// <summary>
		/// Check if a combo attack is possible and adapt attack cooldown to the clip's length
		/// </summary>
		protected virtual void OnMeleeAttack()
        {
            bool canCombo = (Time.time - _attackCooldown.LastOverTime - _attackCooldown.cooldownTime) <= _weapon.ComboIntervalTime;
            AnimationClip attackClip = _weapon.GetNextAttackAnimation(canCombo);

            if (attackClip == null)
                return;
            _anim.SetFloat("AttackSpeed", _weapon.Data.attackSpeed);
            _attackCooldown.SetCooldown(attackClip.length / _weapon.Data.attackSpeed);
            OnAttackAbilityStartEvt?.Invoke(gameObject, _attackCooldown.cooldownTime);
            _anim.Play(attackClip.name);
        }

        #region Animation triggers

        /// <summary>
        /// Called from the ElementDriver by the AnimationEvents class
        /// </summary>
        public virtual void OnMeleeAttackStartEvent() => _weapon.BeginHitboxCheck();

        /// <summary>
        /// Called from the ElementDriver by the AnimationEvents class
        /// </summary>
        public virtual void OnMeleeAttackEndEvent() => _weapon.EndHitBoxCheck();

        /// <summary>
        /// Called from the ElementDriver by the AnimationEvents class
        /// </summary>
        public virtual void OnSpellCastEvent() => _weapon.CastSpell();

        #endregion
    }
}
