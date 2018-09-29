using System;
using UnityEngine;

namespace AI
{
    public class SightBehaviour : MonoBehaviour
    {
        [SerializeField] private GameObject _player;
        [SerializeField] private float _fovAngle = 110;
        [SerializeField] private bool _playerInSight;
        public Vector3 LastSeen { get; set; }
        public Vector3 AbsolutePlayerPosition => _player.transform.position;

        public event EventHandler<PlayerSeenEventArgs> PlayerSeen;

        // Update is called once per frame
        void Update()
        {
            var vectorDirectionAndDistance = _player.transform.position - transform.position;
            var directionVector = vectorDirectionAndDistance.normalized;
            var angleResult = Vector3.Angle(transform.up, directionVector);

            if (!IsInFOV(angleResult)) return;

            var result = Physics2D.Raycast(transform.position, directionVector);
            Debug.DrawRay(transform.position, directionVector);
            if (result.collider != null && result.collider.gameObject == _player)
            {
                LastSeen = _player.transform.position;
                PlayerSeen?.Invoke(this, new PlayerSeenEventArgs(_player.transform.position));
            }
        }

        private bool IsInFOV(float angleResult)
        {
            //Debug.Log("input: " + angleResult + " result: " + (angleResult < _fovAngle / 2));
            return angleResult < _fovAngle / 2;
        }
    }
}