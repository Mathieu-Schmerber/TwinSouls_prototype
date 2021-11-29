using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using TwinSouls.Data;
using TwinSouls.Tools;
using TwinSouls.Spells;

namespace TwinSouls.Player.Kits
{
    public class LightningKit : AMobilityKit
    {
        #region Types



        #endregion

        #region Properties

        [BoxGroup("Moving Ability/Flash")]
        [SerializeField] private float _flashDistance;

        [BoxGroup("Moving Ability/Flash")]
        [SerializeField] private GameObject _areaSpellPrefab;

        [BoxGroup("Moving Ability/Flash")]
        [SerializeField] private LayerMask _untrespassableLayer;

        #endregion

        #region Unity builtins

        // Get references
        protected override void Awake()
        {
            base.Awake();
            type = ElementData.ElementType.LIGHTING;
        }

        #endregion

        protected override void OnMoveAbility() => Flash();

        private void CastAreaSpell()
        {
            AreaSpell[] areas = new AreaSpell[2] {
                Instantiate(_areaSpellPrefab, transform).GetComponent<AreaSpell>(),
                Instantiate(_areaSpellPrefab, transform.position, Quaternion.identity).GetComponent<AreaSpell>()
            };

            foreach (AreaSpell spell in areas)
                spell.CastArea(_stats);
        }

        private Vector3 GetUncollidingDestinationFromWall(float distance, Vector3 normal, CapsuleCollider capsule)
		{
            RaycastHit hit;
            float distanceToObstacle = _flashDistance;
            Vector3 p1 = transform.position + capsule.center + Vector3.up * -capsule.height * 0.5f;
            Vector3 p2 = p1 + Vector3.up * capsule.height;

            // If an obstacle is detected at the expected destination
            // We are overlapping a sphere above the ground but with the correct collider radius
            if (Physics.OverlapSphere(transform.position + Vector3.up + (normal * distance), capsule.radius).Length > 0)
            {
                // Calculating the nearest position to obstacle where the capsule collider will not collide
                // Source documentation: https://docs.unity3d.com/ScriptReference/Physics.CapsuleCast.html
                if (Physics.CapsuleCast(p1, p2, capsule.radius, normal, out hit, _flashDistance))
                    distanceToObstacle = hit.distance;
            }
            return transform.position + normal * distanceToObstacle;
        }

        private void Flash()
        {
            RaycastHit hit;
            Vector3 normal = _controller.GetMovementNormal() == Vector3.zero ? _controller.GetAimNormal() : _controller.GetMovementNormal();
            CapsuleCollider capsule = GetComponent<CapsuleCollider>();
            Vector3 destination = GetUncollidingDestinationFromWall(_flashDistance, normal, capsule);

            // Blocks if there is an untrespassable obstacle
            if (Physics.Linecast(transform.position, destination, out hit, _untrespassableLayer))
                destination = GetUncollidingDestinationFromWall(hit.distance, normal, capsule);

            if (_areaSpellPrefab)
                CastAreaSpell();
            transform.position = destination;
        }

    }
}