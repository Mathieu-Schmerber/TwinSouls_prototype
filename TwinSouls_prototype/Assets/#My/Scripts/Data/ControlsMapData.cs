using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RotaryHeart.Lib.SerializableDictionary;
using Sirenix.OdinInspector;
using TwinSouls.Tools;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TwinSouls.Data
{
	[CreateAssetMenu()]
	public class ControlsMapData : SerializedScriptableObject
	{
		public enum ControllerType
		{
			KEYBOARD,
			XBOX,
			PS4
		}

		[Serializable]
		public class ControlsMapRow
		{
			[HorizontalGroup("Actions"), HideLabel, ReadOnly]
			public string action;

			[HorizontalGroup("Keyboard"), TableColumnWidth(70, Resizable = false), HideLabel, PreviewField]
			public Sprite keyboardKey;

			[HorizontalGroup("XBOX"), TableColumnWidth(70, Resizable = false), HideLabel, PreviewField]
			public Sprite xboxKey;

			[HorizontalGroup("PS4"), TableColumnWidth(70, Resizable = false), HideLabel, PreviewField]
			public Sprite ps4Key;
		}


		[TableList]
		public List<ControlsMapRow> ControlsMapTable = new List<ControlsMapRow>();

		public Sprite GetInputSprite(string action, ControllerType controller)
		{
			ControlsMapRow row = ControlsMapTable.FirstOrDefault(x => x.action == action);

			if (row == null)
				return null;
			switch (controller)
			{
				case ControllerType.KEYBOARD:
					return row.keyboardKey;
				case ControllerType.XBOX:
					return row.xboxKey;
				case ControllerType.PS4:
					return row.ps4Key;
			}
			return null;
		}

#if UNITY_EDITOR
		[Button("Load Controls")]
		public void LoadControls()
		{
			Controls controls = DataLoader.Instance.Controls;

			foreach (InputAction item in controls.asset.FindActionMap("Player"))
			{
				if (!ControlsMapTable.Any(x => x.action == item.name))
					ControlsMapTable.Add(new ControlsMapRow() { action = item.name });
			}
			EditorUtility.SetDirty(this);
			AssetDatabase.SaveAssets();
		}
#endif
	}
}
