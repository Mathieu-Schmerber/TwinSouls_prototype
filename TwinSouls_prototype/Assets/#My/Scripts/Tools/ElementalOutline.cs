using System;
using System.Collections.Generic;
using System.Linq;
using TwinSouls.Data;
using TwinSouls.Spells;
using UnityEngine;

namespace TwinSouls.Tools
{
    public class ElementalOutline : Outline, IElementModulable
    {
        private IEnumerable<ElementData> _elements;
        private AElementProcessor _processor;

        protected override void Awake()
        {
            base.Awake();
            _elements = DataLoader.Instance.Elements;
            _processor = GetComponentInParent<AElementProcessor>();
            _processor.OnEmittedElementChangedEvt += UpdateElement;
        }

        private void OnDestroy()
        {
            _processor.OnEmittedElementChangedEvt -= UpdateElement;
        }

        public void UpdateElement(ElementData.ElementType elementType)
        {
            outlineColor = _elements.FirstOrDefault(x => x.type == elementType)?.color ?? DataLoader.Instance.Constants.noneColor;
            needsUpdate = true;
        }
    }
}
