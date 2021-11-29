using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinSouls.Data;
using UnityEngine;

namespace TwinSouls.Spells
{
	[System.Serializable]
	public class ASpellDescriptor
	{
		public enum TargetMode
		{
			OTHER,
			CASTER_LAYER,
			CASTER_ONLY
		}

		[SerializeField, BoxGroup("Cast Settings")] public TargetMode targetMode;
		[SerializeField, BoxGroup("Cast Settings")] public bool importCasterElement;
		[ShowIf("@importCasterElement == true")]
		[SerializeField, BoxGroup("Cast Settings")] public bool importBoosts;

		[SerializeField, BoxGroup("Cast Settings")] public List<EffectData> effects = new List<EffectData>();

		public float damage;

		public ASpellDescriptor() { }

		public ASpellDescriptor(ASpellDescriptor clone)
		{
			targetMode = clone.targetMode;
			importCasterElement = clone.importCasterElement;
			importBoosts = clone.importBoosts;
			damage = clone.damage;
			effects = new List<EffectData>();
			effects.AddRange(clone.effects);
		}
	}

	[System.Serializable]
	public class ProjectileDescriptor : ASpellDescriptor
	{
		public bool piercing;
		public int maxEntityPierce;
		public float speed;
		public GameObject onHitFx;

		public ProjectileDescriptor() : base() { }

		public ProjectileDescriptor(ProjectileDescriptor clone) : base(clone)
		{
			piercing = clone.piercing;
			maxEntityPierce = clone.maxEntityPierce;
			speed = clone.speed;
			onHitFx = clone.onHitFx;
		}
	}
}
