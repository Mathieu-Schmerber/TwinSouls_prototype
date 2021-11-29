using System;
using System.Text;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace TwinSouls.Editor.ScriptGenerator
{
    public sealed class ScriptGenerator : UnityEditor.AssetModificationProcessor
    {
        public enum ClassType
		{
            DEFAULT,
            ABSTRACT,
            INTERFACE,
            SCRIPTABLE,
            MENU_EDITOR
        }

        private const bool ExcludeScriptsFromNamespace = true;
        private const bool MakeClassSealedByDefault = true;
        private const string RootScriptsFolderName = "Scripts";
        private const string TemplateFolder = "#My/Scripts/Editor/ScriptGenerator/Templates";

        private static readonly string[] _DefaultClassUsings = { "UnityEngine" };
        private static readonly string[] _DefaultInterfaceUsings = { };

        /// <summary>
        ///  This gets called for every .meta file created by the Editor.
        /// </summary>
        public static void OnWillCreateAsset(string path)
        {
            //path = path.Replace(".meta", string.Empty);

            //if (!IsScriptAsset(path))
            //    return;
            //string filePath = ResolveSystemPathFromProjectPath(path);
            //string scriptName = Path.GetFileNameWithoutExtension(path);
            //ClassType type = GetClassType(scriptName);
            //string content = GetContentForType(type).Replace("#NAMESPACE#", ResolveNamespaceFromPath(filePath)).Replace("#SCRIPTNAME#", scriptName);
            

            //File.WriteAllText(filePath, content);
        }

        private static string GetContentForType(ClassType type)
		{
            Dictionary<ClassType, string> templates = new Dictionary<ClassType, string>();
            templates.Add(ClassType.DEFAULT, "Class.txt");
            templates.Add(ClassType.ABSTRACT, "Abstract.txt");
            templates.Add(ClassType.INTERFACE, "Interface.txt");
            templates.Add(ClassType.SCRIPTABLE, "Scriptable.txt");
            templates.Add(ClassType.MENU_EDITOR, "MenuEditorWindow.txt");

            string selection = Path.Combine(Application.dataPath, TemplateFolder, templates[type]);
            return File.ReadAllText(selection);
        }

        private static ClassType GetClassType(string scriptName)
		{
            ClassType detection = ClassType.DEFAULT;

            if (!(scriptName.Length > 2)) return detection;
            else if (scriptName.StartsWith("I") && char.IsUpper(scriptName[1]))
                detection = ClassType.INTERFACE;
            else if (scriptName.StartsWith("A") && char.IsUpper(scriptName[1]))
                detection = ClassType.ABSTRACT;
            else if (scriptName.EndsWith("Data"))
                detection = ClassType.SCRIPTABLE;
            else if (scriptName.EndsWith("MenuEditor"))
                detection = ClassType.MENU_EDITOR;
            else
                return ClassType.DEFAULT;

            if (EditorUtility.DisplayDialog($"{detection} ?", $"Do you want to create a class type {detection} ?", "Yes", "No"))
                return detection;
            return ClassType.DEFAULT;
        }

        private static bool IsScriptAsset(string path) => path.EndsWith(".cs");

        private static string ResolveNamespaceFromPath(string pathInProject)
        {
            var rootScriptsFolderName = $"/{RootScriptsFolderName}/";
            pathInProject = pathInProject.Substring(pathInProject.IndexOf(rootScriptsFolderName) + rootScriptsFolderName.Length);

            var lastDirectoryIndex = pathInProject.LastIndexOf("/");
            if (lastDirectoryIndex > 0)
            {
                pathInProject = pathInProject.Substring(0, lastDirectoryIndex);

                if (ExcludeScriptsFromNamespace)
                {
                    pathInProject = pathInProject.Replace("#My/Scripts/", "/").Replace('/', '.');
                }
            }
            else
            {
                pathInProject = string.Empty;
            }

            var fullNamespace = $"TwinSouls.{pathInProject}";
            return fullNamespace;
        }

        private static string ResolveSystemPathFromProjectPath(string projectPath)
        {
            var systemPath = projectPath.Insert(0, Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("Assets")));
            return systemPath;
        }
    }
}
