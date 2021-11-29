using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwinSouls.Entity
{
	public class NpcKit : AKit
	{
		protected override bool WantsToAttack() => true; // TODO: Get weapon range
	}
}