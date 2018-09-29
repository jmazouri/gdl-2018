using System.Collections.Generic;
using System.Linq;
using Enums;
using Generic;
using UnityEngine;

namespace AI
{
    public class BaseAIController : BaseCharacter
    {
        [SerializeField] private TileNavController _navController;
        [SerializeField] private Transform[] _patrolRoute;
        [SerializeField] private float _patrolSpeed;
        [SerializeField] private float _investigationSpeed;
        [SerializeField] private float _chaseSpeed;
        private int _index;
        public List<Vector3> Route { get; set; }
        public AIState CurrentState { get; set; }

        void Start()
        {
            if (_patrolRoute.Length > 0)
            {
                AssignDestination(_patrolRoute[_index].position);
            }
        }

        void Update()
        {
            if (Route == null || Route.Count == 0)
            {
                return;
            }

            if (Vector3.Distance(Route.First(), transform.position) <= 0.1f)
            {
                Route.Remove(Route.First());
                if (CurrentState == AIState.Patrolling && (Route == null || Route.Count == 0))
                {
                    _index++;
                    if (_index >= _patrolRoute.Length)
                    {
                        _index = 0;
                    }
                    AssignDestination(_patrolRoute[_index].position);
                    return; //todo: jank
                }

                if (Route == null || Route.Count == 0)
                {
                    return; //todo: jank
                }
            }

            Vector3 diff = Route.First() - transform.position;
            diff.Normalize();

            float rotationValue = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotationValue - 90);
            var finalSpeed = 0f;
            switch (CurrentState)
            {
                    case AIState.Patrolling:
                        finalSpeed = _patrolSpeed;
                        break;
                    case AIState.Investigating:
                        finalSpeed = _investigationSpeed;
                        break;
                    case AIState.Chasing:
                        finalSpeed = _chaseSpeed;
                        break; 
            }
            transform.Translate(Vector3.up * finalSpeed * Time.deltaTime);
        }

        public void AssignDestination(Vector3 destination, AIState newState = AIState.Patrolling)
        {
            Debug.Log(newState);
            CurrentState = newState;

            Route = null;
            _navController.ResolvePath(transform.position, destination, this);
        }
    }
}