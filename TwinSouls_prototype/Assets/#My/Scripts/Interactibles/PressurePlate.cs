using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TwinSouls.Data;
using TwinSouls.Spells;
using TwinSouls.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace TwinSouls.Interactibles
{
    public class PressurePlate : Activatable
    {
		[SerializeField] private ElementData.ElementType _requiredElement;
		private Outline _outline;
		private List<AElementProcessor> _processors = new List<AElementProcessor>();

		private void Awake()
		{
			_outline = GetComponent<Outline>();
			if (_requiredElement == ElementData.ElementType.NONE)
				Debug.LogError("Pressure plate element cannot be NONE");
			else
				GetComponentInChildren<Image>().sprite = DataLoader.GetElementOfType(_requiredElement).icon;
		}

		private void OnTriggerEnter(Collider other)
		{
			AElementProcessor processor = other.GetComponent<AElementProcessor>();

			if (!_processors.Contains(processor))
				_processors.Add(processor);
		}

		private void OnTriggerExit(Collider other)
		{
			AElementProcessor processor = other.GetComponent<AElementProcessor>();

			if (_processors.Contains(processor))
			{
				_processors.Remove(processor);
				OnStateChanged();
			}
		}

		private void OnTriggerStay(Collider other) => OnStateChanged();

		protected override void OnStateChanged()
		{
			SetActive(_processors == null || _processors.Count == 0 ? false : _processors.Any(item => item.EmittedElement == _requiredElement));
		}

		protected override void OnActivated()
		{
			base.OnActivated();
			_outline.OutlineColor = Color.green;
		}

		protected override void OnDisactivated()
		{
			base.OnDisactivated();
			_outline.OutlineColor = Color.red;
		}
	}
}