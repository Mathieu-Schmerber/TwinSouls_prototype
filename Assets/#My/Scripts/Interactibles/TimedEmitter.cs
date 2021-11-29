using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TwinSouls.Spells;
using TwinSouls.Tools;
using TwinSouls.UI;

namespace TwinSouls.Interactibles
{
	/// <summary>
	/// Switches back from emitting an element to emitting NONE after a given time.
	/// </summary>
    public class TimedEmitter : MonoBehaviour
    {
		[SerializeField] private Cooldown _timer = new Cooldown() {
			automaticReset = false,
			readyOnStart = false
		};

		private bool _timerActive = false;
        private AElementProcessor _processor;
		private CooldownCircle _cc;

		private void Awake()
		{
			_cc = GetComponentInChildren<CooldownCircle>();
			_processor = GetComponent<AElementProcessor>();

			_timer.IsOverEvent += (cd) => _processor.UpdateEmittedElement(Data.ElementData.ElementType.NONE);
			_processor.OnEmittedElementChangedEvt += _processor_OnEmittedElementChangedEvt;
		}

		private void OnDestroy()
		{
			_processor.OnEmittedElementChangedEvt -= _processor_OnEmittedElementChangedEvt;
		}

		private void Update()
		{
			if (_timerActive)
				_timer.Run();
		}

		private void _processor_OnEmittedElementChangedEvt(Data.ElementData.ElementType obj)
		{
			if (obj != Data.ElementData.ElementType.NONE)
			{
				_timerActive = true;
				_cc.StartCooldown(_timer.cooldownTime);
				_timer.Init();
			}
		}
	}
}