using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using TwinSouls.Data;
using TwinSouls.Player.Kits;
using TwinSouls.Tools;
using UnityEngine.InputSystem;
using TwinSouls.Spells;
using TwinSouls.UI;
using TwinSouls.Entity;

namespace TwinSouls.Player
{
	/// <summary>
	/// Handles the vote system and element switch throughout the link. <br></br>
	/// Switches between ElementalKits, and pass over the attack animations events to them.
	/// </summary>
    public class ElementDriver : MonoBehaviour
    {
		#region Types

		#endregion

		#region Properties

		/// <summary>
		/// Inputs data
		/// </summary>
		private InputHandler _inputs;

		/// <summary>
		/// Target endpoint
		/// </summary>
		[FoldoutGroup("Elemental Link")]
		public ElementDriver endPoint;

		/// <summary>
		/// Time it takes to propagate through the whole link
		/// </summary>
		[FoldoutGroup("Elemental Link"), LabelText("Propagation Time (sec)")]
		[ShowInInspector]
		private float _propagationTime = 5f;

		/// <summary>
		/// Cashing all elements 
		/// </summary>
		private ElementData[] _elementsList;

		/// <summary>
		/// Suggested element, null if no suggestion
		/// </summary>
		[HideInInspector] public ElementData suggestion;

		/// <summary>
		/// Public to access the processor of the other twin
		/// </summary>
		[HideInInspector] public ElementEffectProcessor processor;

		/// <summary>
		/// Public to access the processor of the other twin
		/// </summary>
		[HideInInspector] public EffectPool pool;

		/// <summary>
		/// Current equiped and enabled kit
		/// </summary>
		[HideInInspector] public AMobilityKit currentKit;

		private const float MAX_PROPAGATION = 1;

		/// <summary>
		/// Current propagation state, always between 0 - 1
		/// </summary>
		private float _propagation = .5f;

		/// <summary>
		/// Allows propagation value lerping
		/// </summary>
		private float _targetPropagation;

		/// <summary>
		/// PlayerCanvas reference
		/// </summary>
		private PlayerCanvas _playerCanvas;

		/// <summary>
		/// Is ready to switch element (suggestion animation done ?)
		/// </summary>
		[HideInInspector] public bool readyToSwitch = false;

		[SerializeField] private Cooldown _suggestionCooldown;

		#endregion

		#region Unity builtins

		// Get references
		protected void Awake()
        {
			processor = GetComponent<ElementEffectProcessor>();
			pool = GetComponent<EffectPool>();
			_propagation = 0.5f;
			_targetPropagation = _propagation;
			_elementsList = DataLoader.Instance.Elements.ToArray();
			_playerCanvas = GetComponentInChildren<PlayerCanvas>();
			_inputs = GetComponent<InputHandler>();
			_inputs.OnElementVoteInputEvt += Inputs_OnElementVoteInputEvt;

			processor.OnRepressionStartEvt += Processor_OnRepressionEvt;
			processor.OnRepressionEndEvt += Processor_OnRepressionEvt;
			processor.OnEmittedElementChangedEvt += SwitchKit;
		}

		//private void Start() => SwitchKit();

		private void Update()
		{
			// If other twin did not join yet
			if (endPoint == null) return;
			
			Propagation();
		}

		#endregion

		#region Suggestions system

		#region Input assignement

		private void Inputs_OnElementVoteInputEvt(ElementData element) => MakeSuggestion(element);

		#endregion

		/// <summary>
		/// Suggestion system
		/// Make a suggestion to switch element, switch when the other soul wants to switch too
		/// </summary>
		/// <param name="suggestion"></param>
		public void MakeSuggestion(ElementData suggestion)
		{
			// Cooldown down or just one player
			if (!_suggestionCooldown.IsOver() || endPoint == null) return;

			// Have to wait the end of the suggestion animation to be ready
			readyToSwitch = false;

			this.suggestion = suggestion;
			_playerCanvas.DisplaySuggestion(suggestion);
			_suggestionCooldown.Reset();
		}

		/// <summary>
		/// Executes the kit change and link propagation change.
		/// </summary>
		public void ApplyElementChange()
		{
			// Processing elements through the truth table
			if (endPoint != null)
				ProcessElementPropagation(this.processor.EmittedElement, endPoint.processor.EmittedElement);
			OnLinkChanged();
			// If the element switch resulted to a similar state as before (between merge and repression only)
			if (_propagation == _targetPropagation && _propagation == MAX_PROPAGATION)
				OnEndpointLinked();

			// Applying kits
			//SwitchKit();
		}

		/// <summary>
		/// Proceeds the element switch after both sides suggested an element
		/// </summary>
		private void AcceptSuggestion()
		{
			// Update player canvas
			_playerCanvas.AcceptSuggestion();

			ApplyElementChange();

			_playerCanvas.AcceptSuggestion();

			// Reset suggestion
			suggestion = null;
		}

