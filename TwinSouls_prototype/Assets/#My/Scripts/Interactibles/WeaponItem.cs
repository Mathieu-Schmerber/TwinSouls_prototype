using System.Collections;
using System.Collections.Generic;
using TwinSouls.Data;
using TwinSouls.Player;
using TwinSouls.Tools;
using UnityEngine;

namespace TwinSouls.Interactibles
{
	public class WeaponItem : MonoBehaviour
	{
		[SerializeField] private float _rotateSpeed;
		[SerializeField] private float _floatRange;
		[SerializeField] private WeaponData _data;
		public WeaponData Data { get => _data; private set => _data = value; }

		private Vector3 _initialPos;
		private float _randomFloatingTime;

		private void Start()
		{
			_initialPos = transform.position;
			_randomFloatingTime = Random.Range(0, 10);
			transform.Rotate(Vector3.up * Random.Range(0, 360));
			UpdateAppearance(Data);
		}

		private void Update()
		{
			transform.Rotate(Vector3.up * _rotateSpeed * Time.deltaTime);
			transform.position = new Vector3(_initialPos.x, _initialPos.y + Mathf.Sin(Time.time + _randomFloatingTime) * _floatRange, _initialPos.z);
		}

		private void OnTriggerEnter(Collider other)
		{
			other.GetComponentInChildren<PlayerWeaponHolder>()?.SuggestWeapon(this);
		}

		private void OnTriggerExit(Collider other)
		{
			other.GetComponentInChildren<PlayerWeaponHolder>()?.UnSuggestWeapon(this);
		}

		public static GameObject Spawn(WeaponData data, Vector3 position)
		{
			WeaponItem prefab = DataLoader.Spawnable.Load<WeaponItem>("WeaponItem");
			WeaponItem result = Instantiate(prefab.gameObject, new Vector3(position.x, position.y + prefab.transform.localScale.y + prefab._floatRange, position.z), Quaternion.identity).GetComponent<WeaponItem>();

			result.Data = data;
			result.UpdateAppearance(data);
			return result.gameObject;
		}

		private void UpdateAppearance(WeaponData data)
		{
			Outline component;

			GetComponent<MeshFilter>().mesh = data.mesh;
			if (GetComponent<Outline>() == null)
			{
				component = gameObject.AddComponent<Outline>();
				component.OutlineWidth = 4;
			}
		}
	}
}