using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TwinSouls.Player;
using TwinSouls.Spells;
using TwinSouls.Player.Kits;
using TwinSouls.Tools;
using Sirenix.OdinInspector;
using TwinSouls.Data;

namespace TwinSouls.UI
{
	public class InputsCanvas : MonoBehaviour
	{
		[Serializable]
		public class InputWheel
		{
			public GameObject groupGO;
			[TabGroup("Input icons")] public Image fire;
			[TabGroup("Input icons")] public Image lightining;
			[TabGroup("Input icons")] public Image ice;
			[TabGroup("Input icons")] public Image water;
			[TabGroup("Input icons")] public Image moving;
			[TabGroup("Input icons")] public Image attack;
			[TabGroup("Cooldown UI")] public CooldownCircle movingLoadCirlce;
			[TabGroup("Cooldown UI")] public Image movingBackgroundCirlce;
			[TabGroup("Cooldown UI")] public Image movingIcon;
			[TabGroup("Cooldown UI")] public CooldownCircle attackLoadCirlce;
			[TabGroup("Cooldown UI")] public Image attackBackgroundCirlce;
			[TabGroup("Cooldown UI")] public Image attackElementIcon;

			[HideInInspector] public AElementProcessor processor;
		}

		[SerializeField] private InputWheel[] _inputWheels = new InputWheel[2];

		private void Awake()
		{
			foreach (InputWheel wheel in _inputWheels)
				wheel.groupGO.SetActive(false);
			InputHandler.OnPlayerInputReadyEvt += StageManager_OnPlayerSpawnedEvt;
			AMobilityKit.OnMovingAbilityStartEvt += AElementalKit_OnMovingAbilityStartEvt;
			AMobilityKit.OnAttackAbilityStartEvt += AElementalKit_OnAttackAbilityStartEvt;
		}

		private void AElementalKit_OnAttackAbilityStartEvt(GameObject arg1, float arg2)
		{
			InputWheel wheel = _inputWheels.FirstOrDefault(x => x.processor.gameObject == arg1);

			if (wheel != null)
				wheel.attackLoadCirlce.StartCooldown(arg2);
		}

		private void AElementalKit_OnMovingAbilityStartEvt(GameObject arg1, float arg2)
		{
			InputWheel wheel = _inputWheels.FirstOrDefault(x => x.processor.gameObject == arg1);

			if (wheel != null)
				wheel.movingLoadCirlce.StartCooldown(arg2);
		}

		private void OnDestroy()
		{
			InputHandler.OnPlayerInputReadyEvt -= StageManager_OnPlayerSpawnedEvt;
			AMobilityKit.OnMovingAbilityStartEvt -= AElementalKit_OnMovingAbilityStartEvt;
			AMobilityKit.OnAttackAbilityStartEvt -= AElementalKit_OnAttackAbilityStartEvt;
		}

		private void StageManager_OnPlayerSpawnedEvt(InputHandler handler)
		{
			int index = _inputWheels.Count(x => x.groupGO.activeSelf);
			InputWheel wheel = _inputWheels[index];

			wheel.processor = handler.GetComponent<AElementProcessor>();
			UpdateWheelElement(wheel, wheel.processor.EmittedElement);
			wheel.processor.OnEmittedElementChangedEvt += (element) => UpdateWheelElement(wheel, element);

			wheel.groupGO.SetActive(true);
			wheel.fire.sprite = handler.GetInputSprite(handler.Input().FireVote);
			wheel.lightining.sprite = handler.GetInputSprite(handler.Input().LightingVote);
			wheel.ice.sprite = handler.GetInputSprite(handler.Input().IceVote);
			wheel.water.sprite = handler.GetInputSprite(handler.Input().WaterVote);
			wheel.moving.sprite = handler.GetInputSprite(handler.Input().MovingAbility);
			wheel.attack.sprite = handler.GetInputSprite(handler.Input().Attack);
		}

		private void UpdateWheelElement(InputWheel wheel, ElementData.ElementType data)
		{
			ElementData element = DataLoader.GetElementOfType(data);

			wheel.movingBackgroundCirlce.color = element?.color ?? DataLoader.Instance.Constants.noneColor;
			wheel.attackBackgroundCirlce.color = element?.color ?? DataLoader.Instance.Constants.noneColor;
			if (element != null)
			{
				wheel.attackElementIcon.sprite = element.icon;
				wheel.attackElementIcon.color = Color.white;
				wheel.movingIcon.color = Color.white;
			}
			else
			{
				wheel.attackElementIcon.color = Color.black;
				wheel.movingIcon.color = Color.black;
			}
		}
	}
}