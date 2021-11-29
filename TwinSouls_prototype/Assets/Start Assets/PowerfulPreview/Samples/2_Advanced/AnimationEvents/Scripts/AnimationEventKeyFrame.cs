using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StartAssets.AnimationEvents
{
    /// <summary>
    /// It's a key frame of the animation event, telling the system to 
    /// create and invoke the animation event at some specific time of the 
    /// associated animation. 
    /// </summary>
    public class AnimationEventKeyFrame : ScriptableObject, IComparable<AnimationEventKeyFrame>
    {
        public delegate void OnInvokeTimeChangedDelegate(float previous, float current);
        public event OnInvokeTimeChangedDelegate OnInvokeTimeChanged;

        /// <summary>
        /// The time of the associated animation when it should be invoked. 
        /// </summary>
        public float InvokeTime
        {
            set
            {
                var prev = m_InvokeTime;
                m_InvokeTime = Mathf.Clamp01(value);
                OnInvokeTimeChanged?.Invoke(prev, m_InvokeTime);
            }
            get
            {
                return m_InvokeTime;
            }
        }
        /// <summary>
        /// The associated animation event.
        /// </summary>
        public AnimationEvent Event
        {
            set
            {
                m_AnimationEvent = value;
            }
            get
            {
                return m_AnimationEvent;
            }
        }

        public bool UsePrefab
        {
            set
            {
                m_UsePrefab = value;
            }
            get
            {
                return m_UsePrefab;
            }
        }


        /// <summary>
        /// A fix for the cases when there are several animation events assigned to the same 
        /// frame of the animation. So each of the events will have different orders in the 
        /// sorted list. 
        /// </summary>
        public int CompareTo(AnimationEventKeyFrame other)
        {
            var result = InvokeTime.CompareTo(other.InvokeTime);
            if (result == 0)
            {
                return 1;
            }

            return result;
        }

        [SerializeField]
        private AnimationEvent m_AnimationEvent;
        [SerializeField]
        private bool m_UsePrefab; 
        [SerializeField]
        private float m_InvokeTime;
    }
}