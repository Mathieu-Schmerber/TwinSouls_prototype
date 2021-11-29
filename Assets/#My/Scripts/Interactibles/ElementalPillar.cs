using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TwinSouls.Spells;
using TwinSouls.Data;
using RotaryHeart.Lib.SerializableDictionary;
using TwinSouls.Tools;
using System.Linq;
using Sirenix.OdinInspector;
using TwinSouls.UI;

namespace TwinSouls.Interactibles
{
	public class ElementalPillar : Activatable
	{
		#region Types

		[Serializable]
		public class ElementBoolDictionary : SerializableDictionaryBase<ElementData.ElementType, bool>
		{
			public ElementBoolDictionary()
			{
				this.Add(ElementData.ElementType.FIRE, false);
				this.Add(ElementData.ElementType.LIGHTING, false);
				this.Add(ElementData.ElementType.WATER, false);
				this.Add(ElementData.ElementType.ICE, false);
			}
		}

		#endregion

		#region Properties

		[SerializeField] private ElementBoolDictionary _initialState = new ElementBoolDictionary();
		[SerializeField] private Cooldown _resetCd;
		private Outline _outline;
		private bool _resetTimer = false;

		[ReadOnly, ShowInInspector] private ElementBoolDictionary _currentState = new ElementBoolDictionary();
		private AElementProcessor _processor;

		private CooldownCircle _cc;

		#endregion

		#region Unity Builtins

		private void Awake()
		{
			_outline = GetComponent<Outline>();
			_processor = GetComponent<AElementProcessor>();
			_resetCd.IsOverEvent += (cd) => ResetState();
			_cc = GetComponentInChildren<CooldownCircle>();

			_processor.OnEmittedElementChangedEvt += _processor_OnEmittedElementChangedEvt;
		}

		private void Start() => ResetState();

		private void OnDestroy()
		{
			_processor.OnEmittedElementChangedEvt -= _processor_OnEmittedElementChangedEvt;
		}

		private void Update()
		{
			if (_resetTimer) 
				_resetCd.Run();
		}

		#endregion

		private void ResetState()
		{
			_resetTimer = false;
			_initialState.Keys.ToList().ForEach(item => _currentState[item] = _initialState[item]);
			BroadcastMessage(nameof(ElementalPillarFeedback.UpdateFeedback), _currentState, SendMessageOptions.RequireReceiver);
			SetActive(!_currentState.Any(item => !item.Value));
			_outline.enabled = IsActive;
		}

		private void _processor_OnEmittedElementChangedEvt(ElementData.ElementType obj)
		{
			if (IsActive && !_desactivable)
				return;
			_currentState[obj] = !_currentState[obj];
			OnStateChanged();
		}

		protected override void OnStateChanged()
		{
			BroadcastMessage(nameof(ElementalPillarFeedback.UpdateFeedback), _currentState);
			SetActive(!_currentState.Any(item => !item.Value));
			_resetTimer = !IsActive;
			_outline.enabled = IsActive;
			if (!IsActive)
			{
				_cc.StartCooldown(_resetCd.cooldownTime);
				_resetCd.Init();
			}
		}
	}
}