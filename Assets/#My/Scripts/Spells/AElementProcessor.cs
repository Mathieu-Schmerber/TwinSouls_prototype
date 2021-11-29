using Sirenix.OdinInspector;
using System;
using TwinSouls.Data;
using TwinSouls.Entity;
using UnityEngine;

namespace TwinSouls.Spells
{
	/// <summary>
	/// Describes an object that is able to emit an element and process an incoming one.
	/// </summary>
	[RequireComponent(typeof(Damageable))]
	public abstract class AElementProcessor : MonoBehaviour
	{
		#region Properties

		[SerializeField] private ElementData.ElementType _startElement;

		/// <summary>
		/// Triggered whenever the emitted element changes.
		/// </summary>
		public event Action<ElementData.ElementType> OnEmittedElementChangedEvt;

		private ElementData.ElementType _emittedElement;

		/// <summary>
		/// Current emitted element.
		/// </summary>
		public ElementData.ElementType EmittedElement 
		{ 
			get => _emittedElement;
			protected set
			{
				if (value != _emittedElement)
					OnEmittedElementChangedEvt?.Invoke(value);
				_emittedElement = value;
			} 
		}

		public ElementData.ElementType StartElement { get => _startElement; set => _startElement = value; }

		protected Damageable _damageable;
		
		#endregion

		protected virtual void Awake()
		{
			_damageable = GetComponent<Damageable>();
		}

		protected virtual void Start()
		{
			EmittedElement = StartElement; // assigned from inspector
		}

		protected void RaiseEmittedElementEvent(ElementData.ElementType data) => OnEmittedElementChangedEvt?.Invoke(data);

		/// <summary>
		/// Overrideable setter for the EmittedElement.
		/// </summary>
		/// <param name="elementType"></param>
		public virtual void UpdateEmittedElement(ElementData.ElementType elementType) => EmittedElement = elementType;

		/// <summary>
		/// Called when an element is penetrating the gameobject.
		/// </summary>
		/// <param name="inputEffect"></param>
		/// <param name="caster"></param>
		public abstract void ProcessEffect(EffectData inputEffect, Transform caster);

		public abstract void ProcessElementalDamage(ElementData.ElementType element, float damage, Transform caster);
	}
}
