using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinSouls.Data;
using TwinSouls.Tools;
using UnityEngine;

namespace TwinSouls.Interactibles
{
	public class ElementalPillarFeedback : MonoBehaviour
	{
		[SerializeField] private ElementData.ElementType _type;
		private ElementData _data;
		private Renderer _renderer;

		private void Awake()
		{
			_data = DataLoader.GetElementOfType(_type);
			_renderer = GetComponent<Renderer>();
		}

		public void UpdateFeedback(ElementalPillar.ElementBoolDictionary elements)
		{
			_renderer.material = elements[_type] ? _data.material : DataLoader.Instance.Constants.noneMaterial;
		}
	}
}
