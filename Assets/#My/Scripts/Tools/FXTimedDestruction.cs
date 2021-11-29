using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;

namespace TwinSouls.Tools 
{
    public class FXTimedDestruction : TimedDestruction
    {
        #region Properties

        private ParticleSystem[] _ps;

        public static event System.Action<GameObject, float> OnSmoothDestroyEvt;

        #endregion

        #region Unity builtins

        // Get references
        private void Awake()
        {
            _ps = GetComponentsInChildren<ParticleSystem>();
        }

        #endregion

        /// <summary>
        /// Smoothly stops the particle system after the given time
        /// </summary>
        /// <param name="time"></param>
        public void TimedSmoothFxKill(float time) => Invoke(nameof(SmoothFxKill), time);

        /// <summary>
        /// Plays the particle system and only then stops it smoothly
        /// </summary>
        public void PlayThenStop() => Invoke(nameof(SmoothFxKill), _ps.Max(ps => ps.main.startLifetime.constantMax));

        /// <summary>
        /// Smoothly stops the particle system
        /// </summary>
        public void SmoothFxKill()
		{
            float time = _ps.Max(ps => ps.main.startLifetime.constantMax);

            _ps.ToList().ForEach(ps => ps.Stop(true, ParticleSystemStopBehavior.StopEmitting));
            SetDestructionTimer(time);
            OnSmoothDestroyEvt?.Invoke(gameObject, time);
        }
    }
}