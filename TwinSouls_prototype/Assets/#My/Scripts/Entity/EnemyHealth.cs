using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;

namespace TwinSouls.Entity
{
    public class EnemyHealth : Damageable
    {
        #region Properties

        private Animator _animator;

        #endregion

        #region Unity builtins

        // Get references
        protected override void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            base.Awake();
        }

		#endregion

		protected override void ApplyProcessedDamage(GameObject source, float amount, bool directSource)
		{
            if (directSource)
                _animator.Play("GetHit");
			base.ApplyProcessedDamage(source, amount, directSource);
		}
	}
}