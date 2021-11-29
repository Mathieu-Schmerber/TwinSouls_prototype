using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;

namespace TwinSouls.Editor.EditorWindows.Encyclopedia.Creators 
{
    public abstract class ASerializedScriptableCreator : IScriptableCreator
    {
        [ShowInInspector]
        public string Name { get; set; }

        [HideInInspector]
        public string Path { get; set; }

        [ShowInInspector, InlineEditor(InlineEditorModes.GUIOnly, Expanded = true)]
        public ScriptableObject Data { get; set; }

        public ASerializedScriptableCreator(string path) => Path = path;

        private void SaveAsset()
        {
            AssetDatabase.CreateAsset(Data, System.IO.Path.Combine(Path, Name + ".asset"));
            AssetDatabase.SaveAssets();
        }

        [Button("Create new", Style = ButtonStyle.Box)]
        protected virtual void CreateNewData() => SaveAsset();
    }
}