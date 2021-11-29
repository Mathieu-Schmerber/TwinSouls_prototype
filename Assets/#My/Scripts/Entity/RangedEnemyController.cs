using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinSouls.Interactibles;
using TwinSouls.Tools;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TwinSouls.Entity
{
	public class RangedEnemyController : NpcController
	{
		private Spawner _source;
		private Transform _targetPlayer;

		protected override Vector3 TargetPosition { get => _targetPlayer.position; }

		protected override void Awake()
		{
			base.Awake();
			_targetPlayer = StageManager.Instance.Players.OrderBy(x => Vector3.Distance(transform.position, x.position)).First();
			_source = GetComponentInParent<Spawner>();
		}

		protected override Vector3 GetNewDestination()
		{
			Vector3 destination = _source.GetRandomSpawnablePosition();

			return new Vector3(destination.x, transform.position.y, destination.z);
		}

		protected override void OnDestinationReached()
		{
			// Destination calculation
			base.OnDestinationReached();

			_targetPlayer = StageManager.Instance.Players.OrderBy(x => Vector3.Distance(transform.position, x.position)).First();
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(Destination, 0.2f);
			Gizmos.DrawLine(transform.position + Vector3.up, Destination);
			Gizmos.color = Color.magenta;
			Gizmos.DrawLine(transform.position + Vector3.up, TargetPosition);
		}
	}
}
