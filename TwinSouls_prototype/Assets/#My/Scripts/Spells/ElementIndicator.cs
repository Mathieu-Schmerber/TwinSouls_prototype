using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TwinSouls.Data;
using TwinSouls.Tools;
using UnityEngine;

namespace TwinSouls.Spells
{
	/// <summary>
	/// Indicates which element an entity is emitting
	/// </summary>
	public class ElementIndicator : MonoBehaviour, IElementModulable
	{
		private ParticleSystemRenderer _main;
		private ParticleSystem _circle;
		private IEnumerable<ElementData> _elements;
		private ElementEffectProcessor _processor;
		private EffectPool _pool;

		private void Awake()
		{
			_main = GetComponent<ParticleSystemRenderer>();
			_circle = transform.GetChild(0).GetComponent<ParticleSystem>();
			_elements = DataLoader.Instance.Elements;
			_processor = GetComponentInParent<ElementEffectProcessor>();
			_main.enabled = false;
			_pool = GetComponentInParent<EffectPool>();

			_pool.OnBoostChangedEvt += SetBoost;
			_processor.OnEmittedElementChangedEvt += UpdateElement;
		}

		private void OnDestroy()
		{
			_pool.OnBoostChangedEvt -= SetBoost;
			_processor.OnEmittedElementChangedEvt -= UpdateElement;
		}

		private void Start() => SetBoost(ElementData.ElementType.NONE);

		public void SetBoost(ElementData.ElementType elementType)
		{
			Color color = _elements.FirstOrDefault(x => x.type == elementType)?.color ?? DataLoader.Instance.Constants.noneColor;

			_circle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

			if (elementType == ElementData.ElementType.NONE)
				return;

			ElementData data = DataLoader.GetElementOfType(elementType);

			_circle.Play(true);

			ParticleSystem.MainModule _circleColor = _circle.main;

			_circleColor.startColor = new Color(color.r, color.g, color.b, 1);
		}

		public void UpdateElement(ElementData.ElementType elementType)
		{
			if (elementType == ElementData.ElementType.NONE)
			{
				_main.enabled = false;
				return;
			}

			ElementData data = _elements.First(x => x.type == elementType);

			_main.enabled = true;
			_main.mesh = data.mesh3d;
			_main.material = data.material;
		}
	}
}