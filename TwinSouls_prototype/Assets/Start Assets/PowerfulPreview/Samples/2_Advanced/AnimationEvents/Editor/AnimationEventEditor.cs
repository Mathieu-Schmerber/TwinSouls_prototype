using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace StartAssets.AnimationEvents
{
    /// <summary>
    /// It's a base editor class for any animation event editor.
    /// It contains a property <c>KeyFrameColor</c> which allows you 
    /// to color <c>KeyFrameControl</c> with some specific color for the specific event. 
    /// </summary>
    [CustomEditor(typeof(AnimationEvent))]
    public abstract class AnimationEventEditor : Editor
    {
        /// <summary>
        /// It's a color which the <c>KeyFrameControl</c> will be colored with on the animation
        /// events timeline. 
        /// </summary>
        public virtual Color KeyFrameColor
        {
            get
            {
                return Color.grey; 
            }
        }

        public AnimationEvent Event
        {
            get
            {
                return target as AnimationEvent;
            }
        }
    }
}


