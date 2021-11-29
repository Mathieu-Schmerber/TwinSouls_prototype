using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TwinSouls.Player.Kits;
using TwinSouls.Spells;
using UnityEngine;

namespace TwinSouls.Player.Kits
{
	public abstract class ADashableElementalKit : AMobilityKit
	{
		#region Properties

		[BoxGroup("Moving Ability/Dash")]
		[SerializeField] protected float _dashDistance;
		[BoxGroup("Moving Ability/Dash")]
		[SerializeField] protected float _dashTime;
		[BoxGroup("Moving Ability/Dash")]
		[SerializeField] private GameObject _areaTrailSpell;

		protected Rigidbody _rb;

		#endregion

		protected override void Awake()
		{
			base.Awake();
			_rb = GetComponent<Rigidbody>();
		}

		protected override void OnMoveAbility() => OnDash();

		protected void OnDash()
		{
			float dashSpeed = _dashDistance / _dashTime;
			Vector3 normal = _controller.GetMovementNormal() == Vector3.zero ? _controller.GetAimNormal() : _controller.GetMovementNormal();

			if (_areaTrailSpell)
				gameObject.AddComponent<TrailSpell>().StartFollowing(_areaTrailSpell, 0.05f);
			_rb.velocity = normal * dashSpeed;
			_anim.SetBool("IsDashing", true);
			_controller.CanMove = false;
			Invoke(nameof(OnDashEnd), _dashTime);
		}

		protected void OnDashEnd()
		{
			if (_areaTrailSpell)
				Destroy(gameObject.GetComponent<TrailSpell>());
			_anim.SetBool("IsDashing", false);
			_controller.CanMove = true;
			_rb.velocity = Vector3.zero;
		}
	}
}