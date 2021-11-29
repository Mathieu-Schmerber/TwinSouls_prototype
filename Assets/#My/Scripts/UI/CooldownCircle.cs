using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TwinSouls.UI
{
	public class CooldownCircle : MonoBehaviour
	{
		private float _time;
		private float _speed;
		private Image _circle;

		private void Awake()
		{
			_circle = GetComponent<Image>();
		}

		private void Update()
		{
			if (_circle.fillAmount > 0)
				_circle.fillAmount -= _speed * Time.deltaTime;
		}

		public void StartCooldown(float time)
		{
			_circle.fillAmount = 1;
			_time = time;
			_speed = _circle.fillAmount / time;
		}
	}
}