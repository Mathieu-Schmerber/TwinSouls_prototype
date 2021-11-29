using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinSouls.Data;
using UnityEngine;

namespace TwinSouls.Spells
{
	/// <summary>
	/// ElementProcessor made only to declare an object as spell affectable.
	/// </summary>
	public class IdleElementProcessor : AElementProcessor
	{
		public override void ProcessEffect(EffectData inputEffect, Transform caster) => RaiseEmittedElementEvent(inputEffect.element);

		public override void ProcessElementalDamage(ElementData.ElementType element, float damage, Transform caster)
			=> _damageable.ApplyDamage(caster.gameObject, damage, true);
	}
}
