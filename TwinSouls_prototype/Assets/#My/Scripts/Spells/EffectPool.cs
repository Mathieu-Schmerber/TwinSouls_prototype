using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using TwinSouls.Data;
using TwinSouls.Tools;
using System;
using TwinSouls.Entity;

namespace TwinSouls.Spells
{
    /// <summary>
    /// Stores the active & boost effects. <br></br>
    /// Applies the effects to the entity.
    /// </summary>
    public class EffectPool : MonoBehaviour
    {
        #region Types

        /// <summary>
        /// Carries the EffectData payload alongside its representative cooldowns and instance
        /// </summary>
        public class ActiveEffect
		{
            public Transform caster;
            public EffectData data;
            public Cooldown durationCd;
            public Cooldown intervalCd;
            public GameObject effectInstance;
            public Action OnLiftedAction;

            public ActiveEffect(EffectData data, Transform caster, Action liftedAction = null)
            {
                this.data = data;
                this.durationCd = new Cooldown()
                {
                    automaticReset = false,
                    readyOnStart = false,
                    cooldownTime = data.duration
                };
                intervalCd = !data.overTime ? null : new Cooldown()
                {
                    automaticReset = false,
                    readyOnStart = true,
                    cooldownTime = data.interval
                };
                this.caster = caster;
                this.durationCd.Init();
                if (intervalCd != null)
                    this.intervalCd.Init();
                this.OnLiftedAction = liftedAction;
            }
        }

        #endregion

        #region Properties

        private Stats _stats;
        private Damageable _damageable;
        [ReadOnly, ShowInInspector] private List<ActiveEffect> _boostEffects = new List<ActiveEffect>();

        /// <summary>
        /// List of effects enhancing the entity. <br></br>
        /// Boosts are infinite and need to be cancelled
        /// </summary>
		public List<ActiveEffect> BoostEffects
		{
			get { return _boostEffects; }
		}

        /// <summary>
        /// Current active effects
        /// </summary>
        [ReadOnly, ShowInInspector] private List<ActiveEffect> _activeEffects = new List<ActiveEffect>();

        public event Action<EffectData> OnEffectLiftedEvt;
        public event Action<ElementData.ElementType> OnBoostChangedEvt;

        #endregion

        #region Unity builtins

        // Get references
        private void Awake()
        {
            _stats = GetComponent<Stats>();
            _damageable = GetComponent<Damageable>();
        }

		private void Update()
		{
            ManageActives();
            ManageBoosts();
        }

		#endregion

		#region Actives & Boosts loops

        /// <summary>
        /// Manage actives cooldowns & apply the effects to the entity
        /// </summary>
        private void ManageActives()
		{
            if (_activeEffects.Count == 0) return;

            for (int i = 0; i < _activeEffects.Count; i++)
            {
                ActiveEffect effect = _activeEffects[i];

                if (effect.durationCd.IsOver())
                {
                    i--;
                    LiftEffect(effect);
                }
                else if (effect.data.overTime && effect.intervalCd.IsOver())
                {
                    ApplyEffect(effect);
                    effect.intervalCd.Reset();
                }
            }
        }

        /// <summary>
        /// Manage the boost cooldowns
        /// </summary>
        private void ManageBoosts()
		{
            if (_boostEffects.Count == 0) return;

			for (int i = 0; i < _boostEffects.Count; i++)
			{
                ActiveEffect boost = _boostEffects[i];

                if (boost.durationCd.IsOver())
                {
                    OnBoostChangedEvt?.Invoke(ElementData.ElementType.NONE);
                    _boostEffects.Remove(boost);
                    i--;
                }
			}
		}

		#endregion

		#region Calculations

