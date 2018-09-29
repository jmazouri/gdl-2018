using AI;
using UnityEngine;

namespace Generic
{
    public class ArrowBehavior : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private BoxCollider2D _collider;

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

        void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.IsTouchingLayers(LayerMask.GetMask("Obstacle", "Enemy"))) return;

            var wolf = other.GetComponent<BaseAIController>();
            if (wolf != null)
            {
                wolf.TakeDamage(1);
            }

            Destroy(gameObject);
        }
    }
}
