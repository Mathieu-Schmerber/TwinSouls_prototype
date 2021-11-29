using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TwinSouls.Editor.EditorWindows.Encyclopedia.Creators 
{
    public interface IScriptableCreator
    {
		string Name { get; set; }
		string Path { get; set; }
		ScriptableObject Data { get; set; }
	}
}