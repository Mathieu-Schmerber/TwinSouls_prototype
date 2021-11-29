using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TwinSouls.Interactibles;

namespace TwinSouls.UI
{
    public class WaveCanvas : MonoBehaviour
    {
        private TextMeshProUGUI _text;
		private CanvasGroup _group;

		[Header("Show animation")]
		[SerializeField] private Vector2 _shakeAmount = new Vector2(1, 1);
		[SerializeField] private float _shakeTime = 1f;

		private void Awake()
		{
			_text = GetComponentInChildren<TextMeshProUGUI>();
			_group = GetComponent<CanvasGroup>();
			Hide();
			Spawner.OnSpawnerClearedEvt += OnSpawnerCleared;
			Spawner.OnWaveStartEvt += OnWaveStateChanged;
		}

		private void OnDestroy()
		{
			Spawner.OnSpawnerClearedEvt -= OnSpawnerCleared;
			Spawner.OnWaveStartEvt -= OnWaveStateChanged;
		}

		private void SetString(int currentWave, int totalWaves) => _text.text = $"Waves: {currentWave}/{totalWaves}";

		private void Show(int currentWave, int totalWaves)
		{
			_group.alpha = 1;
			iTween.PunchScale(_text.gameObject, _shakeAmount, _shakeTime);
		}

		private void Hide() => _group.alpha = 0;

		private void OnWaveStateChanged(int currentWave, int totalWaves)
		{
			Show(currentWave, totalWaves);
			SetString(currentWave, totalWaves);
		}

		private void OnSpawnerCleared() => Hide();
	}
}