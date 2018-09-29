using UnityEngine;

namespace AI
{
    public interface IAIInteractor
    {
        void ReceiveAudio(float strength, Vector3 sourcePosition);
	
    }
}