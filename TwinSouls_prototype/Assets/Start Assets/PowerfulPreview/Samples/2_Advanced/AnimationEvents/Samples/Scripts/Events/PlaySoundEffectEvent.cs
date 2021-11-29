using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StartAssets.AnimationEvents.Samples
{
    public class PlaySoundEffectEvent : AnimationEvent
    {
        public bool PreviewSound
        {
            get
            {
                return m_PreviewSound;
            }
        }

        public AudioClip audioClip;

        public override void Invoke( GameObject parent, MonoBehaviour coroutineMaster )
        {
            AudioSource.PlayClipAtPoint(audioClip, parent.transform.position);
        }

        [SerializeField]
        private bool m_PreviewSound;
    }
}

