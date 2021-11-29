using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TwinSouls.Interactibles
{
	public class Teleporter : MonoBehaviour
	{
		[SerializeField] private Vector3 _destinationOffsetPos;

		private void OnTriggerEnter(Collider other)
		{
			other.transform.position = transform.position + _destinationOffsetPos;
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawLine(transform.position, transform.position + _destinationOffsetPos);
			Gizmos.DrawWireSphere(transform.position + _destinationOffsetPos, 0.1f);
		}
	}
}
