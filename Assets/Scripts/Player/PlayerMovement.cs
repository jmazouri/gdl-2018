using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _maxSpeed = 2f;
    [SerializeField] private float _acceleration = 0.3f;
    [SerializeField] private float _friction = 0.2f;
    [SerializeField] private Rigidbody2D _rigidbody;

    private Vector2 _velocity;

    // Use this for initialization
    void Start()
    {
        _maxSpeed += _friction;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        _velocity.x = VelocityForAxis("Horizontal", _velocity.x);
        _velocity.y = VelocityForAxis("Vertical", _velocity.y);

        _rigidbody.velocity = _velocity;
        Debug.Log($"Current _velocity: {_rigidbody.velocity}");
    }

    private float VelocityForAxis(string axis, float currentVelocity)
    {
        var movement = Input.GetAxisRaw(axis) * _acceleration;

        currentVelocity = Mathf.Clamp(currentVelocity + movement, -_maxSpeed, _maxSpeed);

        if (currentVelocity > 0.0f)
        {
            currentVelocity -= _friction;
        }
        else if (currentVelocity < 0.0f)
        {
            currentVelocity += _friction;
        }

        return currentVelocity;
    }
}
