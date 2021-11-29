using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using TwinSouls.Data;

namespace TwinSouls.Player.Kits 
{
    public class FireKit : ADashableElementalKit
    {
        #region Types



        #endregion

        #region Properties

        #endregion

        #region Unity builtins

        // Get references
        protected override void Awake()
        {
            base.Awake();
            type = ElementData.ElementType.FIRE;
        }

		protected override void Update()
		{
			base.Update();
		}

        #endregion
    }
}