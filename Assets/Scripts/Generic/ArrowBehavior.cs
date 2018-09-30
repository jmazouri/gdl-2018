using AI;
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

        void OnTriggerEnter2D(Collider2D collision)
        {
            // Magic numbers are bad but can't figure out how to solve this otherwise.
            if (collision.gameObject.layer != 9 && collision.gameObject.layer != 10) return;

            var wolf = collision.gameObject.GetComponent<BaseAIController>();
            if (wolf != null)
            {
                wolf.TakeDamage(1);
            }

            Destroy(gameObject);
        }
    }
}
