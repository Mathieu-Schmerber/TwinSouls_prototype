using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TwinSouls.Interactibles
{
    public class GatherArea : Activatable
    {
		#region Properties

		private int _currentNumber = 0;

		public int CurrentNumber { 
			get => _currentNumber; 
			set 
			{ 
				_currentNumber = value;
				OnStateChanged();
			} 
		}

		public LayerMask _eligibleDetection;
		public int _activationNumber;

		private TextMeshProUGUI _trackerText;

		#endregion

		private void Awake()
		{
			_trackerText = GetComponentInChildren<TextMeshProUGUI>();
		}

		private void Start() => _trackerText.text = GetText();

		private void OnTriggerEnter(Collider other)
		{
			if (_eligibleDetection == (_eligibleDetection | (1 << other.gameObject.layer)))
				CurrentNumber++;
		}

		private void OnTriggerExit(Collider other)
		{
			if (_eligibleDetection == (_eligibleDetection | (1 << other.gameObject.layer)))
				CurrentNumber--;
		}

		private string GetText() => $"{CurrentNumber} / {_activationNumber}";

		protected sealed override void OnStateChanged()
		{
			bool changed = SetActive(CurrentNumber == _activationNumber);

			if (changed || (!_desactivable && !IsActive))
				_trackerText.text = GetText();
		}
	}
}