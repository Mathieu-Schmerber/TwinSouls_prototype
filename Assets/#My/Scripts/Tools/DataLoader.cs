using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;
using TwinSouls.Data;

namespace TwinSouls.Tools
{
    public class DataLoader
    {
		#region Paths

		public const string RESOURCES_FOLDER = "Assets/#My/Resources/";

		public class Constant
		{
			public static string ResourcesPath { get => "Data/Constants"; }
			public static string AssetsPath { get => Path.Combine(RESOURCES_FOLDER, ResourcesPath); }
		}

		public class Weapon
		{
			public static string ResourcesPath { get => "Data/Weapons"; }
			public static string AssetsPath { get => Path.Combine(RESOURCES_FOLDER, ResourcesPath); }
		}

		public class Element
		{
			public static string ResourcesPath { get => "Data/Elements"; }
			public static string AssetsPath { get => Path.Combine(RESOURCES_FOLDER, ResourcesPath); }
		}

		public class Effect
		{
			public static string ResourcesPath { get => "Data/Effects"; }
			public static string AssetsPath { get => Path.Combine(RESOURCES_FOLDER, ResourcesPath); }
		}

		public class Template
		{
			public const string ANIMATION = "AnimationTemplate";

			public static string ResourcesPath { get => "Prefabs/Templates"; }
			public static string AssetsPath { get => Path.Combine(RESOURCES_FOLDER, ResourcesPath); }
			public static string GetByName(string name) => Path.Combine(ResourcesPath, name);
		}

		public class Spawnable
		{
			public static string PLAYER { get => GetResourceAssetPath("Twin"); }

			public static string ResourcesPath { get => "Prefabs/Spawnables"; }
			public static string AssetsPath { get => Path.Combine(RESOURCES_FOLDER, ResourcesPath); }
			public static string GetResourceAssetPath(string asset) => Path.Combine(ResourcesPath, asset);
			public static T Load<T>(string asset) where T : Object
			{
				return Resources.Load<T>(GetResourceAssetPath(asset));
			}
		}

		#endregion

		#region Singleton

		private static DataLoader _instance;

		public static DataLoader Instance
		{
			get => _instance == null ? _instance = new DataLoader() : _instance;
		}


		#endregion

		#region Resources caching

		private ConstantData _constant;
		public ConstantData Constants
		{
			get => _constant == null ? _constant = Resources.LoadAll<ConstantData>(Constant.ResourcesPath).First() : _constant;
		}

		private IEnumerable<WeaponData> _weapons;
		public IEnumerable<WeaponData> Weapons
		{
			get => _weapons == null ? _weapons = Resources.LoadAll<WeaponData>(Weapon.ResourcesPath) : _weapons;
		}

		private IEnumerable<ElementData> _elements;
		public IEnumerable<ElementData> Elements
		{
			get => _elements == null ? _elements = Resources.LoadAll<ElementData>(Element.ResourcesPath) : _elements;
		}

		private IEnumerable<EffectData> _effects;
		public IEnumerable<EffectData> Effects
		{
			get => _effects == null ? _effects = Resources.LoadAll<EffectData>(Effect.ResourcesPath) : _effects;
		}

		private Controls _controls;
		public Controls Controls
		{
			get => _controls == null ? _controls = new Controls() : _controls;
		}

		private ControlsMapData _controlsMap;
		public ControlsMapData ControlsMap
		{
			get => _controlsMap == null ? _controlsMap = Resources.LoadAll<ControlsMapData>(Constant.ResourcesPath).First() : _controlsMap;
		}


		#endregion

		#region Load Specific

		public static ElementData GetElementOfType(ElementData.ElementType type) =>
			Instance.Elements.FirstOrDefault(element => element.type == type);

		public static EffectData GetEffectOfType(EffectData.EffectType type) =>
			Instance.Effects.FirstOrDefault(effect => effect.type == type);

		public static EffectData GetEffectOfFusion(EffectData A, EffectData B) =>
			Instance.Effects.FirstOrDefault(effect => !effect.isPrimary && effect.fusion.Contains(A) && effect.fusion.Contains(B));

		public static EffectData GetEffectFromElementType(ElementData.ElementType type) =>
			Instance.Effects.FirstOrDefault(effect => effect.isPrimary && effect.element == type);

		public static GameObject GetTemplateOfName(string name) => Resources.Load<GameObject>(Template.GetByName(name));

		public static GameObject GetResource(string path) => Resources.Load<GameObject>(path);

		#endregion
	}
}