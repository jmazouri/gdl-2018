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

        [SerializeField] private Animator _animator;

        private MovementMode _movementMode = MovementMode.Walk;

        private Vector2 _velocity;
        private Vector2 _axisInput;

        public MovementMode MovementMode
        {
            get
            {
                return _movementMode;
            }

            private set
            {
                _movementMode = value;
                MovementModeChanged?.Invoke(_movementMode);
                Debug.Log($"{gameObject.name}: Movement mode set to <{_movementMode}>!");
            }
        }

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
                MovementMode = MovementMode.Sneak;
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                MovementMode = MovementMode.Walk;
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                MovementMode = MovementMode.Run;
            }

            _axisInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }

        void FixedUpdate()
        {
            // _velocity.x = VelocityForAxis(_axisInput.x, _velocity.x);
            // _velocity.y = VelocityForAxis(_axisInput.y, _velocity.y);
            _velocity = GetNextVelocity(_axisInput, _velocity);

            _rigidbody.velocity = _velocity;
            _animator.SetFloat("Velocity", _velocity.magnitude);

            if (_velocity != Vector2.zero)
            {
                var angle = Mathf.Atan2(_velocity.normalized.y, _velocity.normalized.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
            Debug.DrawRay(transform.position, _velocity.normalized, Color.blue, 1);

            MakeNoise();
        }

        private Vector2 GetNextVelocity(Vector2 input, Vector2 current)
        {
            var setting = _movementConfig.GetMovementSetting(_movementMode);
            var newSpeed = input.normalized * setting.Acceleration + current;
            return Vector2.ClampMagnitude(newSpeed.magnitude > setting.Friction ? newSpeed - newSpeed.normalized * setting.Friction : Vector2.zero, setting.MaxSpeed);
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
