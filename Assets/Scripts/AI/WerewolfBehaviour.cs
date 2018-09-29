using Enums;
using UnityEngine;

namespace AI
{
	[RequireComponent(typeof(BaseAIController), typeof(SightBehaviour))]
	public class WerewolfBehaviour : MonoBehaviour, IAIInteractor
	{
		[SerializeField] private float _detectionStrength = 4;
		private BaseAIController _aiControllerController;
		private SightBehaviour _sight;

		void Start()
		{
			_aiControllerController = GetComponent<BaseAIController>();
			_sight = GetComponent<SightBehaviour>();
			_sight.PlayerSeen += OnPlayerSeen;
		}

		private void OnPlayerSeen(object sender, PlayerSeenEventArgs e)
		{
			_aiControllerController.AssignDestination(e.Position, AIState.Chasing);
		}

		public void ReceiveAudio(float strength, Vector3 sourcePosition)
		{
			Debug.Break();
			if (strength < _detectionStrength) return;
			_sight.LastSeen = sourcePosition;
			_aiControllerController.AssignDestination(sourcePosition, AIState.Investigating);
		}
	}
}

