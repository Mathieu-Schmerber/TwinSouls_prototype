using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using TwinSouls.Spells;

namespace TwinSouls.Data 
{
    [CreateAssetMenu()]
    public class WeaponData : SerializedScriptableObject
    {
		#region Types

		[System.Serializable]
		public class WeaponSpell
		{
			public GameObject spellPrefab;
			public ProjectileDescriptor descriptor;
		}

		[System.Serializable]
		public class WeaponAttack
		{
			[Required] public AnimationClip attackAnimation;
			public WeaponSpell spellToCast;
			public int onHitDamage;
		}

		#endregion

		#region Properties

		public Mesh mesh;
		public float comboIntervalTime;
		public float attackSpeed;
		public List<WeaponAttack> attackCombos;

		#endregion
	}
}