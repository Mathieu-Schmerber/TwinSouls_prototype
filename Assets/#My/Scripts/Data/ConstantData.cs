using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;

namespace TwinSouls.Data 
{
    [CreateAssetMenu()]
    public class ConstantData : ScriptableObject
    {
        #region Types

        #endregion

        #region Properties

        public Color noneColor;
        public Gradient noneGradient;
        public Material noneMaterial;

		#endregion
	}
}