		/// <summary>
		/// Calculates the total additional stats given by the active effects of given type
		/// </summary>
		/// <param name="type">Type of the effects to consider</param>
		/// <param name="valueType">Value type of the effects to consider</param>
		/// <param name="refferedStat">Current stat without effects buffs (from Stats component), useless if ValueType == UNIT</param>
		/// <returns></returns>
		private float CalculateEffectValue(EffectData.EffectType type, bool stackable, float refferedStat)
        {
            IEnumerable<float> unitValues = _activeEffects.Where(e => e.data.type == type && e.data.valueType == EffectData.ValueType.UNIT).Select(e => e.data.value);
            IEnumerable<float> percentageValues = _activeEffects.Where(e => e.data.type == type && e.data.valueType == EffectData.ValueType.PERCENTAGE).Select(e => e.data.value);

            if (!stackable)
                return (!unitValues.Any() ? 0 : unitValues.Max()) + (!percentageValues.Any() ? 0 : (percentageValues.Max() / 100) * refferedStat);
            return (!unitValues.Any() ? 0 : unitValues.Sum()) + (!percentageValues.Any() ? 0 : (percentageValues.Sum() / 100) * refferedStat);
        }

        /// <summary>
        /// Recalculates the entity's Stats based on the _activeEffects list
        /// </summary>
        /// <param name="effect"></param>
        private void ProcessBuffCalulations(ActiveEffect effect)
		{
            float damage;

            switch (effect.data.type)
            {
                case EffectData.EffectType.BURN:
                    damage = CalculateEffectValue(effect.data.type, effect.data.stackable, _stats.MaxHealth.StaticValue);
                    _damageable.ApplyDamage(effect.caster.gameObject, damage, false);
                    break;
                case EffectData.EffectType.SLOW:
                    _stats.Speed.temporaryValue = -CalculateEffectValue(effect.data.type, effect.data.stackable, _stats.Speed.StaticValue);
                    break;
                case EffectData.EffectType.SPEED:
                    _stats.Speed.temporaryValue = CalculateEffectValue(effect.data.type, effect.data.stackable, _stats.Speed.StaticValue);
                    break;
                case EffectData.EffectType.STUN:
                    _stats.IsStunned = _activeEffects.Any(e => e.data.type == EffectData.EffectType.STUN);
                    break;
                case EffectData.EffectType.WEAK:
                    _stats.Power.temporaryValue = -CalculateEffectValue(effect.data.type, effect.data.stackable, _stats.Power.StaticValue);
                    _stats.Defense.temporaryValue = -CalculateEffectValue(effect.data.type, effect.data.stackable, _stats.Defense.StaticValue);
                    break;
                case EffectData.EffectType.EXPLOSION:
                    damage = CalculateEffectValue(effect.data.type, effect.data.stackable, _stats.MaxHealth.StaticValue);
                    _damageable.ApplyDamage(effect.caster.gameObject, damage, false);
                    _stats.IsStunned = _activeEffects.Any(e => e.data.type == EffectData.EffectType.EXPLOSION);
                    break;
                case EffectData.EffectType.FREEZE:
                    _stats.IsStunned = _activeEffects.Any(e => e.data.type == EffectData.EffectType.FREEZE);
                    _stats.Power.temporaryValue = -CalculateEffectValue(effect.data.type, effect.data.stackable, _stats.Power.StaticValue);
                    _stats.Defense.temporaryValue = -CalculateEffectValue(effect.data.type, effect.data.stackable, _stats.Defense.StaticValue);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Removes an effect from the _activeEffects list
        /// </summary>
        /// <param name="effect"></param>
        /// <param name="partialLift">if "effect" is not primary and lifted by a repression, 
        /// "partialLift" represents the effect that was repressed, resulting in the lifting of "effect"</param>
        private void LiftEffect(ActiveEffect effect, EffectData partialLift = null, bool fusionEffect = false)
		{
            // Stops and destroy effect particles
            if (effect.effectInstance != null && _activeEffects.Count(eff => eff.effectInstance == effect.effectInstance) == 1)
                effect.effectInstance.AddComponent<FXTimedDestruction>().SmoothFxKill();
            _activeEffects.Remove(effect);
            ProcessBuffCalulations(effect);

            if (fusionEffect)
                return;
            else if (partialLift == null)
                OnEffectLiftedEvt?.Invoke(effect.data);
            else
                OnEffectLiftedEvt?.Invoke(partialLift);
        }

        /// <summary>
        /// Apply an effect, does not add the effect to the _activeEffects list
        /// </summary>
        /// <param name="effect"></param>
        private void ApplyEffect(ActiveEffect effect) => ProcessBuffCalulations(effect);

        #endregion

        #region Filtering

        /// <summary>
        /// Handles the logic of adding the effect if a complex effect is present. <br></br>
        /// Returns the new effect to apply to this entity. <br></br>
        /// </summary>
        /// <param name="effect"></param>
        /// <param name="element"></param>
        /// <param name="caster"></param>
        /// <returns></returns>
        private ActiveEffect ApplyComplexEffectFilter(EffectData effect, ElementData element, Transform caster)
		{
            EffectData weak = null;
            EffectData notWeak = null;
            ActiveEffect complex = _activeEffects.FirstOrDefault(x => !x.data.isPrimary && !x.data.fusion.Contains(effect));

            if (complex == null)
                return null;
            weak = complex.data.fusion.First(x => element.strongerThan.Contains(DataLoader.GetElementOfType(x.element)));
            notWeak = complex.data.fusion.First(x => x != weak);
            LiftEffect(complex, weak);
            return new ActiveEffect(notWeak, transform);
        }

        /// <summary>
        /// Handles the logic of adding the effect if another primary effect is present. <br></br>
        /// Returns the new effect to apply to this entity. <br></br>
        /// </summary>
        /// <param name="effect"></param>
        /// <param name="element"></param>
        /// <param name="caster"></param>
        /// <returns></returns>
        private ActiveEffect ApplyPrimaryEffectFilter(EffectData effect, ElementData element, Transform caster)
		{
            ActiveEffect newEffect = null;
            ElementData activeElem = null;
            EffectData fusion = null;
            ActiveEffect primary = _activeEffects.FirstOrDefault(x => x.data.isPrimary);

            if (primary == null)
                return null;
            fusion = DataLoader.GetEffectOfFusion(effect, primary.data);
            if (fusion != null)
            {
                newEffect = new ActiveEffect(fusion, caster);
                LiftEffect(primary, fusionEffect: true);
            }
            else
            {
                // If the effect is repressing, cancel the active.
                // If the active is stronger, just don't apply the effect.
                activeElem = DataLoader.GetElementOfType(primary.data.element);
                if (element.strongerThan.Contains(activeElem))
                    LiftEffect(primary);
            }
            return newEffect;
        }

        /// <summary>
        /// Filter effect before adding it as an active effect. <br></br>
        /// Can also transform the effect according to fusion or cancelation.
        /// </summary>
        /// <param name="effect"></param>
        /// <param name="caster"></param>
        private void FilterEffect(EffectData effect, Transform caster)
        {
            ElementData element = DataLoader.GetElementOfType(effect.element);
            ActiveEffect newEffect = null;

            // Fusion and repression with primary effect
            newEffect = ApplyPrimaryEffectFilter(effect, element, caster);

            // Partial repression of not primary effect
            if (newEffect == null)
                newEffect = ApplyComplexEffectFilter(effect, element, caster);

            if (newEffect != null)
            {
                ActiveEffect existant = _activeEffects.FirstOrDefault(x => x.data.type == newEffect.data.type);

                if (existant != null)
                {
                    existant.durationCd.Reset();
                    return;
                }
            }
            else
                newEffect = new ActiveEffect(effect, caster);

            // Else if no other effect where present, just add this one
            newEffect.effectInstance = Instantiate(newEffect.data.prefabFx, transform);
            _activeEffects.Add(newEffect);
            ApplyEffect(newEffect);
        }

		#endregion

		#region Public access

		/// <summary>
		/// Add an effect in the effect pool. <br></br>
		/// This effect may get transformed by others already present in the pool.
		/// </summary>
		/// <param name="effect"></param>
		/// <param name="caster"></param>
		public void AddEffect(EffectData.EffectType effect, Transform caster) => AddEffect(DataLoader.GetEffectOfType(effect), caster);

        public void AddEffect(EffectData effect, Transform caster)
        {
            ActiveEffect active;

            // If stackable, we stack only effect from different casters
            if (effect.stackable)
                active = _activeEffects.FirstOrDefault(x => x.data.type == effect.type && x.caster == caster);
            // If not stackable, we extend the duration anyway
            else
                active = _activeEffects.FirstOrDefault(x => x.data.type == effect.type);

            // Extending cooldown if a same effect is already present
            if (active != null)
                active.durationCd.Reset();
            else
            {
                if (effect.element == ElementData.ElementType.NONE) 
                {
                    ActiveEffect newEffect = new ActiveEffect(effect, caster);

                    newEffect.effectInstance = Instantiate(newEffect.data.prefabFx, transform);
                    _activeEffects.Add(newEffect);
                    return;
                }
                FilterEffect(effect, caster);
            }
        }

        public void AddWithoutFiltering(EffectData.EffectType effect, Transform caster) => AddWithoutFiltering(DataLoader.GetEffectOfType(effect), caster);

        /// <summary>
        /// Apply an effect, without executing any filtering check.
        /// </summary>
        /// <param name="effect"></param>
        /// <param name="caster"></param>
        public void AddWithoutFiltering(EffectData effect, Transform caster)
		{
            ActiveEffect active;

            // If stackable, we stack only effect from different casters
            if (effect.stackable)
                active = _activeEffects.FirstOrDefault(x => x.data == effect && x.caster == caster);
            // If not stackable, we extend the duration anyway
            else
                active = _activeEffects.FirstOrDefault(x => x.data == effect);

            // Extending cooldown if a same effect is already present
            if (active != null)
                active.durationCd.Reset();
            else
            {
                ActiveEffect newEffect = new ActiveEffect(effect, caster);

                newEffect.effectInstance = Instantiate(newEffect.data.prefabFx, transform);
                _activeEffects.Add(newEffect);
                ApplyEffect(newEffect);
            }
        }

        /// <summary>
        /// Remove every effects applied by the specified caster
        /// </summary>
        /// <param name="caster"></param>
        public void RemoveEffectsFrom(Transform caster)
		{
            List<ActiveEffect> removeFilter = _activeEffects.Where(effect => effect.caster == caster).ToList();

            foreach (ActiveEffect item in removeFilter)
                LiftEffect(item);
		}

        /// <summary>
        /// Add an effect to the boosts. <b></b>
        /// If this boost already exists, it will get extended.
        /// </summary>
        /// <param name="effect"></param>
        /// <param name="caster"></param>
        public void AddBoost(EffectData.EffectType effect, Transform caster) => AddBoost(DataLoader.GetEffectOfType(effect), caster);

        public void AddBoost(EffectData effect, Transform caster)
		{
            ActiveEffect boost = _boostEffects.FirstOrDefault(x => x.data.type == effect.type);

            // Extending cooldown if a same boost is already present
            if (boost != null)
                boost.durationCd.Reset();
            else
            {
                _boostEffects.Add(new ActiveEffect(effect, caster));
                OnBoostChangedEvt?.Invoke(effect.element);
            }
        }

        /// <summary>
        /// Forcefully remove every boosts.
        /// </summary>
        public void CancelBoosts()
		{
            _boostEffects.Clear();
            OnBoostChangedEvt?.Invoke(ElementData.ElementType.NONE);
        }

        #endregion
    }
}