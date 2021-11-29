using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TwinSouls.Data;
using TwinSouls.Entity;
using UnityEngine;


namespace TwinSouls.Spells
{
    public abstract class ProjectileSpell : ASpell<ProjectileDescriptor>
    {
        #region Types

        #endregion

        #region Properties

        protected Rigidbody _rb;

		public float Speed => Descriptor.speed;

		public GameObject OnHitFx => Descriptor.onHitFx;

		private int _piercedEntities = 0;

		#endregion

		#region Unity builtins

		protected override void Awake()
		{
			_rb = GetComponent<Rigidbody>();
		}

		private void FixedUpdate()
		{
			_rb.MovePosition(_rb.position + transform.forward * Time.fixedDeltaTime * Speed);
		}

		private void OnTriggerEnter(Collider other)
		{
			if (TargetFitsTargetMode(other.gameObject) && !other.isTrigger) 
				OnTargetHit(other.gameObject);
		}

		#endregion

		protected virtual void OnTargetHit(GameObject target)
		{
			ApplyOnHitEffects(target);
			if (OnHitFx)
				OnHitFxInstantiated(Instantiate(OnHitFx, transform.position, Quaternion.LookRotation(-transform.forward)));
			_piercedEntities++;
			if (!Descriptor.piercing || (Descriptor.piercing && _piercedEntities >= Descriptor.maxEntityPierce) || target.isStatic)
				Destroy(gameObject);
		}

		protected virtual void ApplyOnHitEffects(GameObject target)
		{
			AElementProcessor processor = target.GetComponent<AElementProcessor>();

			foreach (EffectData effect in Effects)
				processor?.ProcessEffect(effect, Caster.transform);
			target.GetComponent<Damageable>()?.ApplyDamage(target, Descriptor.damage, true);
		}

		protected abstract void OnHitFxInstantiated(GameObject instance);
	}
}