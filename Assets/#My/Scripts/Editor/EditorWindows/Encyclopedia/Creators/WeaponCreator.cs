using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using TwinSouls.Data;
using TwinSouls.Tools;
using System;

namespace TwinSouls.Editor.EditorWindows.Encyclopedia.Creators
{
	public class WeaponCreator : ASerializedScriptableCreator
	{
		public WeaponCreator(string path) : base(path)
		{
			Data = ScriptableObject.CreateInstance<WeaponData>();
			Data.name = $"New Weapon";
			Name = Data.name;

			(Data as WeaponData).comboIntervalTime = 0.15f;
			(Data as WeaponData).attackSpeed = 1;
			(Data as WeaponData).attackCombos = new List<WeaponData.WeaponAttack>()
			{
				new WeaponData.WeaponAttack()
				{
					spellToCast = new WeaponData.WeaponSpell()
					{
						descriptor = new Spells.ProjectileDescriptor()
					}
				}
			};
		}
	}
}