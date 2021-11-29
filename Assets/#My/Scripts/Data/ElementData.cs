using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

namespace TwinSouls.Data 
{
    [CreateAssetMenu()]
    public class ElementData : SerializedScriptableObject
    {
        #region Types

        [System.Serializable]
        public enum ElementType
        {
            /// <summary>
            /// NONE is used to describe an entity not emitting any element. <br></br>
            /// In the effect context: an effect that can be forcefully applied without being supressed by others.
            /// </summary>
            NONE,
            FIRE,
            LIGHTING,
            ICE,
            WATER
        }

        #endregion

        #region Properties

        [HideLabel, PreviewField(55)]
        [HorizontalGroup("Horizontal", 55, LabelWidth = 67, MarginLeft = 0)]
        public Sprite icon;

        [HorizontalGroup("Horizontal"), VerticalGroup("Horizontal/RightSide")]
        public Mesh mesh3d;

        [HorizontalGroup("Horizontal"), VerticalGroup("Horizontal/RightSide")]
        public ElementType type;
        [HorizontalGroup("Horizontal"), VerticalGroup("Horizontal/RightSide")]
        public EffectData.EffectType effect;

        [Title("Graphics")]
        public Color color;
        public Gradient gradient;
        public Material material;

        [Title("Logic"), Space(20)]
        public List<ElementData> merge;
        public List<ElementData> neutral;
        public List<ElementData> strongerThan;
        public List<ElementData> weakerThan;

        #endregion
    }
}