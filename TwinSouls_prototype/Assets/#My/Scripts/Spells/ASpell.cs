using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using TwinSouls.Data;
using TwinSouls.Tools;
using static TwinSouls.Spells.EffectPool;
using static TwinSouls.Spells.ASpellDescriptor;
using System;

namespace TwinSouls.Spells
{
    public abstract class ASpell<T> : MonoBehaviour 
		where T : ASpellDescriptor
	{
		#region Properties

		[SerializeField] private T _descriptor;

		public T Descriptor
		{
			get => _descriptor;
			private set { _descriptor = value; }
		}


		public bool IsActive { get => _spellPs.isEmitting || !GetComponent<TimedDestruction>(); }
        protected ParticleSystem _spellPs;

		public List<EffectData> Effects
		{
			get { return Descriptor.effects; }
			set { Descriptor.effects = value; }
		}

		private Stats _caster;
		public Stats Caster
		{
			get => _caster;
			set
			{
				if (_caster != value)
				{
					_caster = value;
					Descriptor.damage = Descriptor.damage * (Caster.Power.Value / 100);
					AssignEffects();
				}
			}
		}


		#endregion

		#region Unity builtins

		// Get references
		protected virtual void Awake()
        {
            _spellPs = GetComponent<ParticleSystem>();
        }

		private void AssignEffects()
		{
			ElementData.ElementType emit = Caster.GetComponent<AElementProcessor>().EmittedElement;

			if (emit == ElementData.ElementType.NONE || Descriptor.importCasterElement == false)
				return;
			else
				Descriptor.effects.Add(DataLoader.GetEffectFromElementType(emit));

			if (Descriptor.importBoosts)
			{
				foreach (ActiveEffect boost in Caster.GetComponent<EffectPool>().BoostEffects)
					Descriptor.effects.Add(boost.data);
			}
		}

		#endregion

		public void Initialize(T descriptor, Stats caster)
		{
			Descriptor = (T)Activator.CreateInstance(typeof(T), descriptor);
			Caster = caster;
		}

		/// <summary>
		/// Is the target in the layer range ?
		/// </summary>
		/// <param name="target"></param>
		/// <returns></returns>
		protected bool TargetFitsTargetMode(GameObject target)
		{
			bool result = false;

			switch (Descriptor.targetMode)
			{
				case TargetMode.OTHER:
					result = (target.layer != Caster.gameObject.layer);
					break;
				case TargetMode.CASTER_LAYER:
					result = (target.layer == Caster.gameObject.layer);
					break;
				case TargetMode.CASTER_ONLY:
					result = (target == Caster.gameObject);
					break;
			}
			return result;
		}
	}
}