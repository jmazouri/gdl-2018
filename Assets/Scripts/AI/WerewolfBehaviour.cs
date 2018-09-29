using System;
using UnityEngine;

namespace AI
{
	[RequireComponent(typeof(BaseAIController))]
	public class WerewolfBehaviour : MonoBehaviour, IAIInteractor
	{
		[SerializeField] private float _detectionStrength = 4;
		private BaseAIController _aiControllerController;

		void Start()
		{
			_aiControllerController = GetComponent<BaseAIController>();
		}
		
		
		
		
		public void ReceiveAudio(float strength)
		{
			if (strength < _detectionStrength) return;
			
			throw new NotImplementedException();
		}
	}
}

