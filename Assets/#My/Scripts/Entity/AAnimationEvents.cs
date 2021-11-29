using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TwinSouls.Entity
{
    public abstract class AAnimationEvents : MonoBehaviour
    {
        public enum Events
        {
            NewEvent,
            OnMeleeAttackStart,
            OnSpellCast,
            OnMeleeAttackEnd
        }

        public abstract void OnAnimatorEvent(string eventName);
    }
}
