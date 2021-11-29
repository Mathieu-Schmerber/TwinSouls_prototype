using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using TwinSouls.Entity;

namespace TwinSouls.Player 
{
    public class AnimationEvents : AAnimationEvents
    {
        #region Properties

        private ElementDriver _driver;

        #endregion

        #region Unity builtins

        // Get references
        private void Awake()
        {
            _driver = transform.GetComponentInParent<ElementDriver>();
        }

        #endregion

        public override void OnAnimatorEvent(string eventName)
        {
            Events evt;

            if (Enum.TryParse<AAnimationEvents.Events>(eventName, out evt))
                _driver.ManageGenericEvent(evt);
            else
                _driver.SendMessage(eventName, SendMessageOptions.RequireReceiver);
        }
    }
}