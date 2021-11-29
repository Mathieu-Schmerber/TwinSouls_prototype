using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StartAssets.AnimationEvents.Samples
{
    public class ThrowEvent : AnimationEvent
    {
        public GameObject prefab;
        public GameObject hitPrefab; 
        public Vector3 direction; 
        public InstantiateOptions origin;

        public override void Invoke( GameObject parent, MonoBehaviour coroutineMaster )
        {
            var instance = GameObject.Instantiate( prefab, Vector3.zero, Quaternion.Euler( Vector3.zero ) );
            var projectile = instance.AddComponent<Projectile>();
            projectile.direction = direction;
            projectile.hitPrefab = hitPrefab;

            if( origin.linkToBodyPart)
            {
                var skeletonAvatar = parent.GetComponent<Animator>();
                if (skeletonAvatar != null)
                {
                    instance.transform.position = skeletonAvatar.GetBoneTransform( origin.bodyPart ).transform.position;
                }
            }
            else
            {
                instance.transform.position = parent.transform.position + origin.positionOffset;
            }

            instance.transform.LookAt( parent.transform.position + parent.transform.TransformDirection( Vector3.forward ) * 1000.0f );
        }
    }
}