using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using TwinSouls.Data;
using TwinSouls.Spells;
using TwinSouls.Tools;

namespace TwinSouls.Player.Kits 
{
    public class IceKit : AMobilityKit
    {
        #region Types



        #endregion

        #region Properties

        [BoxGroup("Moving Ability/Slide")]
        [SerializeField] protected float _slideAreaLifetime;
        [BoxGroup("Moving Ability/Slide")]
        [SerializeField] protected GameObject _slideAreaFxPrefab;

        #endregion

        #region Unity builtins

        // Get references
        protected override void Awake()
        {
            base.Awake();
            type = ElementData.ElementType.ICE;
        }

        #endregion

        protected override void OnMoveAbility() => CastSlideArea();

        private void CastSlideArea()
        {
            Vector3 normal = _controller.GetMovementNormal() == Vector3.zero ? _controller.GetAimNormal() : _controller.GetMovementNormal();
            AreaSpell[] spells = Instantiate(_slideAreaFxPrefab, transform.position + normal * 5, Quaternion.identity).GetComponents<AreaSpell>();

			foreach (AreaSpell spell in spells)
			{
                spell.transform.forward = normal;
                spell.CastArea(_stats, _slideAreaLifetime);
            }
        }
    }
}