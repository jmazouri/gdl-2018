using TMPro;
using UnityEngine;

namespace Level
{
    public class InteractableHay : InteractionTarget
    {
        private bool _containsPlayer;
        private GameObject _player;

        public override void Interact(Interactor interactor)
        {
            _player = interactor.gameObject;

            _player.SetActive(false);

            _containsPlayer = true;
        }

        public override void InternalStart()
        {
            
        }

        public override void InternalUpdate()
        {
            if (!_containsPlayer) return;

            if (Input.GetKeyDown(KeyCode.E))
            {
                _player.SetActive(true);
            }
        }
    }
}
