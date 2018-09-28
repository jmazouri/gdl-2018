﻿using System.Collections.Generic;
using Enums;
using Generic;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : BaseCharacter
    {
        [SerializeField] private MovementConfig _movementConfig;
        
        [SerializeField] private Rigidbody2D _rigidbody;

        private MovementMode _movementMode = MovementMode.Walk;

        private Vector2 _velocity;

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                Debug.Log($"{gameObject.name}: Changed to sneaking!");
                _movementMode = MovementMode.Sneak;
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                Debug.Log($"{gameObject.name}: Changed to walking!");
                _movementMode = MovementMode.Walk;
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                Debug.Log($"{gameObject.name}: Changed to running!");
                _movementMode = MovementMode.Run;
            }
        }

        void FixedUpdate()
        {
            _velocity.x = VelocityForAxis("Horizontal", _velocity.x);
            _velocity.y = VelocityForAxis("Vertical", _velocity.y);

            _rigidbody.velocity = _velocity;
        }

        private float VelocityForAxis(string axis, float currentVelocity)
        {
            var currentSetting = _movementConfig.GetMovementSetting(_movementMode);

            var movement = Input.GetAxisRaw(axis) * currentSetting.Acceleration;
            
            currentVelocity = Mathf.Clamp(currentVelocity + movement, -currentSetting.MaxSpeed, currentSetting.MaxSpeed);

            if (currentVelocity > 0.0f && currentVelocity > currentSetting.Friction)
            {
                currentVelocity -= currentSetting.Friction;
            }
            else if (currentVelocity < 0.0f && currentVelocity < -currentSetting.Friction)
            {
                currentVelocity += currentSetting.Friction;
            }
            else
            {
                currentVelocity = 0.0f;
            }

            return currentVelocity;
        }
    }
}
