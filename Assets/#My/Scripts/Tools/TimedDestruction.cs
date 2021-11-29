using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;


namespace TwinSouls.Tools 
{
	/// <summary>
	/// Destroy gameobject after a defined time. <br></br>
	/// Usefull if you wish to destroy a temporary instanciated object from an object bound to be destroyed before (eg. Particles, Projectiles, ...)
	/// </summary>
    public class TimedDestruction : MonoBehaviour
    {
		public void SetDestructionTimer(float seconds) => Invoke(nameof(DestroyInstance), seconds);

		public virtual void DestroyInstance()
		{
			Destroy(gameObject);
		}
	}
}