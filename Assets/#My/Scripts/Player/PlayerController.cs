using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.InputSystem;
using TwinSouls.Entity;

namespace TwinSouls.Player 
{
    public class PlayerController : AController
    {
		#region Properties

		private InputHandler _inputs;
		[SerializeField] private Transform _previsualisationPivot;
		private SpriteRenderer _previsualisation;
		private Vector3 _aimInput;

		public bool IsAiming { get => _aimInput.magnitude != 0; }

		#endregion

		#region Unity builtins

		protected override void Awake()
		{
			_previsualisation = _previsualisationPivot.GetComponentInChildren<SpriteRenderer>(true);
			_inputs = GetComponent<InputHandler>();
			base.Awake();
		}

		protected override void Update()
		{
			base.Update();
			ApplySmoothRotation(_previsualisationPivot);
			_previsualisation.enabled = IsAiming;
			_aimInput = new Vector3(_inputs.aimAxis.x, 0, _inputs.aimAxis.y);
		}

		#endregion

		#region Abstraction

		protected override Vector3 GetMovementsInputs() => new Vector3(_inputs.movementAxis.x, 0, _inputs.movementAxis.y);

		protected override Vector3 GetTargetPosition()
		{
			if (IsAiming)
				return _rb.position + _aimInput;
			else if (!IsAiming && GetMovementsInputs().magnitude > 0)
				return _rb.position + GetMovementNormal();
			else
				return _rb.position + _previsualisationPivot.forward;
		}

		#endregion
	}
}