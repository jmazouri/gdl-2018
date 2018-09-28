using System;
using System.ComponentModel;
using Enums;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "MovementConfig", menuName = "Scriptable/Movement", order = 1)]
    public class MovementConfig : ScriptableObject
    {
        [SerializeField] private MovementSetting _sneakSetting;

        [SerializeField] private MovementSetting _walkSetting;

        [SerializeField] private MovementSetting _runsetting;

        public MovementSetting GetMovementSetting(MovementMode movementMode)
        {
            switch (movementMode)
            {
                case MovementMode.Sneak:
                    return _sneakSetting;
                case MovementMode.Run:
                    return _runsetting;
                case MovementMode.Walk:
                    return _walkSetting;
                default:
                    throw new InvalidEnumArgumentException("FUCK YOU VISUAL STUDIO");
            }
        }
    }
}
