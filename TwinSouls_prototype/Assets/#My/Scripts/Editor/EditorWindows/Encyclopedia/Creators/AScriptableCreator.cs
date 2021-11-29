using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;

namespace TwinSouls.Editor.EditorWindows.Encyclopedia.Creators
{
    public abstract class AScriptableCreator : IScriptableCreator
    {
        [ShowInInspector]
		public string Name { get; set; }
		public string Path { get; set; }
        public ScriptableObject Data { get; set; }

        public AScriptableCreator(string path) => Path = path;
        
        private void SaveAsset()
		{
            AssetDatabase.CreateAsset(Data, System.IO.Path.Combine(Path, Name + ".asset"));
            AssetDatabase.SaveAssets();
        }

        [Button("Create new", Style = ButtonStyle.Box)]
        protected virtual void CreateNewData() => SaveAsset();
    }
}