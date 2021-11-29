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
	/// Simple ElementProcessor which absorbs any element
	/// </summary>
	public class ElementAbsorber : AElementProcessor
	{
		public override void ProcessEffect(EffectData inputEffect, Transform caster) => UpdateEmittedElement(inputEffect.element);

		public override void ProcessElementalDamage(ElementData.ElementType element, float damage, Transform caster)
			=> _damageable?.ApplyDamage(caster.gameObject, damage, true);
	}
}
