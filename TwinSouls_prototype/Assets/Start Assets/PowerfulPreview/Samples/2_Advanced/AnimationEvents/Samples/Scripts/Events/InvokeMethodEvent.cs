using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace StartAssets.AnimationEvents.Samples
{
    /// <summary>
    /// This class implements an event which invokes some method 
    /// of the events character by it's name with some delay. 
    /// 
    /// It's just an example of what you can achieve with animation
    /// events so you may use any class as an origin for the methods call. 
    /// </summary>
    public class InvokeMethodEvent : AnimationEvent
    {
        public override void Invoke( GameObject parent, MonoBehaviour coroutineMaster )
        {
            parent.GetComponent<EventsCharacter>().Invoke( m_MethodName, m_Delay );
        }

        [SerializeField]
        private string m_MethodName;
        [SerializeField]
        private float m_Delay;

    }
}
