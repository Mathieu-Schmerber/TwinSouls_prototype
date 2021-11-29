using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;


namespace TwinSouls.Player 
{
    public class MultipleTargetCamera : MonoBehaviour
    {
        #region Types

        #endregion

        #region Properties

        [SerializeField, TabGroup("Zoom")] private float _minZoom = 7f;
        [SerializeField, TabGroup("Zoom")] private float _maxZoom = 14f;
        [SerializeField, TabGroup("Zoom")] private float _zoomSmoothTime = .2f;

        [SerializeField, TabGroup("Centered")] private Vector3 _offset;
        [SerializeField, TabGroup("Centered")] private float _smoothTime = .5f;

        private Vector3 _velocity;
        private Camera _camera;
        public List<Transform> targets;

		#endregion

		#region Unity builtins

		private void Awake()
		{
            _camera = GetComponent<Camera>();
            targets = new List<Transform>();
			StageManager.OnPlayerSpawnedEvt += StageManager_OnPlayerSpawnedEvt;
        }

		private void StageManager_OnPlayerSpawnedEvt(GameObject player)
		{
            targets.Add(player.transform);
        }

        private void LateUpdate()
        {
            if (targets.Count == 0) return;

            Bounds cameraBounds = ProcessBounds();

            Move(cameraBounds);
            Zoom(cameraBounds);
        }

        #endregion

        /// <summary>
		/// Camera position offsetting
		/// </summary>
		private void Move(Bounds cameraBounds)
        {
            Vector3 center = cameraBounds.center;
            Vector3 offCenter = center + _offset;

            transform.position = Vector3.SmoothDamp(transform.position, offCenter, ref _velocity, _smoothTime);
        }

        /// <summary>
		/// Camera FOV offsetting
		/// </summary>
		private void Zoom(Bounds cameraBounds)
        {
            Vector3 center = cameraBounds.center;
            float distanceToCenter = Vector3.Distance(center, targets[0].position);

            if (distanceToCenter > _maxZoom)
                distanceToCenter = _maxZoom;
            else if (distanceToCenter < _minZoom)
                distanceToCenter = _minZoom;
            _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, distanceToCenter, _zoomSmoothTime);
        }

        /// <summary>
        /// Calculates boundaries including the target array
        /// </summary>
        /// <returns></returns>
        private Bounds ProcessBounds()
        {
            Bounds bounds = new Bounds(targets[0].position, Vector3.zero);

            targets.ForEach(target => bounds.Encapsulate(target.position));
            return bounds;
        }
    }
}