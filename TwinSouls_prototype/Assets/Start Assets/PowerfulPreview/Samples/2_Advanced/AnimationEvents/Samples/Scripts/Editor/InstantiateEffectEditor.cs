using StartAssets.AnimationEvents.Samples;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using StartAssets.PowerfulPreview;
using System;

namespace StartAssets.AnimationEvents.Samples
{
    [CustomEditor( typeof( InstantiateEffectEvent) )]
    public class InstantiateEffectEventEditor : AnimationEventEditor, IPreviewable
    {
        public override Color KeyFrameColor
        {
            get
            {
                return Color.red; 
            }
        }

        public void DrawPreview( GameObject parent, Preview preview, float currentTime, bool playing )
        {
            var effect = target as InstantiateEffectEvent;
            
            var changedEffectPrefab = mStoredEffect != effect.effectPrefab;
            if( changedEffectPrefab && mPreviewObject != null )
            {
                preview.Scene.DestroyInstance( mPreviewObject );
                mPreviewObject = null; 
            }
            if ( effect.effectPrefab != null && mPreviewObject == null )
            {
                mStoredEffect = effect.effectPrefab;
                mPreviewObject = GameObject.Instantiate( effect.effectPrefab, parent.transform.position, Quaternion.Euler( Vector3.zero ) );
                mPreviewObject.hideFlags = HideFlags.HideAndDontSave; 
                mParticleSystems = mPreviewObject.GetComponentsInChildren<ParticleSystem>();
                preview.Scene.AddObject( mPreviewObject, false );
            }

            if( mPreviewObject != null )
            {
                mPreviewObject.SetActive( true );
                if ( effect.destroyOnTimer )
                {
                    mPreviewObject.SetActive( currentTime < effect.lifeTime );
                }
            }
            
            if ( mPreviewObject == null || !mPreviewObject.activeSelf )
            {
                return;
            }

            var skeletonAvatar = parent.GetComponent<Animator>();
            if (skeletonAvatar != null && effect != null)
            {
                if (effect.instantiateOptions.linkToBodyPart)
                {
                    var bodyPartTransform = skeletonAvatar.GetBoneTransform(effect.instantiateOptions.bodyPart);
                    mPreviewObject.transform.position = bodyPartTransform.position + effect.instantiateOptions.positionOffset;
                }
                else
                {
                    mPreviewObject.transform.position = parent.transform.position + effect.instantiateOptions.positionOffset;
                }

                if( !Mathf.Approximately( mPrevTime, currentTime ) )
                {
                    foreach (var ps in mParticleSystems)
                    {
                        ps.Simulate(currentTime, true, true, true);
                    }
                }
            }

            mPrevTime = currentTime; 
        }

        private GameObject mPreviewObject;
        private GameObject mStoredEffect;
        private float mPrevTime; 
        private ParticleSystem[] mParticleSystems; 
    }
}