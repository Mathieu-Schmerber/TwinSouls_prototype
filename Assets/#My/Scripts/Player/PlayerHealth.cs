using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinSouls.Entity;
using UnityEngine;

namespace TwinSouls.Player
{
	public class PlayerHealth : Damageable
	{
		public override void OnDeath(GameObject killer)
		{
			TriggerOnDeath(killer);
		}
	}
}
