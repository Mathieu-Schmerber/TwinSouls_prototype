using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TwinSouls.Data;
using TwinSouls.Tools;
using UnityEngine;

namespace TwinSouls.Spells
{
    /// <summary>
    /// Handles element equations. <br></br>
    /// Needs an EffectPool to apply & boost effects on the entity.
    /// </summary>
    [RequireComponent(typeof(EffectPool))]
    public class ElementEffectProcessor : AElementProcessor
	{
		#region Properties
		
		public event Action OnRepressionStartEvt;
		public event Action OnRepressionEndEvt;

		public bool IsRepressed { get => EmittedElement == ElementData.ElementType.NONE; }
		
		/// <summary>
		/// Element to emit when the repression is over.
		/// </summary>
		[ShowInInspector, ReadOnly] private ElementData.ElementType _elementToEmit;

		private ElementData.ElementType? _repressor = null;

		private EffectPool _effectPool;

		#endregion

		#region Unity builtins

		protected override void Awake()
		{
			base.Awake();
            _effectPool = GetComponent<EffectPool>();
			_effectPool.OnEffectLiftedEvt += _effectPool_OnEffectLiftedEvt;
		}

		#endregion

		private void Repress(ElementData.ElementType repressor)
		{
			_effectPool.CancelBoosts();
			_elementToEmit = EmittedElement;
			EmittedElement = ElementData.ElementType.NONE;
			_repressor = repressor;

			OnRepressionStartEvt?.Invoke();
		}

		#region Public access

		public override void ProcessElementalDamage(ElementData.ElementType element, float damage, Transform caster)
		{
			ElementData attack = DataLoader.GetElementOfType(element);
			ElementData emitted = DataLoader.GetElementOfType(EmittedElement);

			if (IsRepressed || emitted.weakerThan.Contains(attack) || emitted.merge.Contains(attack))
				_damageable.ApplyDamage(caster.gameObject, damage, true);
		}

		public override void UpdateEmittedElement(ElementData.ElementType elementType)
		{
			if (IsRepressed)
				_elementToEmit = elementType;
			else
				EmittedElement = elementType;
		}

		public override void ProcessEffect(EffectData inputEffect, Transform caster)
		{
			if (inputEffect.element == ElementData.ElementType.NONE)
			{
				_effectPool.AddWithoutFiltering(inputEffect, caster);
				return;
			}
			else if (!IsRepressed)
			{
				var equation = BuildEquation(inputEffect);
				var result = ResolveEquation(equation);

				ApplyResult(inputEffect, result, EmittedElement, caster);
			}
			else
				ApplyResult(inputEffect, inputEffect.GetSingleOrFusionElements().ToList(), EmittedElement, caster);
		}

		#endregion

		#region Equation

		private List<ElementData.ElementType> BuildEquation(EffectData inputEffect)
		{
			List<ElementData.ElementType> expression = new List<ElementData.ElementType>();

			// Handle fusion effects
			if (inputEffect.isPrimary)
				expression.Add(inputEffect.element);
			else
				expression.AddRange(inputEffect.GetSingleOrFusionElements());
			expression.Add(EmittedElement);
			return expression;
		}

		private List<ElementData.ElementType> ResolveEquation(List<ElementData.ElementType> expression)
		{
			ElementData emitted = DataLoader.GetElementOfType(EmittedElement);

			// Step 1: Remove duplicates
			List<ElementData.ElementType> equation = expression.Distinct().ToList();

			// Step 2: Remove element weaker than emitted
			equation.RemoveAll(x => emitted.strongerThan.Contains(DataLoader.GetElementOfType(x)));

			// Step 3: Remove emitted element if weaker
			for (int i = 0; i < equation.Count; i++)
			{
				if (emitted.weakerThan.Contains(DataLoader.GetElementOfType(equation[i]))) {
					equation.Remove(emitted.type);
					break;
				}
			}
			return equation;
		}

		private void ApplyResult(EffectData sourceEffect, List<ElementData.ElementType> result, 
								 ElementData.ElementType emitted, Transform caster)
		{
			// Does not contain the emitted element
			if (emitted == ElementData.ElementType.NONE || !result.Contains(emitted))
			{
				result.ForEach(x => _effectPool.AddEffect(DataLoader.GetEffectFromElementType(x), caster));

				if (emitted != ElementData.ElementType.NONE)
					Repress(sourceEffect.element);
			}
			// Contains emitted element
			else
			{
				result.Remove(emitted);
				result.ForEach(x => _effectPool.AddBoost(DataLoader.GetEffectFromElementType(x), caster));
			}
		}

		private void _effectPool_OnEffectLiftedEvt(EffectData obj)
		{
			if (!IsRepressed || _repressor == null)
				return;

			EffectData repressorEffect = DataLoader.GetEffectFromElementType(_repressor.Value);

			if ((obj.isPrimary && obj == repressorEffect) || (!obj.isPrimary && obj.fusion.Contains(repressorEffect)))
			{
				EmittedElement = _elementToEmit;
				_repressor = null;

				OnRepressionEndEvt?.Invoke();
			}
		}

		#endregion
	}
}