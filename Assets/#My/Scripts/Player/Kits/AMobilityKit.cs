using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using TwinSouls.Tools;
using TwinSouls.Data;
using TwinSouls.Spells;
using System;
using TwinSouls.Entity;

namespace TwinSouls.Player.Kits
{
    /// <summary>
    /// Handles ability variation with element, defines a moving ability
    /// </summary>
    public abstract class AMobilityKit : AKit
    {
        #region Properties

        public static event Action<GameObject, float> OnMovingAbilityStartEvt;

        protected EffectPool _effectPool;
        private InputHandler _inputs;
        protected PlayerController _controller;
        protected ElementDriver _driver;

        public ElementData.ElementType type { get; set; }

        [BoxGroup("Moving Ability")]
        [SerializeField] private Cooldown _moveAbilityCooldown;

        #endregion

        #region Unity builtins

        // Get references
        protected override void Awake()
        {
            base.Awake();
            _controller = GetComponent<PlayerController>();
            _driver = GetComponent<ElementDriver>();
            _inputs = GetComponent<InputHandler>();
        }

		// Initialization
		protected override void Start()
        {
            _moveAbilityCooldown.Init();
            OnMovingAbilityStartEvt?.Invoke(gameObject, _moveAbilityCooldown.cooldownTime);
        }

        private void OnEnable()
		{
            _inputs.OnMovingAbilityInputEvt += Inputs_OnMovingAbilityEvt;
        }

        private void OnDisable()
		{
            _inputs.OnMovingAbilityInputEvt -= Inputs_OnMovingAbilityEvt;
        }

        #endregion

        #region Inputs

        private void Inputs_OnMovingAbilityEvt()
        {
            if (_moveAbilityCooldown.IsOver())
            {
                OnMoveAbility();
                OnMovingAbilityStartEvt?.Invoke(gameObject, _moveAbilityCooldown.cooldownTime);
                _moveAbilityCooldown.Reset();
            }
        }

        protected override bool WantsToAttack() => _inputs.attackDown;

        #endregion

        protected abstract void OnMoveAbility();
	}
}