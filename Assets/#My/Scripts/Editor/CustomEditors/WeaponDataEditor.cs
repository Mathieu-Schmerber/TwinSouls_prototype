using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using TwinSouls.Data;
using Sirenix.Utilities.Editor;
using TwinSouls.Editor.EditorWindows.PreviewWindows;
using UnityEditorInternal;
using TwinSouls.Player;
using System;
using TwinSouls.Spells;

namespace TwinSouls.Editor.CustomEditors
{
	[CustomEditor(typeof(WeaponData))]
	public class WeaponDataEditor : OdinEditor
	{
		#region Editor fields

		private List<WeaponData.WeaponAttack> _changeBuffer = new List<WeaponData.WeaponAttack>();
		private WeaponData _data;
		private bool _isClicked;

		#endregion

		public override void OnInspectorGUI()
		{
			PropertyTree tree = this.Tree;
			_data = this.target as WeaponData;

			tree.BeginDraw(true);
			Draw(tree);
			tree.EndDraw();
		}

		#region Validity

		/// <summary>
		/// Checks if an attack animation is valid
		/// </summary>
		private bool IsValid(AnimationClip clip)
		{
			string start = AnimationEvents.Events.OnMeleeAttackStart.ToString();
			string end = AnimationEvents.Events.OnMeleeAttackEnd.ToString();
			string spell = AnimationEvents.Events.OnSpellCast.ToString();
			string[] events = Enum.GetNames(typeof(AnimationEvents.Events));

			// If no clip or event doesn't exist
			if (clip == null || !(clip.events.Count(evt => events.Contains(evt.stringParameter)) == clip.events.Count()))
				return false;
			// If no hitbox event
			else if (!clip.events.Any(evt => evt.stringParameter == start) && !clip.events.Any(evt => evt.stringParameter == end))
				return true;

			return (
			// If we start hitbox detection the same amount of time we end them
			clip.events.Count(evt => evt.stringParameter == start) == clip.events.Count(evt => evt.stringParameter == end) &&
			// If the last hit box detection has an end
			(clip.events.Last(evt => evt.stringParameter == start).time < clip.events.Last(evt => evt.stringParameter == end).time)
			);
		}

		/// <summary>
		/// Checks if an attack is valid
		/// </summary>
		private bool IsValid(WeaponData.WeaponAttack attack)
		{
			return (attack.attackAnimation != null && IsValid(attack.attackAnimation));
		}
		#endregion

		#region Rendering

		/// <summary>
		/// Draws the list item toolbox
		/// </summary>
		private void RenderAttackSideToolBox(WeaponData.WeaponAttack attack, ref List<WeaponData.WeaponAttack> dataBuffer)
		{
			int index = dataBuffer.IndexOf(attack);

			GUILayout.BeginVertical(GUILayout.Width(30));
			GUILayout.Space(10);
			if (GUILayout.Button("Up") && index != 0)
			{
				WeaponData.WeaponAttack tmp = dataBuffer[index];

				dataBuffer[index] = dataBuffer[index - 1];
				dataBuffer[index - 1] = tmp;
			}
			if (GUILayout.Button(new GUIContent("", EditorGUIUtility.IconContent("P4_DeletedLocal").image)))
				dataBuffer.Remove(attack);
			if (GUILayout.Button("Down") && index < dataBuffer.Count - 1)
			{
				WeaponData.WeaponAttack tmp = dataBuffer[index];

				dataBuffer[index] = dataBuffer[index + 1];
				dataBuffer[index + 1] = tmp;
			}
			GUILayout.Space(10);
			GUILayout.EndVertical();
		}

		/// <summary>
		/// Draws the list item fields
		/// </summary>
		private void RenderAttackDataFields(ref WeaponData.WeaponAttack attack, int index)
		{
			Color baseBack = GUI.backgroundColor;
			Color baseFront = GUI.color;

			GUI.backgroundColor = IsValid(attack) ? baseBack : Color.red;
			SirenixEditorGUI.BeginBox($"Attack n° {index + 1}");
			{
				GUI.backgroundColor = baseBack;
				attack.onHitDamage = SirenixEditorFields.IntField("Melee on-hit damages", attack.onHitDamage);
				// Animation field
				GUILayout.BeginHorizontal();
				{
					GUI.color = IsValid(attack) ? baseFront : Color.red;
					attack.attackAnimation = (AnimationClip)EditorGUILayout.ObjectField("Animation", attack.attackAnimation, typeof(AnimationClip), false);
					if (attack.attackAnimation != null)
					{
						if (GUILayout.Button(new GUIContent("", EditorGUIUtility.IconContent("animationvisibilitytoggleon").image), GUILayout.Width(50)))
							AnimationPreviewWindow.OpenWindow(attack.attackAnimation);
						else if (GUILayout.Button(new GUIContent("", EditorGUIUtility.IconContent("d_editicon.sml").image), GUILayout.Width(50)))
						{
							EditorGUIUtility.PingObject(AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GetAssetPath(attack.attackAnimation)));
							Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GetAssetPath(attack.attackAnimation));
						}
					}
					GUI.color = baseFront;
				}
				GUILayout.EndHorizontal();

