using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 direction;
    public GameObject hitPrefab; 

    private void Update()
    {
        transform.position += transform.TransformDirection( direction ) * Time.deltaTime; 
    }

    private void OnTriggerEnter( Collider other )
    {
        Instantiate( hitPrefab, transform.position, transform.rotation );
        Destroy( gameObject );
    }
}
