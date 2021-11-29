using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTime : MonoBehaviour
{
    public float lifeTime;

	private void Start ()
    {
        mDeathTime = Time.time + lifeTime;
	}
	
	private void Update ()
    {
	    if( Time.time > mDeathTime)
        {
            Destroy( gameObject );
        }
	}

    private float mDeathTime;
}
