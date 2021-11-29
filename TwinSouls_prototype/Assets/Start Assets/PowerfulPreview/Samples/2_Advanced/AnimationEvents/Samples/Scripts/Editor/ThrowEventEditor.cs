using System;
using System.Collections;
using System.Collections.Generic;
using StartAssets.PowerfulPreview;
using UnityEditor;
using UnityEngine;

namespace StartAssets.AnimationEvents.Samples
{
    [CustomEditor(typeof( ThrowEvent ))]
    public class ThrowEventEditor : AnimationEventEditor, IPreviewable
    {
        public override Color KeyFrameColor
        {
            get
            {
                return Color.blue;
            }
        }

        public void DrawPreview( GameObject parent, Preview preview, float currentTime, bool playing )
        {
            var throwEvent = target as ThrowEvent; 
            if( mParticleSystemProvider == null )
            {
                if( throwEvent.prefab != null )
                {
                    var instance = preview.Scene.Instantiate( throwEvent.prefab );
                    mParticleSystemProvider = new ParticleSystemAnimator( instance );
                }
            }

            var skeletonAvatar = parent.GetComponent<Animator>();
            if (skeletonAvatar != null && mParticleSystemProvider != null)
            {
                Vector3 origin = parent.transform.position;

                if (throwEvent.origin.linkToBodyPart)
                {
                    origin = skeletonAvatar.GetBoneTransform( throwEvent.origin.bodyPart ).position;
                }
                origin += throwEvent.origin.positionOffset;

                var realPos = origin + throwEvent.direction * currentTime;
                mParticleSystemProvider.GameObject.transform.position = realPos;

                mParticleSystemProvider.Simulate(currentTime);
            }
        }

        private ParticleSystemAnimator mParticleSystemProvider;
        private float mPrevTime; 
    }
}