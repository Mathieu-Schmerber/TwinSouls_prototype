using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TwinSouls.Interactibles
{
    public class Door : MonoBehaviour
	{
		/// <summary>
		/// Used to avoid visual glitches when the door enters the ground
		/// </summary>
		const float STEP = 0.01f;

		[SerializeField] private bool _forceInputActiveAfterOpened = false;
		[SerializeField] private Activatable[] _inputs;
		[SerializeField] private float _openTime;

		private Transform _graphics;
		private Collider _collider;
		private Vector3 _initialPos;
		private float _height;

		private void Awake()
		{
			_initialPos = transform.position;
			_graphics = transform.GetChild(0);
			_collider = GetComponent<Collider>();
			_height = transform.lossyScale.y;
			foreach (Activatable item in _inputs.Where(item => item != null))
			{
				item.OnActivatedEvt += OnStateChanged;
				item.OnDisactivatedEvt += OnStateChanged;
			}
		}

		private void OnDestroy()
		{
			foreach (Activatable item in _inputs.Where(item => item != null))
			{
				item.OnActivatedEvt -= OnStateChanged;
				item.OnDisactivatedEvt -= OnStateChanged;
			}
		}

		public void OnStateChanged()
		{
			if (!_inputs.Any(item => !item.IsActive))
			{
				if (_forceInputActiveAfterOpened)
					_inputs.ToList().ForEach(item => item._desactivable = false);
				Open();
			}
			else
				Close();
		}

		public void Open()
		{
			Vector3 destination = _initialPos - (Vector3.up * (_height - STEP));

			_collider.enabled = false;
			iTween.MoveTo(_graphics.gameObject, destination, _openTime);
		}

		public void Close()
		{
			_collider.enabled = true;
			iTween.MoveTo(_graphics.gameObject, _initialPos, _openTime);
		}
	}
}