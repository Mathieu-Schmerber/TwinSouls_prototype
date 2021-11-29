using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinSouls.Player;

namespace TwinSouls.Entity
{
	public class AnimationEventsBroadcaster : AAnimationEvents
	{
		private ElementalWeapon _weapon;

		private void Awake()
		{
			_weapon = transform.parent.GetComponentInChildren<ElementalWeapon>();
		}

		public override void OnAnimatorEvent(string eventName)
		{
			AAnimationEvents.Events evt;

			if (Enum.TryParse<AAnimationEvents.Events>(eventName, out evt))
			{
				switch (evt)
				{
					case AAnimationEvents.Events.NewEvent:
						break;
					case AAnimationEvents.Events.OnMeleeAttackStart:
						_weapon.BeginHitboxCheck();
						break;
					case AAnimationEvents.Events.OnSpellCast:
						_weapon.CastSpell();
						break;
					case AAnimationEvents.Events.OnMeleeAttackEnd:
						_weapon.EndHitBoxCheck();
						break;
					default:
						break;
				}
			}
		}
	}
}