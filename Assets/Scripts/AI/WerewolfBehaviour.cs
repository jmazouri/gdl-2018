using System;
using System.Collections;
using System.Linq;
using Enums;
using UnityEngine;

namespace AI
{
	[RequireComponent(typeof(BaseAIController), typeof(SightBehaviour))]
	public class WerewolfBehaviour : MonoBehaviour, IAIInteractor
	{
		[SerializeField] private float _detectionStrength = 4;
		[SerializeField] private float _investigationTileTime = 5;
		private BaseAIController _aiController;
		private SightBehaviour _sight;
		private Coroutine _investigateRoutine;

		void Start()
		{
			_aiController = GetComponent<BaseAIController>();
			_sight = GetComponent<SightBehaviour>();
			_sight.PlayerSeen += OnPlayerSeen;
			_aiController.IsDoneChasing += OnDoneChasing;
		}

		private void OnDoneChasing(object sender, EventArgs e)
		{
			_aiController.CurrentState = AIState.Investigating;
			_investigateRoutine = StartCoroutine(CycleLocalArea(transform.position, _investigationTileTime));
		}

		private IEnumerator CycleLocalArea(Vector3 location, float investigationTileTime)
		{
			var adjacentNodes = _aiController.GetNodePerimeter(location);
			var pseudoPos = _aiController.GetPseudoPosition(location);
			adjacentNodes = adjacentNodes.OrderBy(x => Vector2Int.Distance(pseudoPos, x)).ToList();
			foreach (var node in adjacentNodes)
			{
				_aiController.AssignDestination(_aiController.GetRealWorldPosition(node), AIState.Investigating);
				yield return new WaitUntil(() => _aiController.CurrentState == AIState.Idle);
				yield return new WaitForSeconds(_investigationTileTime);
			}

			_aiController.StartPatrol();
		}

		private void OnPlayerSeen(object sender, PlayerSeenEventArgs e)
		{
			if (_investigateRoutine != null)
			{
				StopCoroutine(_investigateRoutine);
			}
			_aiController.AssignDestination(e.Position, AIState.Chasing);
		}

		public void ReceiveAudio(float strength, Vector3 sourcePosition)
		{
			if (strength < _detectionStrength) return;
			_sight.LastSeen = sourcePosition;
			if (_investigateRoutine != null)
			{
				StopCoroutine(_investigateRoutine);
			}
			_investigateRoutine = StartCoroutine(CycleLocalArea(sourcePosition, _investigationTileTime));
			_aiController.AssignDestination(sourcePosition, AIState.Investigating);
		}
	}
}

