using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _maxWalkingSpeed = 2f;
        [SerializeField] private float _maxRunningSpeed = 5f;
        [SerializeField] private float _maxSneakingSpeed = 1f;

        [SerializeField] private float _acceleration = 0.3f;
        [SerializeField] private float _friction = 0.2f;

        [SerializeField] private Rigidbody2D _rigidbody;

        private MovementMode _movementMode = MovementMode.Walk;

        private Vector2 _velocity;

        private Dictionary<MovementMode, int> _maxWalkingSpeeds;

        // Use this for initialization
        void Start()
        {
            _maxWalkingSpeed += _friction;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                Debug.Log("Changed to sneaking!");
                _movementMode = MovementMode.Sneak;
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                Debug.Log("Changed to walking!");
                _movementMode = MovementMode.Walk;
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                Debug.Log("Changed to running!");
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
            var movement = Input.GetAxisRaw(axis) * _acceleration;

            // TODO: this is a fuckfest but i didn't wanna use a switch for this bullshit. Cleanup when we have time
            var currentMaxSpeed = (_movementMode == MovementMode.Sneak ? _maxSneakingSpeed : _movementMode == MovementMode.Run ? _maxRunningSpeed : _maxWalkingSpeed);
            currentVelocity = Mathf.Clamp(currentVelocity + movement, -currentMaxSpeed, currentMaxSpeed);

            if (currentVelocity > 0.0f)
            {
                currentVelocity -= _friction;
            }
            else if (currentVelocity < 0.0f)
            {
                currentVelocity += _friction;
            }

            return currentVelocity;
        }
    }
}
