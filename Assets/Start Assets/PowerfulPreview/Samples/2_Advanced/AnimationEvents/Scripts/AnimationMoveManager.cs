using System.Collections.Generic;
using UnityEngine;

namespace StartAssets.AnimationEvents
{
    [RequireComponent( typeof( Animator ) )]
    public class AnimationMoveManager : MonoBehaviour
    {
        public AnimationMove[] movements;

        private void Awake()
        {
            mAnimator = GetComponent<Animator>();
            mMovements = new Dictionary<int, AnimationMove>();
            foreach ( var movement in movements )
            {
                mMovements[ movement.Animation.GetHashCode() ] = movement;
            }
            mPrevAnimationTime = 0.0f;
        }

        private void LateUpdate()
        {
            for ( int iLayer = 0; iLayer < mAnimator.layerCount; iLayer++ )
            {
                var clipsInfo = mAnimator.GetCurrentAnimatorClipInfo( iLayer );
                foreach ( var clipInfo in clipsInfo )
                {
                    var hash = clipInfo.clip.GetHashCode();
                    if ( mCurrentMove != null && mCurrentMove.Animation.GetHashCode() == hash )
                    {
                        var time = mAnimator.GetCurrentAnimatorStateInfo( iLayer ).normalizedTime % 1.0f;
                        var delta = Mathf.Abs( time - mPrevAnimationTime );
                        mPrevAnimationTime = time;
                        if ( delta > 0.95f && mAnimator.GetCurrentAnimatorStateInfo( iLayer ).loop )
                        {
                            mCurrentMove.PrepareEvents();
                        }
                        mCurrentMove.Handle( gameObject, time, this );
                    }
                    else
                    {
                        if ( mMovements.TryGetValue( hash, out mCurrentMove ) )
                        {
                            mCurrentMove.PrepareEvents();
                        }
                    }
                }
            }
        }

        private Animator mAnimator;
        private Dictionary<int, AnimationMove> mMovements;
        private AnimationMove mCurrentMove;

        private float mPrevAnimationTime;
    }

}