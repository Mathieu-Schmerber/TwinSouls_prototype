using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinSouls.Data;
using TwinSouls.Player;
using UnityEngine;

namespace TwinSouls.Entity
{
	public class WeaponHolder : MonoBehaviour
	{
		protected ElementalWeapon _weaponScript;
		protected MeshFilter _weaponMesh;

		protected virtual void Awake()
		{
			_weaponScript = GetComponentInChildren<ElementalWeapon>();
			_weaponMesh = _weaponScript.GetComponent<MeshFilter>();
		}

		/// <summary>
		/// Equips a weapon.
		/// </summary>
		/// <param name="data"></param>
		public virtual void EquipWeapon(WeaponData data)
		{
			_weaponScript.Data = data;
			_weaponMesh.mesh = data.mesh;
		}
	}
}
