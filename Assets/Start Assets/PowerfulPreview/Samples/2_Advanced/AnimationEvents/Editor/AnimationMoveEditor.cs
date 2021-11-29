using StartAssets.PowerfulPreview;
using StartAssets.PowerfulPreview.Controls;
using StartAssets.PowerfulPreview.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace StartAssets.AnimationEvents
{
    [CustomEditor(typeof(AnimationMove))]
    public class AnimationMoveEditor : PreviewEditor<AnimationMove>
    {
        [MenuItem("Assets/Create/Animation Events/Animation Move")]
        public static void CreateAnimationMove()
        {
            AssetCreator.CreateAsset<AnimationMove>("AnimationMove");
        }

        protected override void OnCreate()
        {
            mKeyFrameEditors = new SortedList<float, AnimationEventKeyFrameEditor>(new EqualKeyComparer());

            SetUpPreviewAnimator();
            ValidateInternalAssets();
            CreateKeyFrameEditors();

            mAnimationProperty = serializedObject.FindProperty("m_Animation");
            mKeyFramesProperty = serializedObject.FindProperty("m_KeyFrames");
            mStoredKeyFramesSize = mKeyFramesProperty.arraySize;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            ReleaseKeyFrameEditors();
        }
        protected override void OnGUIUpdate()
        {
            EditorGUILayout.BeginHorizontal();
            DrawAnimationField();
            EditorGUILayout.Space();
            if( GUILayout.Button( "Add Key Frame"))
            {
                AddKeyFrame(mTimeline.CurFrame);
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(10);

            DrawEvents();
            serializedObject.ApplyModifiedProperties();

            mPreviewAnimator.SampleAnimation(mTimeline.CurTime);

            if( mStoredKeyFramesSize != mKeyFramesProperty.arraySize )
            {
                ValidateInternalAssets();
                CreateKeyFrameEditors();
            }
            mStoredKeyFramesSize = mKeyFramesProperty.arraySize;
        }

        private void DrawAnimationField()
        {
            EditorGUILayout.LabelField("Animation", GUILayout.Width( 64 ));
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(mAnimationProperty, new GUIContent(""));
            if( EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }

            if( mPreviewAnimator.Animation != asset.Animation )
            {
                mPreviewAnimator.Animation = asset.Animation;
                if (asset.Animation != null)
                {
                    mTimeline.CurFrame = 0;
                    mTimeline.EndTime = asset.Animation.length;
                    mTimeline.Framerate = asset.Animation.frameRate;

                    foreach( var keyFrameEditor in mKeyFrameEditors )
                    {
                        keyFrameEditor.Value.AnimationEndFrame = mTimeline.EndFrame;
                    }
                }
            }
            
            mTimeline.Visible = asset.Animation != null;
            mPreviewAnimator.Animator.rootPosition = Vector3.zero;
            mPreviewAnimator.Animator.rootRotation = Quaternion.Euler(Vector3.zero);
        }
        private void DrawEvents()
        {
            EditorGUILayout.Space();

            mScrollPosition = EditorGUILayout.BeginScrollView(mScrollPosition);
            if( mKeyFrameEditors.Count > 0 )
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            }
            foreach ( var item in mKeyFrameEditors )
            {
                var editor = item.Value;

                EditorGUI.indentLevel++;
                editor.OnInspectorGUI();
                if( editor.IsPreviewable )
                {
                    var eventTime = Mathf.Max( 0.0f, mTimeline.CurTime - editor.KeyFrame.InvokeTime );
                    bool playing = eventTime > 0.0f && mTimeline.Playing;
                    editor.PreviewableEditor.DrawPreview(mPreviewAnimator.Character, preview, eventTime, playing);
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                if ( mModifiedKeyFrame )
                {
                    mModifiedKeyFrame = false;
                    break;
                }
            }
            EditorGUILayout.EndScrollView();
        }

        /// <summary>
        /// This method is called on enable of the editor, or when UnDo is executed 
        /// to check if  all of the internal assets are actually referenced inside 
        /// the asset, or they are not used anymore and we need to release them. 
        /// </summary>
        private void ValidateInternalAssets()
        {
            var usedAssets = new HashSet<UnityEngine.Object>();

            usedAssets.Add(asset);
            if( asset.KeyFrames != null )
            {
                foreach (var keyFrame in asset.KeyFrames)
                {
                    usedAssets.Add(keyFrame);
                    if (keyFrame.Event != null)
                    {
                        usedAssets.Add(keyFrame.Event);
                    }
                }
            }
            
            var internalAssets = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(asset));
            foreach (var internalAsset in internalAssets)
            {
                if( !usedAssets.Contains( internalAsset ) )
                {
                    AssetDatabase.RemoveObjectFromAsset(internalAsset);
                }
            }
            AssetDatabase.SaveAssets();
        }

        private void AddKeyFrame( int animationFrame )
        {
            var nextLength = mKeyFramesProperty.arraySize + 1;
            mKeyFramesProperty.arraySize = nextLength;

            var keyFrame = CreateInstance<AnimationEventKeyFrame>();
            keyFrame.InvokeTime = (float)animationFrame / mTimeline.EndFrame;
            keyFrame.name = "Key Frame: " + nextLength;
            keyFrame.OnInvokeTimeChanged += (prev, cur) => 
            {
                if (mKeyFrameEditors.TryGetValue(prev, out var editor))
                {
                    mKeyFrameEditors.Remove(prev);
                    mKeyFrameEditors.Add(cur, editor);
                }
            };

            AssetDatabase.AddObjectToAsset(keyFrame, asset);
            AssetDatabase.SaveAssets();

            var element = mKeyFramesProperty.GetArrayElementAtIndex(nextLength - 1);
            element.objectReferenceValue = keyFrame;

            serializedObject.ApplyModifiedProperties();

            CreateKeyFrameEditor(keyFrame);
            mEventDrawer.SetKeyFrameControls(mKeyFrameEditors);
        }
        private void DeleteKeyFrame( AnimationEventKeyFrame keyFrame )
        {
            mModifiedKeyFrame = true;

            for( int i = 0; i < mKeyFrameEditors.Values.Count; i++ )
            {
                var value = mKeyFrameEditors.Values[i];
                if( value.KeyFrame == keyFrame)
                {
                    mKeyFrameEditors.RemoveAt(mKeyFrameEditors.IndexOfValue(value));
                }
            }
            mEventDrawer.SetKeyFrameControls(mKeyFrameEditors);

            var nextLength = mKeyFramesProperty.arraySize - 1;

            int keyFrameIndex = -1;
            for( int i = 0; i < asset.KeyFrames.Length; i++ )
            {
                if( asset.KeyFrames[ i ] == keyFrame )
                {
                    keyFrameIndex = i;
                    for( int j = i + 1; j < asset.KeyFrames.Length; j++ )
                    {
                        asset.KeyFrames[j - 1] = asset.KeyFrames[j];
                    }
                    break;
                }
            }
            if( keyFrameIndex < 0 )
            {
                return;
            }

            mKeyFramesProperty.arraySize = nextLength; 
            for( int i = keyFrameIndex; i < nextLength; i++ )
            {
                mKeyFramesProperty.GetArrayElementAtIndex(i).objectReferenceValue = asset.KeyFrames[i];
            }
            AssetDatabase.RemoveObjectFromAsset(keyFrame);
            AssetDatabase.SaveAssets();
            serializedObject.ApplyModifiedProperties();
        }
        
        private void SetUpPreviewAnimator()
        {
            mPreviewAnimator = new PreviewAnimator(preview);
            mPreviewAnimator.Character = PreviewAnimator.DefaultUnityCharacter;
            mPreviewAnimator.Animation = asset.Animation;

            mTimeline = new Timeline();
            mEventDrawer = new AnimationEventKeyFramesDrawer();
            mTimeline.AddDrawer(mEventDrawer, 0);
            if (asset.Animation != null)
            {
                mTimeline.EndTime = asset.Animation.length;
                mTimeline.Framerate = asset.Animation.frameRate;
            }
            preview.AddControl(mTimeline);
        }

        private void CreateKeyFrameEditors()
        {
            if (asset.KeyFrames == null)
            {
                return;
            }

            ReleaseKeyFrameEditors();
            foreach( var keyFrame in asset.KeyFrames )
            {
                CreateKeyFrameEditor(keyFrame);
            }
            mEventDrawer.SetKeyFrameControls(mKeyFrameEditors);
        }
        private void ReleaseKeyFrameEditors()
        {
            foreach (var item in mKeyFrameEditors)
            {
                Object.DestroyImmediate(item.Value);
            }
            mKeyFrameEditors.Clear();
        }

        private AnimationEventKeyFrameEditor CreateKeyFrameEditor( AnimationEventKeyFrame keyFrame )
        {
            var editor = CreateEditor(keyFrame) as AnimationEventKeyFrameEditor;
            editor.AnimationEndFrame = mTimeline.EndFrame;
            editor.OnRemoveRequest += () =>
            {
                DeleteKeyFrame(keyFrame);
                Editor.DestroyImmediate(editor);
            };
            mKeyFrameEditors.Add( keyFrame.InvokeTime, editor);
            return editor;
        }

        /// <summary>
        /// A class that is used as a comparer for the <c>mKeyFrameEditors</c> sorted list,
        /// to allow it to use equal keys. 
        /// </summary>
        private class EqualKeyComparer : IComparer<float>
        {
            public int Compare(float x, float y)
            {
                if (x >= y)
                {
                    return 1;
                }
                return -1;
            }
        }

        private SerializedProperty mAnimationProperty;
        private SerializedProperty mKeyFramesProperty;
        private SortedList<float, AnimationEventKeyFrameEditor> mKeyFrameEditors;
        private bool mModifiedKeyFrame;

        private AnimationClip mStoredAnimation; 
        private int mStoredKeyFramesSize; 
        private Vector2 mScrollPosition; 
        
        protected Timeline mTimeline;
        private PreviewAnimator mPreviewAnimator;
        private AnimationEventKeyFramesDrawer mEventDrawer;
    }
}