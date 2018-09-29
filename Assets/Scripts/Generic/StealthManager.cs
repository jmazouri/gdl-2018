using System.Runtime.InteropServices;
using UnityEngine;

namespace Generic
{
    public class StealthManager : MonoBehaviour
    {
        [SerializeField] private GameObject _player;

        private float _currentSuspicion;

        // Use this for initialization
        void Start()
        {
            if (_player == null) Debug.LogError("Player not assigned in StealthManager");
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
