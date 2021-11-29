using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StartAssets.PowerfulPreview;
using UnityEditor;
using UnityEngine;

namespace StartAssets.AnimationEvents.Samples
{
    [CustomEditor( typeof( PlaySoundEffectEvent) )]
    public class PlaySoundEffectEventEditor : AnimationEventEditor, IPreviewable
    {
        public override Color KeyFrameColor
        {
            get
            {
                return Color.green;
            }
        }

        private void OnDestroy()
        {
            if ( mAudioSource != null )
            {
                GameObject.DestroyImmediate( mAudioSource.gameObject );
                mAudioSource = null;
            }
        }

        public void DrawPreview( GameObject parent, Preview preview, float currentTime, bool playing )
        {
            var effect = target as PlaySoundEffectEvent;
            if( effect.audioClip == null || !effect.PreviewSound )
            {
                return;
            }

            if( mAudioSource == null )
            {
                var gameObject = new GameObject( "AudioSource" );
                gameObject.hideFlags = HideFlags.HideAndDontSave; 
                mAudioSource = gameObject.AddComponent<AudioSource>();
            }

            var deltaTime = currentTime - mPrevTime;
            mPrevTime = currentTime;

            if( !playing || mAudioSource.isPlaying || currentTime > effect.audioClip.length )
            {
                if( !playing)
                {
                    mAudioSource.Stop();
                }
                return;
            }
            mAudioSource.PlayOneShot( effect.audioClip );
        }

        private AudioSource mAudioSource;
        private float mPrevTime = 0.0f;
    }
}