		/// <summary>
		/// Truth table computation
		/// </summary>
		/// <param name="twinElement"></param>
		public void ProcessElementPropagation(ElementData.ElementType currentElement, ElementData.ElementType twinElement)
		{
			ElementData current = DataLoader.GetElementOfType(currentElement);
			ElementData twin = DataLoader.GetElementOfType(twinElement);

			if (currentElement == ElementData.ElementType.NONE && twinElement != ElementData.ElementType.NONE)
				_targetPropagation = 0;
			else if (twinElement == ElementData.ElementType.NONE && currentElement != ElementData.ElementType.NONE)
				_targetPropagation = 1;
			else if (twinElement == currentElement && currentElement == ElementData.ElementType.NONE)
				_targetPropagation = 0.5f;
			else
			{
				// This twin element suppresses the other twin
				_targetPropagation = current.strongerThan.Contains(twin) ? 1f : _targetPropagation;
				// This twin element is repelled by the other twin
				_targetPropagation = current.weakerThan.Contains(twin) ? 0f : _targetPropagation;
				// The elements are even and make a collision midway
				_targetPropagation = current.neutral.Contains(twin) ? 0.5f : _targetPropagation;
				// The elements merge together, reaching each other twin
				_targetPropagation = current.merge.Contains(twin) ? 1f : _targetPropagation;
			}
		}

		public void SwitchKit(ElementData.ElementType element)
		{
			if (processor.IsRepressed)
				currentKit = null;
			foreach (AMobilityKit kit in GetComponents<AMobilityKit>())
			{
				kit.enabled = kit.type == element;
				currentKit = kit.enabled ? kit : currentKit;
			}
		}

		#endregion

		#region Link mechanic

		/// <summary>
		/// Calculate element propagation
		/// </summary>
		private void Propagation()
		{
			// Adapt propagation over time
			if (_targetPropagation != _propagation)
			{
				float multiplier = _targetPropagation > _propagation ? 1 : -1;

				_propagation += ((MAX_PROPAGATION / _propagationTime) * Time.deltaTime) * multiplier;

				// Checking if the targeted propagation value is close enough (1%)
				if (Mathf.Max(_targetPropagation, _propagation) - Mathf.Min(_targetPropagation, _propagation) < MAX_PROPAGATION / 100)
				{
					_propagation = _targetPropagation;
					if (_propagation == MAX_PROPAGATION)
						OnEndpointLinked();
				}
			}
			// propagation reached targeted value, and propagation is fully extended
			else if (_targetPropagation == _propagation && _propagation == MAX_PROPAGATION)
				OnEndpointLinkingStay();
		}

		/// <summary>
		/// Called when this ElementDriver link was modified
		/// </summary>
		private void OnLinkChanged()
		{

		}

		/// <summary>
		/// Called when this ElementDriver reached the other twin
		/// </summary>
		public void OnEndpointLinked()
		{

		}

		/// <summary>
		/// Called constantly while this ElementDriver is touching the other twin
		/// </summary>
		private void OnEndpointLinkingStay()
		{
			if (!processor.IsRepressed)
				endPoint.processor.ProcessEffect(DataLoader.GetEffectFromElementType(processor.EmittedElement), transform);
		}

		private void Processor_OnRepressionEvt()
		{
			this.ApplyElementChange();
			endPoint.ApplyElementChange();
		}

		/// <summary>
		/// Calculates element endpoint position depending on propagation value
		/// </summary>
		/// <returns></returns>
		public Vector3 GetEndPointProgression(float altitude)
		{
			if (endPoint == null)
				return transform.position;
			Vector3 dir = (endPoint.transform.position - transform.position).normalized;
			float distance = Vector3.Distance(endPoint.transform.position, transform.position);

			return transform.position + (dir * distance * _propagation) + new Vector3(0, altitude, 0);
		}

		#endregion

		#region Animation events

		/// <summary>
		/// Triggered by the AnimationEvents class
		/// </summary>
		/// <param name="eventType"></param>
		public void ManageGenericEvent(AAnimationEvents.Events eventType)
		{
			switch (eventType)
			{
				case AAnimationEvents.Events.OnMeleeAttackStart:
					currentKit?.OnMeleeAttackStartEvent();
					break;
				case AAnimationEvents.Events.OnSpellCast:
					currentKit?.OnSpellCastEvent();
					break;
				case AAnimationEvents.Events.OnMeleeAttackEnd:
					currentKit?.OnMeleeAttackEndEvent();
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// Triggered by animation events from the AnimationEvents class
		/// </summary>
		public void OnSuggestionAccept() => _playerCanvas.OnAcceptEnd();

		/// <summary>
		/// Triggered by animation events from the AnimationEvents class
		/// </summary>
		public void OnSuggestionEnd()
		{
			readyToSwitch = true;

			// The other twin also suggested an element
			// Suggestion validation
			if (readyToSwitch && endPoint.readyToSwitch && endPoint.suggestion != null && suggestion != null)
			{
				this.processor.UpdateEmittedElement(this.suggestion.type);
				endPoint.processor.UpdateEmittedElement(endPoint.suggestion.type);

				this.pool.RemoveEffectsFrom(endPoint.transform);
				endPoint.pool.RemoveEffectsFrom(this.transform);

				this.AcceptSuggestion();
				endPoint.AcceptSuggestion();
			}
		}

		#endregion
	}
}