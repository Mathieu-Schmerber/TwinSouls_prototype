using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TwinSouls.Data;
using UnityEngine;
using TwinSouls.Tools;
using TwinSouls.Entity;

namespace TwinSouls.Spells
{
    public class ElementalProjectile : ProjectileSpell
    {
		#region Properties

		private ParticleSystem.MainModule _main;
		private ParticleSystem.ColorOverLifetimeModule _trailColor;
		private TrailRenderer _trail;

		#endregion

		#region Unity Builtins

		protected override void Awake()
		{
			base.Awake();
			_main = GetComponent<ParticleSystem>().main;
			if (transform.childCount > 0)
				_trailColor = transform.GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime;
			_trail = GetComponent<TrailRenderer>();
		}

		private void Start()
		{
			ElementData.ElementType mainElement = Effects.First().element;

			_main.startColor = DataLoader.GetElementOfType(mainElement).color;
			if (transform.childCount > 0)
				_trailColor.color = DataLoader.GetElementOfType(mainElement).gradient;
			if (_trail)
				_trail.colorGradient = DataLoader.GetElementOfType(mainElement).gradient;
		}

		#endregion

		protected override void ApplyOnHitEffects(GameObject target)
		{
			AElementProcessor processor = target.GetComponent<AElementProcessor>();

			foreach (EffectData effect in Effects)
				processor?.ProcessEffect(effect, Caster.transform);
			processor?.ProcessElementalDamage(Effects.First().element, Descriptor.damage, Caster.transform);
		}

		protected override void OnHitFxInstantiated(GameObject instance)
		{
			ParticleSystem.ColorOverLifetimeModule hit = instance.GetComponent<ParticleSystem>().colorOverLifetime;

			hit.color = DataLoader.GetElementOfType(Effects.First().element).gradient;
		}
	}
}