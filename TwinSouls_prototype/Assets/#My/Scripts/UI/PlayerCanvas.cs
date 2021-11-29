using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TwinSouls.Data;
using TMPro;
using TwinSouls.Player;

namespace TwinSouls.UI 
{
    public class PlayerCanvas : MonoBehaviour
    {
        #region Properties

        [SerializeField] private Image _suggestionUI;
        [SerializeField] private GameObject _interactionUI;
        private TextMeshProUGUI _interactionText;
        private Image _interactionImage;
        private InputHandler _handler;
        private PlayerWeaponHolder _holder;
        private Text _suggestionMark;
        private Animator _anim;

        #endregion

        #region Unity builtins

        // Get references
        private void Awake()
        {
            _holder = transform.parent.GetComponentInChildren<PlayerWeaponHolder>();
            _handler = transform.parent.GetComponent<InputHandler>();
            _suggestionMark = _suggestionUI.GetComponentInChildren<Text>();
            _anim = _suggestionUI.GetComponent<Animator>();

            _interactionText = _interactionUI.GetComponentInChildren<TextMeshProUGUI>();
            _interactionImage = _interactionUI.GetComponentInChildren<Image>();
            _interactionUI.SetActive(false);
            _holder.OnSuggestionChangedEvt += _holder_OnSuggestionChangedEvt;
        }

		private void OnDestroy()
		{
            _holder.OnSuggestionChangedEvt -= _holder_OnSuggestionChangedEvt;
        }

		private void Start()
		{
            _suggestionUI.GetComponent<CanvasGroup>().alpha = 0;
        }

        #endregion

        #region Public access

        public void DisplaySuggestion(ElementData element)
        {
            _suggestionUI.sprite = element.icon;
            _suggestionMark.text = "?";
            _anim.Play("suggestion_show", -1, 0);
        }

        public void AcceptSuggestion() => _anim.Play("suggestion_accept");

        public void OnAcceptEnd()
        {
            _suggestionMark.text = "✓";
        }

        #endregion

        private void _holder_OnSuggestionChangedEvt(WeaponData obj)
        {
            _interactionUI.gameObject.SetActive(obj != null);
            if (obj != null)
			{
                iTween.PunchScale(_interactionUI.gameObject, new Vector3(0.5f, 0.5f, 0.5f), 0.5f);
                _interactionImage.sprite = _handler.GetInputSprite(_handler.Input().Interact);
                _interactionText.text = $"Pickup {obj.name}";
			}
        }
    }
}