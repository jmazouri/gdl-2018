using System;
using UnityEngine;

namespace Generic
{
    public class BaseCharacter : MonoBehaviour
    {
        [SerializeField] private float _maxHitPoints = 10;
        [SerializeField] private float _hitPoints = 10;

        public delegate void HealthChangeHandler(float currentHealth, float change);

        public event HealthChangeHandler HealthChanged;
        public event Action Deadened;

        /// <summary>
        /// Maximum amount of hit points for the character
        /// </summary>
        public float MaxHitPoints => _maxHitPoints;

        /// <summary>
        /// The hitpoints of the character.
        /// Readonly, if you want to adjust the HP use TakeDamage(float) or Heal(float)
        /// </summary>
        public float HitPoints => _hitPoints;

        /// <summary>
        /// Lets the character take damage and decide if it needs to die
        /// </summary>
        /// <param name="damage">the amount of damage taken</param>
        public void TakeDamage(float damage)
        {
            Debug.Log($"{gameObject.name}: Taking {damage} damage");
            AdjustHitpoints(Mathf.Abs(damage) * -1);
        }

        /// <summary>
        /// Lets the character heal itself
        /// </summary>
        /// <param name="health">the amount of health healed</param>
        public void Heal(float health)
        {
            Debug.Log($"{gameObject.name}: Healing for {health} hitpoints");
            AdjustHitpoints(Mathf.Abs(health));
        }
    
        private void AdjustHitpoints(float damage)
        {
            _hitPoints += damage;

            if (_hitPoints > _maxHitPoints) _hitPoints = _maxHitPoints;
            if (_hitPoints <= 0.0f) Die();
            else Debug.Log($"{gameObject.name}: Hitpoints {_hitPoints} left");
            HealthChanged?.Invoke(_hitPoints, damage);
        }

        /// <summary>
        /// Destroy the object and start the dying behaviour
        /// </summary>
        public virtual void Die()
        {
            Debug.Log($"{gameObject.name}: Dying!");
            Deadened?.Invoke();
            Destroy(gameObject);
        }
    }
}