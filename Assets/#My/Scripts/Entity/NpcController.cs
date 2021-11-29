using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using TwinSouls.Player;

namespace TwinSouls.Entity
{
	/// <summary>
	/// Defines a non-playable character controller. <br></br>
	/// Handles the possibility to be aiming at a target, and to be able to move to a waypoint.
	/// </summary>
	public abstract class NpcController : AController
	{
		#region Properties

		[SerializeField] private float _destinationReachingRange = 1.5f;

		private Vector3 _targetPosition;
		private Vector3 _destinationPosition;

		/// <summary>
		/// Distance to destination to consider it reached;
		/// </summary>
		public float DestinationReachingRange { get => _destinationReachingRange; }

		/// <summary>
		/// The position of the target to be aiming at
		/// </summary>
		protected virtual Vector3 TargetPosition { get => _targetPosition; set => _targetPosition = value; }

		/// <summary>
		/// The destination's position to go to
		/// </summary>
		protected Vector3 Destination { get => _destinationPosition; set => _destinationPosition = value; }

		#endregion

		#region Unity Builtins

		protected virtual void Start() => Destination = GetNewDestination();

		protected override void FixedUpdate()
		{
			// Movement applied from the base class
			base.FixedUpdate();

			if (Vector3.Distance(transform.position, Destination) <= DestinationReachingRange)
				OnDestinationReached();
		}

		#endregion

		/// <summary>
		/// Process and return the next waypoint
		/// </summary>
		/// <returns></returns>
		protected abstract Vector3 GetNewDestination();

		/// <summary>
		/// Called whenever the destination was reached
		/// </summary>
		protected virtual void OnDestinationReached() => Destination = GetNewDestination();

		protected override Vector3 GetMovementsInputs() => (Destination - transform.position).normalized;

		protected override Vector3 GetTargetPosition() => TargetPosition;
	}
}