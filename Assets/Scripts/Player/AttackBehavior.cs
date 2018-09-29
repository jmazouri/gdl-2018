using UnityEngine;

namespace Player
{
    public class AttackBehavior : MonoBehaviour
    {
        [SerializeField] private GameObject _arrowPrefab;
        [SerializeField] private int _ammo = 10;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) DoAttack();
        }

        private void DoAttack()
        {
            if (_ammo > 0)
            {
                Debug.DrawRay(transform.position, transform.right, Color.green, 1);
                Debug.Log("I shot");
                _arrowPrefab.transform.rotation = transform.rotation;
                _arrowPrefab.transform.position = transform.position;
                Instantiate(_arrowPrefab);

                _ammo--;
            }
        }
    }
}
