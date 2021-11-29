using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TwinSouls.Data;
using TwinSouls.Tools;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;

namespace TwinSouls.Player
{
    /// <summary>
    /// Input handler. <br></br>
    /// Stores the inputs states
    /// </summary>
    public class InputHandler : MonoBehaviour
    {
        public static event Action<InputHandler> OnPlayerInputReadyEvt;

        public Vector2 movementAxis;
        public Vector2 aimAxis;
        public bool attackDown;
        public event Action OnMovingAbilityInputEvt;
        public event Action<ElementData> OnElementVoteInputEvt;
        public event Action OnInteractInputEvt;

        private Rigidbody _rb;
        private InputDevice _currentDevice;
        private PlayerInput _playerInput;

		private void Awake()
		{
            _playerInput = GetComponent<PlayerInput>();
            _currentDevice = _playerInput.devices.First();
            _rb = GetComponent<Rigidbody>();
		}

		private void Start()
		{
            OnPlayerInputReadyEvt?.Invoke(this);
        }

		public void OnMove(InputAction.CallbackContext ctx) => movementAxis = ctx.ReadValue<Vector2>();

        public void OnAim(InputAction.CallbackContext ctx)
        {
            if (_rb == null)
                return;

            if (!(_currentDevice is Gamepad) && Camera.main != null)
			{
                Vector2 playerPosToScreen = Camera.main.WorldToScreenPoint(_rb.position);
                Vector2 dir = (Mouse.current.position.ReadValue() - playerPosToScreen).normalized;

                aimAxis = dir;
			}
            else
                aimAxis = ctx.ReadValue<Vector2>();
        }

        public void OnAttack(InputAction.CallbackContext ctx)
		{
            if (ctx.performed)
                attackDown = true;
            else if (ctx.canceled)
                attackDown = false;
		}

        public void OnMovingAbility(InputAction.CallbackContext ctx) => OnMovingAbilityInputEvt?.Invoke();

        public void OnElementVote(ElementData element) => OnElementVoteInputEvt?.Invoke(element);

        public void OnInteract(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
                OnInteractInputEvt?.Invoke();
        }

        public Controls.PlayerActions Input() => DataLoader.Instance.Controls.Player;

        public Sprite GetInputSprite(InputAction action)
		{
            ControlsMapData.ControllerType type;

            if (_currentDevice is Gamepad)
            {
                if (_currentDevice is XInputController)
                    type = ControlsMapData.ControllerType.XBOX;
                else
                    type = ControlsMapData.ControllerType.PS4;
            }
            else
                type = ControlsMapData.ControllerType.KEYBOARD;
            return DataLoader.Instance.ControlsMap.GetInputSprite(action.name, type);
		}
    }
}