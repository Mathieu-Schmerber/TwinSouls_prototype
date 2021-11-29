using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using TwinSouls.Data;
using TwinSouls.Tools;
using TwinSouls.Spells;

namespace TwinSouls.Player 
{
    public class LinkEffect : MonoBehaviour, IElementModulable
    {
        #region Types

        #endregion

        #region Properties

        private const float ALTITUDE_OFFSET = 1;

        private ElementEffectProcessor _processor;
        private ElementDriver _driver;
        private LineRenderer _lr;
        private Material _mat;
        private ParticleSystem _particles;

        [SerializeField] private Vector2 offset;
        private IEnumerable<ElementData> _elements;

        protected ElementData.ElementType CurrentElement { get => _processor.EmittedElement; }

        #endregion

        #region Unity builtins

        // Get references
        private void Awake()
        {
            _driver = GetComponentInParent<ElementDriver>();
            _lr = GetComponent<LineRenderer>();
            _mat = _lr.material;
            _particles = GetComponentInChildren<ParticleSystem>();
            _elements = DataLoader.Instance.Elements;
            _processor = _driver.processor;

            _processor.OnEmittedElementChangedEvt += UpdateElement;
        }

		private void OnDestroy()
		{
            _processor.OnEmittedElementChangedEvt -= UpdateElement;
        }

        // Initialization
        private void Update()
        {
            Vector3 startpoint = _driver.transform.position + new Vector3(0, ALTITUDE_OFFSET, 0);
            Vector3 endpoint = _driver.GetEndPointProgression(ALTITUDE_OFFSET);

            _lr.SetPositions(new Vector3[] { startpoint, endpoint });
            _mat.mainTextureOffset += offset * Time.deltaTime;
            _particles.transform.position = endpoint;
        }

        #endregion

        public void UpdateElement(ElementData.ElementType element)
        {
            ParticleSystem.ColorOverLifetimeModule colorOverLifetime = _particles.colorOverLifetime;
            Gradient gradient = _elements.FirstOrDefault(x => x.type == element)?.gradient ?? DataLoader.Instance.Constants.noneGradient;
            Color color = _elements.FirstOrDefault(x => x.type == element)?.color ?? DataLoader.Instance.Constants.noneColor;

            colorOverLifetime.color = color;
            _lr.colorGradient = gradient;
        }
    }
}