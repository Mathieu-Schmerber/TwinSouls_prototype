using StartAssets.PowerfulPreview;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace StartAssets.AnimationEvents
{
    [CustomEditor(typeof(AnimationEventKeyFrame))]
    public class AnimationEventKeyFrameEditor : Editor
    {
        /// <summary>
        /// Is invoked when user clicks on the delete ("X") button. 
        /// </summary>
        public event Action OnRemoveRequest; 
       
        /// <summary>
        /// End of the animation which is associated with the animation event, 
        /// used to compute <c>InvokeTime</c>
        /// </summary>
        public int AnimationEndFrame
        {
            set;
            get;
        }
        /// <summary>
        /// Assigned key frame to the editor
        /// </summary>
        public AnimationEventKeyFrame KeyFrame
        {
            get
            {
                if( mKeyFrame == null )
                {
                    mKeyFrame = target as AnimationEventKeyFrame;
                }
                return mKeyFrame;
            }
        }
        /// <summary>
        /// A color of the assigned key frame, can be changed with custom event editors.
        /// </summary>
        public Color KeyFrameColor
        {
            private set;
            get;
        }
        /// <summary>
        /// Returns a cast to IPreviewable of the event editor,
        /// if it's not IPreviewable then returns null. 
        /// </summary>
        public IPreviewable PreviewableEditor
        {
            private set;
            get;
        }
        public bool IsPreviewable
        {
            get
            {
                return PreviewableEditor != null; 
            }
        }

        public override void OnInspectorGUI()
        {
            //Header of the key frame 
            EditorGUILayout.BeginHorizontal();
            {
                mVisible = EditorGUILayout.Foldout(mVisible, "Key Frame", true );
                DrawKeyFrameSelector();
                DrawEventTypeSelector();
                DrawUsePrefabSelector();
                DrawDeleteButton();
            }
            EditorGUILayout.EndHorizontal();
            serializedObject.ApplyModifiedPropertiesWithoutUndo();

            //Draws editor of the assigned event. 
            if( mVisible )
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space();
                EditorGUI.BeginDisabledGroup(KeyFrame.UsePrefab);
                mEventEditor?.OnInspectorGUI();
                EditorGUI.EndDisabledGroup();
                EditorGUI.indentLevel--;
            }
        }

        private void OnEnable()
        {
            if( KeyFrame == null || KeyFrame.Event == null )
            {
                mCurrentEventType = 0;
                mEventEditor = null;
                KeyFrameColor = Color.grey;
            }
            else
            {
                mCurrentEventType = AnimationEventsCollection.Instance.GetIndex(KeyFrame.Event.GetType());
                CreateEditorForEvent(KeyFrame.Event);
            }

            mAnimationEventProperty = serializedObject.FindProperty("m_AnimationEvent");
            AnimationEventsCollection.Instance.Update();
        }

        private void DrawKeyFrameSelector()
        {
            EditorGUI.BeginChangeCheck();
            var frame = Mathf.RoundToInt(KeyFrame.InvokeTime * AnimationEndFrame);
            frame = EditorGUILayout.DelayedIntField(frame, GUILayout.Width(60));
            if (EditorGUI.EndChangeCheck())
            {
                var normalizedTime = (float)frame / AnimationEndFrame;
                serializedObject.FindProperty("m_InvokeTime").floatValue = normalizedTime;
                KeyFrame.InvokeTime = normalizedTime;
            }
        }
        private void DrawEventTypeSelector()
        {
            if (KeyFrame.UsePrefab)
            {
                EditorGUILayout.PropertyField(mAnimationEventProperty);
            }
            else
            {
                EditorGUI.BeginChangeCheck();
                var names = AnimationEventsCollection.Instance.EventNames;
                mCurrentEventType = EditorGUILayout.Popup(mCurrentEventType, names, GUILayout.Width( 180) );
                if (EditorGUI.EndChangeCheck())
                {
                    CreateEvent();      
                }
            }
        }
        private void DrawUsePrefabSelector()
        {
            EditorGUILayout.LabelField("Use Prefab", GUILayout.Width( 80 ) );
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_UsePrefab"), EmptyPrefixLabel, GUILayout.Width(32));
            if (EditorGUI.EndChangeCheck())
            {
                DeleteEvent();
                serializedObject.FindProperty("m_AnimationEvent").objectReferenceValue = null;
            }
        }
        private void DrawDeleteButton()
        {
            if (GUILayout.Button("X", GUILayout.Width(18), GUILayout.Height(16)))
            {
                DeleteEvent();   
                OnRemoveRequest?.Invoke();
            }
            GUILayout.Space(5);
        }

        private void CreateEvent()
        {
            DeleteEvent();

            AnimationEvent changedEvent = null;
            if (mCurrentEventType != 0)
            {
                var names = AnimationEventsCollection.Instance.EventNames;

                changedEvent = AnimationEventsCollection.Instance.Create(names[mCurrentEventType]);
                AssetDatabase.AddObjectToAsset(changedEvent, KeyFrame);
                AssetDatabase.SaveAssets();

                CreateEditorForEvent(changedEvent);
            }
            mAnimationEventProperty.objectReferenceValue = changedEvent;
        }
        private void DeleteEvent()
        {
            if (KeyFrame.Event != null && !KeyFrame.UsePrefab)
            {
                AssetDatabase.RemoveObjectFromAsset(KeyFrame.Event);
                DestroyImmediate(KeyFrame.Event);
                AssetDatabase.SaveAssets();

                KeyFrame.Event = null;
                mAnimationEventProperty.objectReferenceValue = null;
                mCurrentEventType = 0;
                serializedObject.ApplyModifiedProperties();
            }
            if( mEventEditor != null )
            {
                DestroyImmediate(mEventEditor);
                mEventEditor = null; 
            }
        }
        private void CreateEditorForEvent( AnimationEvent e )
        {
            mEventEditor = CreateEditor(e);
            PreviewableEditor = mEventEditor as IPreviewable; 

            var animationEventEditor = mEventEditor as AnimationEventEditor;
            KeyFrameColor = animationEventEditor != null
                    ? animationEventEditor.KeyFrameColor
                    : Color.grey;
        }

        private Editor mEventEditor;
        private SerializedProperty mAnimationEventProperty; 
        private AnimationEventKeyFrame mKeyFrame;

        private bool mVisible; 
        private int mCurrentEventType;

        private static readonly GUIContent EmptyPrefixLabel = new GUIContent(""); 
    }
}
