using UnityEngine;

namespace Assets.Scripts
{
    public class BaseCharacter : MonoBehaviour
    {
        [SerializeField] private float _hitPoints = 10;
        /// <summary>
        /// The hitpoints of the character.
        /// Readonly, if you want to adjust the HP use TakeDamage(float) or Heal(float)
        /// </summary>
        public float HitPoints => _hitPoints;

        // Use this for initialization
        void Start () {
		
        }
	
        // Update is called once per frame
        void Update () {
            if (Input.GetKeyDown(KeyCode.UpArrow)) Heal(1f);
            if (Input.GetKeyDown(KeyCode.DownArrow)) TakeDamage(1f);
        }

        /// <summary>
        /// Lets the character take damage and decide if it needs to die
        /// </summary>
        /// <param name="damage">the amount of damage taken</param>
        public void TakeDamage(float damage)
        {
            AdjustHitpoints(Mathf.Abs(damage) * -1);
        }

        /// <summary>
        /// Lets the character heal itself
        /// </summary>
        /// <param name="health">the amount of health healed</param>
        public void Heal(float health)
        {
            AdjustHitpoints(Mathf.Abs(health));
        }
    
        private void AdjustHitpoints(float damage)
        {
            Debug.Log($"Adjusting hitpoints by {damage} points");
            _hitPoints += damage;
            Debug.Log($"Hitpoints {_hitPoints} left");
            if (_hitPoints <= 0.0f) Die();
        }

        /// <summary>
        /// Destroy the object and start the dying behaviour
        /// </summary>
        public virtual void Die()
        {
            Debug.Log("Dying!");
            Destroy(this);
        }
    }
}
