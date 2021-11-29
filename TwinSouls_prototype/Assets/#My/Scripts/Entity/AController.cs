using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;

namespace TwinSouls.Entity
{
    [RequireComponent(typeof(Stats))]
    public abstract class AController : SerializedMonoBehaviour
    {
        #region Properties

        protected Stats _stats;
        protected Rigidbody _rb;
        protected Quaternion _desiredRotation;
        protected GameObject _graphics;
        protected Animator _gfxAnim;
        private bool _canMove = true;
        [ShowInInspector] private float _rotationSpeed = 6f;

		public bool CanMove { get => _canMove && !_stats.IsStunned; set => _canMove = value; }

		#endregion

		/// <summary>
		/// Gets the movement input normalized
		/// </summary>
		/// <returns></returns>
		protected abstract Vector3 GetMovementsInputs();

        /// <summary>
        /// Gets the position of a target
        /// </summary>
        /// <returns></returns>
        protected abstract Vector3 GetTargetPosition();

        #region Unity builtins

        // Get references
        protected virtual void Awake()
        {
            _stats = GetComponent<Stats>();
            _rb = GetComponent<Rigidbody>();
            _graphics = GetComponentInChildren<Animator>().gameObject;
            _gfxAnim = _graphics?.GetComponent<Animator>();
            _desiredRotation = transform.rotation;
        }

        // Initialization
        protected virtual void Update()
        {
            if (_stats.IsStunned) return;

            Vector3 dir = GetAimNormal();

            if (_graphics)
                ApplySmoothRotation(_graphics.transform);

            Vector3 inputs = GetMovementsInputs();

            _gfxAnim?.SetFloat("Speed", inputs.magnitude);
            _gfxAnim?.SetFloat("Angle", Vector3.Angle(inputs, dir));
        }

        protected virtual void FixedUpdate() => Move();

        #endregion

        public Vector3 GetAimNormal() => (GetTargetPosition() - _rb.position).normalized;
        public Vector3 GetMovementNormal() => GetMovementsInputs().normalized;

        /// <summary>
        /// Rotation angle calculation and lerping
        /// </summary>
        protected void ApplySmoothRotation(Transform tr)
        {
			_desiredRotation = Quaternion.LookRotation(GetAimNormal());
            tr.rotation = Quaternion.Slerp(tr.transform.rotation, _desiredRotation, _rotationSpeed * Time.deltaTime);
            tr.localPosition = Vector3.zero;
		}

        /// <summary>
        /// Movement speed calculation, and object motion
        /// </summary>
        protected virtual void Move()
        {
            if (!CanMove) return;

            // Calculate how fast we should be moving
            var targetVelocity = GetMovementsInputs();
            targetVelocity = transform.TransformDirection(targetVelocity);
            targetVelocity *= _stats.Speed.Value;

            // Apply a force that attempts to reach our target velocity
            var velocity = _rb.velocity;
            var velocityChange = (targetVelocity - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -_stats.Speed.Value, _stats.Speed.Value);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -_stats.Speed.Value, _stats.Speed.Value);
            velocityChange.y = 0;
            _rb.AddForce(velocityChange, ForceMode.VelocityChange);
        }
    }
}