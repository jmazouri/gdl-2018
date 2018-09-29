using AI;
using Enums;
using Generic;
using UnityEngine;

namespace Player
{
    public class PlayerController : BaseCharacter
    {
        [SerializeField] private MovementConfig _movementConfig;

        [SerializeField] private Rigidbody2D _rigidbody;

        private MovementMode _movementMode = MovementMode.Walk;

        private Vector2 _velocity;
        private Vector2 _axisInput;

        public delegate void MovementChanged(MovementMode newMovementMode);

        public event MovementChanged MovementModeChanged;

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
                MovementModeChanged?.Invoke(_movementMode);
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                Debug.Log($"{gameObject.name}: Changed to walking!");
                _movementMode = MovementMode.Walk;
                MovementModeChanged?.Invoke(_movementMode);
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                Debug.Log($"{gameObject.name}: Changed to running!");
                _movementMode = MovementMode.Run;
                MovementModeChanged?.Invoke(_movementMode);
            }

            _axisInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }

        void FixedUpdate()
        {
            _velocity.x = VelocityForAxis(_axisInput.x, _velocity.x);
            _velocity.y = VelocityForAxis(_axisInput.y, _velocity.y);

            _rigidbody.velocity = _velocity;

            MakeNoise();
        }

        private float VelocityForAxis(float direction, float currentVelocity)
        {
            var currentSetting = _movementConfig.GetMovementSetting(_movementMode);

            var movement = direction * currentSetting.Acceleration;

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

        private void MakeNoise()
        {
            var currentSetting = _movementConfig.GetMovementSetting(_movementMode);

            var hits = Physics2D.CircleCastAll(transform.position, currentSetting.NoiseLevel, Vector2.up);

            foreach (var hit in hits)
            {
                var enemy = hit.collider.gameObject.GetComponent<IAIInteractor>();

                enemy?.ReceiveAudio(hit.distance / currentSetting.NoiseLevel, transform.position);
            }
        }
    }
}
