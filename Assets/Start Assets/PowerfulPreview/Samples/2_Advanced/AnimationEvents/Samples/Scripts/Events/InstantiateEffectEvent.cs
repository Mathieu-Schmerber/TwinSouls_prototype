using StartAssets.PowerfulPreview;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StartAssets.AnimationEvents.Samples
{
    /// <summary>
    /// This is an example of animation event which instantiates any effect 
    /// (like fireball) at some point on the world.
    /// It can be attached to it's parent or just instantiated in world coordinates. 
    /// </summary>
    public class InstantiateEffectEvent : AnimationEvent
    {
        public bool useObjectAsParent; 
        public GameObject effectPrefab;
        public bool destroyOnTimer; 
        public float lifeTime;

        public InstantiateOptions instantiateOptions;

        public override void Invoke( GameObject parent, MonoBehaviour coroutineMaster )
        {
            Vector3 pos = parent.transform.position;
            Vector3 angles = parent.transform.eulerAngles;
            mInstance = GameObject.Instantiate( effectPrefab, pos, Quaternion.Euler( angles ), useObjectAsParent ? parent.transform : null );
            mSkeletonAvatar = parent.GetComponent<Animator>();
            if( mSkeletonAvatar != null )
            {
                if ( instantiateOptions.linkToBodyPart )
                {
                    var bodyPart = mSkeletonAvatar.GetBoneTransform( instantiateOptions.bodyPart );
                    if( bodyPart != null )
                    {
                        pos = bodyPart.position;
                        angles = bodyPart.eulerAngles; 
                    }
                }
            }
            pos += instantiateOptions.positionOffset;
            parent.transform.localScale = instantiateOptions.scale;

            mInstance.transform.position = pos;

            if( destroyOnTimer )
            {
                coroutineMaster.StartCoroutine( DestroyOnTimer() );
            }
            coroutineMaster.StartCoroutine( AdjustTransform() );
        }

        private IEnumerator DestroyOnTimer()
        {
            yield return new WaitForSeconds( lifeTime );
            GameObject.Destroy( mInstance );
            mInstance = null;
        }

        private IEnumerator AdjustTransform()
        {
            while( true && mInstance != null)
            {
                if ( instantiateOptions.linkToBodyPart )
                {
                    if ( mSkeletonAvatar != null )
                    {
                        var bodyPart = mSkeletonAvatar.GetBoneTransform( instantiateOptions.bodyPart );
                        if ( bodyPart != null )
                        {
                            var pos = bodyPart.position;
                            var angles = bodyPart.eulerAngles;
                            mInstance.transform.position = pos + instantiateOptions.positionOffset;
                        }
                    }
                }
                yield return null; 
            }
        }

        private GameObject mInstance;
        private Animator mSkeletonAvatar;
    }
}


