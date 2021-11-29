using StartAssets.PowerfulPreview.Controls;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace StartAssets.AnimationEvents
{
    /// <summary>
    /// A drawer for the preview timeline which will draw the animation events  
    /// controls above it. 
    /// </summary>
    public class AnimationEventKeyFramesDrawer : Timeline.IDrawer
    {
        /// <summary>
        /// Sets an array of the key frame controls of the animation. 
        /// </summary>
        /// <param name="keyFrameControls"></param>
        public void SetKeyFrameControls( SortedList<float, AnimationEventKeyFrameEditor> keyFrameControls)
        {
            mKeyFrameControls = keyFrameControls;
        }

        /// <summary>
        /// Draws key frame controls on the timeline. 
        /// </summary>
        public void Draw(Rect rect)
        {
            if( mKeyFrameControls == null )
            {
                return;
            }

            var frameControlWidth = 2.0f;
            foreach( var keyFrameControl in mKeyFrameControls )
            {
                var curX = Mathf.RoundToInt(rect.width * keyFrameControl.Value.KeyFrame.InvokeTime);

                Handles.DrawSolidRectangleWithOutline
                (
                    new Rect(rect.xMin + curX - frameControlWidth / 2, rect.yMin, frameControlWidth, rect.height),
                    keyFrameControl.Value.KeyFrameColor,
                    Color.clear
                );
            }
        }

        private SortedList<float, AnimationEventKeyFrameEditor> mKeyFrameControls;
    }
}