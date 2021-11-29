using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;

namespace TwinSouls.Editor.EditorWindows.Encyclopedia.Creators 
{
    public class TemplatedCreator<T> : ASerializedScriptableCreator where T : ScriptableObject
    {
        public TemplatedCreator(string path) : base(path)
        {
            Data = ScriptableObject.CreateInstance<T>();
            Data.name = $"New {typeof(T)}";
            Name = Data.name;
        }
    }
}