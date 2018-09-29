using UnityEngine;

namespace Generic
{
    public class ArrowBehavior : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private Rigidbody2D _rigidbody;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            var velocity = transform.right * _speed;
            _rigidbody.velocity = velocity;
        }
    }
}
