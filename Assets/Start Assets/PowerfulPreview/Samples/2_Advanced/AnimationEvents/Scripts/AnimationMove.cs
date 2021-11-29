using System;
using System.Collections.Generic;
using UnityEngine;

namespace StartAssets.AnimationEvents
{
    public class AnimationMove : ScriptableObject
    {
        public AnimationClip Animation
        {
            set
            {
                m_Animation = value;
            }
            get
            {
                return m_Animation;
            }
        }
        public AnimationEventKeyFrame[] KeyFrames
        {
            set
            {
                m_KeyFrames = value;
            }
            get
            {
                return m_KeyFrames; 
            }
        }
        
        public int LeftEvents
        {
            get
            {
                if ( mEventMarkers == null )
                {
                    return 0;
                }
                return mEventMarkers.Count;
            }
        }

        public void PrepareEvents()
        {
            mEventMarkers = new Queue<AnimationEventKeyFrame>( m_KeyFrames );
        }

        public void Handle( GameObject parent, float currentProgress, MonoBehaviour coroutineMaster )
        {
            if ( mEventMarkers.Count == 0 )
            {
                return;
            }

            var eventMarker = mEventMarkers.Peek();
            if ( currentProgress >= eventMarker.InvokeTime )
            {
                eventMarker.Event.Invoke( parent, coroutineMaster );
                mEventMarkers.Dequeue();
            }
        }

        [SerializeField]
        public AnimationClip m_Animation;
        [SerializeField]
        public AnimationEventKeyFrame[] m_KeyFrames;

        private Queue<AnimationEventKeyFrame> mEventMarkers;
    }

}