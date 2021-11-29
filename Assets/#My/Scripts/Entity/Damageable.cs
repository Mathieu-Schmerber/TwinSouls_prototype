using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TwinSouls.Entity
{
    [RequireComponent(typeof(Stats)), DisallowMultipleComponent]
    public class Damageable : MonoBehaviour
    {
        #region Properties

        [SerializeField] private bool _animateOnHit = true;

        public event Action<GameObject, float, bool> OnDamageTakenEvt;
        public event Action<GameObject> OnDeathEvt;
        public bool IsInvunerable;

        protected Stats _stats;

        #endregion

        #region Unity builtins

        protected virtual void Awake() => _stats = GetComponent<Stats>();

        #endregion

        public bool IsDead() => _stats.CurrentHealth == 0 && !IsInvunerable;

        public void ApplyDamage(GameObject source, float amount, bool directSource = true)
        {
            if (IsDead()) return;

            float processedDamage = amount * (_stats.Defense.Value / 100);

            ApplyProcessedDamage(source, processedDamage, directSource);
        }

        protected virtual void ApplyProcessedDamage(GameObject source, float amount, bool directSource = true)
		{
            if (directSource && _animateOnHit)
                iTween.PunchScale(gameObject, new Vector3(.5f, .5f, .5f), 1);
            OnDamageTakenEvt?.Invoke(source, amount, directSource);
            if (IsInvunerable) return;
            _stats.CurrentHealth -= amount;
            if (IsDead())
                OnDeath(source);
        }

        protected void TriggerOnDeath(GameObject killer) => OnDeathEvt?.Invoke(killer);

        public virtual void OnDeath(GameObject killer)
        {
            TriggerOnDeath(killer);
            Destroy(gameObject);
        }
    }
}