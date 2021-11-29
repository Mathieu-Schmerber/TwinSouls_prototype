using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using TwinSouls.Data;

namespace TwinSouls.Player.Kits 
{
    public class NoneKit : AMobilityKit
    {
        #region Unity builtins

        // Get references
        protected override void Awake()
        {
            base.Awake();
            type = ElementData.ElementType.NONE;
        }

		protected override void Update()
		{
			base.Update();
		}

		#endregion

		protected override void OnMoveAbility() {}
	}
}