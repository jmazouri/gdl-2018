using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Generic;
using Player;
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
        [SerializeField] private float _damage;
        private int _index;
        public List<Vector3> Route { get; set; }
        public AIState CurrentState { get; set; }

        public event EventHandler IsDoneChasing;

        void Start()
        {
            StartPatrol();
        }

        void Update()
        {
            if (CurrentState == AIState.Dead || CurrentState == AIState.Attacking) return;

            if (Route == null || Route.Count == 0)
            {
                return;
            }

            if (Vector3.Distance(Route.First(), transform.position) <= 0.1f)
            {
                Route.Remove(Route.First());
                if (CurrentState == AIState.Chasing && Route.Count == 0)
                {
                    CurrentState = AIState.Idle;
                    IsDoneChasing?.Invoke(this, EventArgs.Empty);
                    return;
                }

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
                    CurrentState = AIState.Idle;
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

        public List<Vector2Int> GetNodePerimeter(Vector3 location)
        {
            var adjacentPoints = _navController.GetAdjacentNodePoints(_navController.GetPseudoPosition(location).Value)
                .Where(x => _navController.PseudoGrid[x.x, x.y].HasValue).ToList();
            var pointsSet = adjacentPoints.SelectMany(x => _navController.GetAdjacentNodePoints(x))
                .Where(x => _navController.PseudoGrid[x.x, x.y].HasValue).ToList();
            pointsSet = pointsSet.Except(adjacentPoints).Distinct().ToList();
            return pointsSet;
        }

        public Vector2Int GetPseudoPosition(Vector3 location)
        {
            return _navController.GetPseudoPosition(location).Value;
        }

        public Vector3 GetRealWorldPosition(Vector2Int pseudoLocation)
        {
            return _navController.PseudoGrid[pseudoLocation.x, pseudoLocation.y].Value;
        }

        public void StartPatrol()
        {
            if (_patrolRoute.Length > 0)
            {
                AssignDestination(_patrolRoute[_index].position);
                CurrentState = AIState.Patrolling;
            }
            else
            {
                CurrentState = AIState.Idle;
            }
        }

        public override void Die()
        {
            base.Die();
            CurrentState = AIState.Dead;
        }

        public void Attack(Vector3 sightLastSeen)
        {
            CurrentState = AIState.Attacking;
            var player = Physics2D.OverlapCircleAll(transform.position, 0.5f)
                .FirstOrDefault(x => x.gameObject.GetComponent<PlayerController>() != null);
            
            if (player == null) return;
            
            player.GetComponent<PlayerController>().TakeDamage(_damage);
        }
    }
}