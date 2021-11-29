using UnityEngine;
using UnityEditor;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using TwinSouls.Tools;
using TwinSouls.Data;
using System.Collections.Generic;
using TwinSouls.Editor.EditorWindows.Encyclopedia.Creators;
using System;

namespace TwinSouls.Editor.EditorWindows.Encyclopedia 
{
    public class EncyclopediaMenuEditor : OdinMenuEditorWindow
    {
        #region Types



        #endregion

        #region Properties

        List<IScriptableCreator> garbageCollector;

        #endregion

        [MenuItem("Twin Souls/Encyclopedia")]
        private static void OpenWindow() => GetWindow<EncyclopediaMenuEditor>("Encyclopedia").Show();

        /// <summary>
        /// Delete created but not confirmed instances
        /// </summary>
		protected override void OnDestroy()
		{
			base.OnDestroy();

            garbageCollector.ForEach(g =>
            {
                if (g?.Data)
                    DestroyImmediate(g.Data);
            });
		}

        /// <summary>
        /// Builds the side bar
        /// </summary>
		protected override OdinMenuTree BuildMenuTree()
        {
            OdinMenuTree tree = new OdinMenuTree();
            garbageCollector = new List<IScriptableCreator>();

            tree.Config.DrawSearchToolbar = true;

            // Elements
            {
                string category = "Elements";
                string assetPath = DataLoader.Element.AssetsPath;

                garbageCollector.Add(new TemplatedCreator<ElementData>(assetPath));
                tree.Add($"{category}/Create New", garbageCollector.Last(), EditorGUIUtility.IconContent("CreateAddNew").image);
                tree.AddAllAssetsAtPath(category, assetPath, typeof(ElementData));
            }

            // Effects
            {
                string category = "Effects";
                string assetPath = DataLoader.Effect.AssetsPath;

                garbageCollector.Add(new TemplatedCreator<EffectData>(assetPath));
                tree.Add($"{category}/Create New", garbageCollector.Last(), EditorGUIUtility.IconContent("CreateAddNew").image);
                tree.AddAllAssetsAtPath(category, assetPath, typeof(EffectData));
            }

            // Weapons
            {
                string category = "Weapons";
                string assetPath = DataLoader.Weapon.AssetsPath;

                garbageCollector.Add(new WeaponCreator(assetPath));
                tree.Add($"{category}/Create New", garbageCollector.Last(), EditorGUIUtility.IconContent("CreateAddNew").image);
                tree.AddAllAssetsAtPath(category, assetPath, typeof(WeaponData));
            }

            return tree;
        }

        /// <summary>
        /// Builds a tool bar
        /// </summary>
		protected override void OnBeginDrawEditors()
		{
            if (MenuTree == null) return;
            OdinMenuItem selected = this.MenuTree.Selection.FirstOrDefault();
            OdinMenuTreeSelection selection = this.MenuTree.Selection;

            if (selected == null || selection == null)
                return;
            SirenixEditorGUI.BeginHorizontalToolbar();
			{
                GUILayout.Label(selected.Name);
                GUILayout.FlexibleSpace();
                if (selection.SelectedValue is ScriptableObject && SirenixEditorGUI.ToolbarButton("Delete Current"))
				{
                    ScriptableObject asset = selection.SelectedValue as ScriptableObject;
                    string path = AssetDatabase.GetAssetPath(asset);

                    AssetDatabase.DeleteAsset(path);
                    AssetDatabase.SaveAssets();
				}
			}
            SirenixEditorGUI.EndHorizontalToolbar();
		}
	}
}
