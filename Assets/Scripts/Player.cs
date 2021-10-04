using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float acceleration;
    public float friction;
    
    public float analogDeadzone = 0.25f;
        
    private Rigidbody2D _rigidBody;
    private Vector2 _velocity;
    private float _horizontalInput;
    private float _verticalInput;
    private float _magnitude;
    
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
    }

    private void FixedUpdate()
    {
    }

}
