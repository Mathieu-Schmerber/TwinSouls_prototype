using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StartAssets.AnimationEvents.Samples
{
    public class SlowMoEvent : AnimationEvent
    {
        [Range( 0, 1 )]
        public float timeScale = 1.0f;
        public float duration = 1.0f;

        public override void Invoke( GameObject parent, MonoBehaviour coroutineMaster )
        {
            coroutineMaster.StartCoroutine( SlowTime() );
        }

        private IEnumerator SlowTime()
        {
            float t = Time.timeScale;

            Time.timeScale = timeScale;
            yield return new WaitForSeconds( duration );
            Time.timeScale = t; 
        }
    }
}