				// Spell to cast field
				GUILayout.BeginHorizontal();
				{
					attack.spellToCast.spellPrefab = (GameObject)EditorGUILayout.ObjectField("Attack Spell", attack.spellToCast.spellPrefab, typeof(GameObject), false);
					if (attack.spellToCast.spellPrefab != null)
					{
						if (GUILayout.Button(new GUIContent("", EditorGUIUtility.IconContent("animationvisibilitytoggleon").image), GUILayout.Width(50))
						 || GUILayout.Button(new GUIContent("", EditorGUIUtility.IconContent("d_editicon.sml").image), GUILayout.Width(50)))
						{
							EditorGUIUtility.PingObject(AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GetAssetPath(attack.spellToCast.spellPrefab)));
							Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GetAssetPath(attack.spellToCast.spellPrefab));
						}
					}
				}
				GUILayout.EndHorizontal();

				if (attack.spellToCast.spellPrefab != null)
				{
					GUI.backgroundColor = Color.black;
					SirenixEditorGUI.BeginBox("Projectile Description");
					{
						GUI.backgroundColor = baseBack;
						attack.spellToCast.descriptor.targetMode = (ASpellDescriptor.TargetMode)EditorGUILayout.EnumPopup("Target", attack.spellToCast.descriptor.targetMode);
						attack.spellToCast.descriptor.damage = EditorGUILayout.FloatField("Damage", attack.spellToCast.descriptor.damage);
						if (attack.spellToCast.descriptor.importCasterElement = EditorGUILayout.Toggle("Use caster element", attack.spellToCast.descriptor.importCasterElement))
							attack.spellToCast.descriptor.importBoosts = EditorGUILayout.Toggle("Use caster boost", attack.spellToCast.descriptor.importBoosts);
						if (attack.spellToCast.descriptor.piercing = EditorGUILayout.Toggle("Can pierce", attack.spellToCast.descriptor.piercing))
							attack.spellToCast.descriptor.maxEntityPierce = SirenixEditorFields.IntField("Max entity pierce", attack.spellToCast.descriptor.maxEntityPierce);
						attack.spellToCast.descriptor.speed = SirenixEditorFields.FloatField("Speed", attack.spellToCast.descriptor.speed);
						attack.spellToCast.descriptor.onHitFx = (GameObject)EditorGUILayout.ObjectField("Hit FX", attack.spellToCast.descriptor.onHitFx, typeof(GameObject), false);
						GUILayout.BeginHorizontal();
							EditorGUILayout.LabelField($"(Debug) Effects({attack.spellToCast.descriptor.effects.Count})");
						if (GUILayout.Button("Reset"))
							attack.spellToCast.descriptor.effects.Clear();
						GUILayout.EndHorizontal();
					}
					SirenixEditorGUI.EndBox();
				}
			}
			SirenixEditorGUI.EndBox();
		}

		/// <summary>
		/// Draws the list
		/// </summary>
		private List<WeaponData.WeaponAttack> RenderElementValues()
		{
			List<WeaponData.WeaponAttack> dataBuffer = _changeBuffer;

			for (int i = 0; i < _changeBuffer.Count; i++)
			{
				WeaponData.WeaponAttack attack = _changeBuffer[i];

				GUILayout.BeginHorizontal();
				{
					RenderAttackSideToolBox(attack, ref dataBuffer);
					RenderAttackDataFields(ref attack, i);
				}
				GUILayout.EndHorizontal();
				EditorGUILayout.Space();
			}
			return dataBuffer;
		}

		/// <summary>
		/// Draws the custom editor
		/// </summary>
		private void Draw(PropertyTree tree)
		{
			Color baseBack = GUI.backgroundColor;

			_data.mesh = (Mesh)EditorGUILayout.ObjectField("Mesh", _data.mesh, typeof(Mesh), false);
			_data.comboIntervalTime = SirenixEditorFields.FloatField("Time interval to combo", _data.comboIntervalTime);
			_data.attackSpeed = SirenixEditorFields.FloatField("Attack Speed", _data.attackSpeed);

			EditorGUILayout.Space();

			_changeBuffer = _data.attackCombos;
			GUI.backgroundColor = Color.black;
			SirenixEditorGUI.BeginBox();
			{
				GUI.backgroundColor = baseBack;
				if (GUILayout.Button(new GUIContent("Add Attack", EditorGUIUtility.IconContent("d_Toolbar Plus").image), GUILayout.Width(100)))
					_changeBuffer.Add(new WeaponData.WeaponAttack() { spellToCast = new WeaponData.WeaponSpell() });
				_changeBuffer = RenderElementValues();
			}
			SirenixEditorGUI.EndBox();

			_data.attackCombos = _changeBuffer;

			if (GUILayout.Button("Save Changes"))
			{
				EditorUtility.SetDirty(_data);
				AssetDatabase.SaveAssets();
			}
		}

		#endregion
	}
}