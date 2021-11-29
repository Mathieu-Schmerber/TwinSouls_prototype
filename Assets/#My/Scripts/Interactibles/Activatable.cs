using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TwinSouls.Interactibles
{
	public abstract class Activatable : MonoBehaviour
	{
		public event Action OnActivatedEvt;
		public event Action OnDisactivatedEvt;
		public bool IsActive { get; private set; } = false;
		public bool _desactivable = true;

		/// <summary>
		/// Changes the state of the Activable.
		/// </summary>
		/// <param name="active"></param>
		/// <returns>true if the state changed.</returns>
		public bool SetActive(bool active)
		{
			if (active == IsActive || (!active && !_desactivable)) return false;

			IsActive = active;
			if (active)
				OnActivated();
			else
				OnDisactivated();
			return true;
		}

		protected virtual void OnActivated() => OnActivatedEvt?.Invoke();

		protected virtual void OnDisactivated() => OnDisactivatedEvt?.Invoke();

		protected abstract void OnStateChanged();
	}
}
