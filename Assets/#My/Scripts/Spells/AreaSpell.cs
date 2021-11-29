using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using TwinSouls.Tools;
using TwinSouls.Data;

namespace TwinSouls.Spells 
{
    public class AreaSpell : ASpell<ASpellDescriptor>
    {
		#region Properties

		public bool HasBeenCasted { get => GetComponent<FXTimedDestruction>() != null; }

		[SerializeField] private float _defaultActiveTime = 1f;

		#endregion

		#region Unity builtins

		private void OnTriggerStay(Collider other)
		{
			if (!HasBeenCasted || !IsActive) return;
			else if (TargetFitsTargetMode(other.gameObject))
			{
				foreach (EffectData effect in Effects)
					other.GetComponent<AElementProcessor>()?.ProcessEffect(effect, Caster.transform);
			}
		}

		#endregion

		/// <summary>
		/// Activate the area spell for a given time. <br></br>
		/// If no time is given, the default active time of the spell is used.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="time"></param>
		public virtual void CastArea(Stats caster, float? time = null)
		{
			Caster = caster;
			gameObject.AddComponent<FXTimedDestruction>().TimedSmoothFxKill(time.HasValue ? time.Value : _defaultActiveTime);
		}
	}
}