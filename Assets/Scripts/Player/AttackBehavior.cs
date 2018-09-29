using System;
using UnityEngine;

namespace Player
{
    public class AttackBehavior : MonoBehaviour
    {
        [SerializeField] private GameObject _arrowPrefab;
        [SerializeField] private int _ammo = 10;
        [SerializeField] private bool _isUsingCrossbow;

        public int Ammo => _ammo;

        public bool IsUsingCrossbow => _isUsingCrossbow;

        public delegate void AmmoChangeHandler(int count);
        public delegate void WeaponSwapHandler(bool isCrossbow);

        public event AmmoChangeHandler AmmoChanged;
        public event WeaponSwapHandler WeaponSwapped;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) DoAttack();
            if (Input.GetKeyDown(KeyCode.Q)) SwapWeapon();
        }

        private void DoAttack()
        {
            if (_isUsingCrossbow && _ammo > 0)
            {
                Debug.DrawRay(transform.position, transform.right, Color.green, 1);
                Debug.Log("I shot");
                _arrowPrefab.transform.rotation = transform.rotation;
                _arrowPrefab.transform.position = transform.position;
                Instantiate(_arrowPrefab);

                SpendAmmo(1);
            }
        }

        public void SpendAmmo(int count)
        {
            AdjustAmmo(-Math.Abs(count));
        }

        public void GainAmmo(int count)
        {
            AdjustAmmo(Math.Abs(count));
        }

        private void AdjustAmmo(int count)
        {
            _ammo += count;
            AmmoChanged?.Invoke(_ammo);
        }

        private void SwapWeapon()
        {
            _isUsingCrossbow = !IsUsingCrossbow;
            WeaponSwapped?.Invoke(_isUsingCrossbow);
        }
    }
}
