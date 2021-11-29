using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using TwinSouls.Tools;

namespace TwinSouls.Data 
{
	/// <summary>
	/// Scriptable object
	/// </summary>
	public class EffectData : SerializedScriptableObject
    {
        #region Types

        public enum EffectType
		{
            BURN,
            SLOW,
            SPEED,
            STUN,
            WEAK,
            EXPLOSION,
            FREEZE
        }

        public enum ValueType
		{
            UNIT,
            PERCENTAGE
		}

        #endregion

        #region Properties

        [Tooltip("False if it is the cause of effects combination")]
        public bool isPrimary = true;

        /// <summary>
        /// The element associated with this effect. <br></br>
        /// Use this field only if isPrimary is true.
        /// </summary>
        [ShowIf("@isPrimary == true")]
        public ElementData.ElementType element;

        /// <summary>
        /// The combination of types required to create this effect. <br></br>
        /// Use this field only if isPrimary is false.
        /// </summary>
        [ShowIf("@isPrimary == false")]
        public EffectData[] fusion = new EffectData[2];

        public EffectType type;
        public bool stackable;
        public float duration;

        [HorizontalGroup("Effect Value")]
        [ShowIf("@type != EffectType.STUN")]
        public float value;

        [HorizontalGroup("Effect Value"), HideLabel]
        [ShowIf("@type != EffectType.STUN")]
        public ValueType valueType;

        [HorizontalGroup("Intervals")]
        public bool overTime;

        [HorizontalGroup("Intervals", MarginRight = 0.25f)]
        [ShowIf("@overTime"), LabelWidth(50)]
        public float interval;

        [Required, Space]
        public GameObject prefabFx;

        #endregion

        public ElementData.ElementType[] GetSingleOrFusionElements()
		{
            if (isPrimary)
                return new ElementData.ElementType[] { this.element };
            else
                return fusion.Select(x => x.element).ToArray();
		}
    }
}