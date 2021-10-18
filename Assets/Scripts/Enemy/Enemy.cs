
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject target;
    public float stopDistance;
    public float speed;
    public float acceleration;
    public float friction;
    public float turnSpeed;

    // private Rigidbody2D _rigidBody;
    private float _speed;
    private float _velocity;
    private float _acceleration;
    private float _friction;
    
    void Start()
    {
        // _rigidBody = GetComponent<Rigidbody2D>();
        InitializeSpeedValues();
    }

    void Update()
    {
        CalculateMovement();
    }

    private void FixedUpdate()
    {
        // ApplyMovement();
        // ApplyRotation();
    }

    #region Movement Methods for Enemy
    
    void InitializeSpeedValues()
    {
        _speed = speed;
        _velocity = 0;
        _acceleration = acceleration;
        _friction = friction;
    }
    
    void CalculateMovement()
    {
        var targetTransform = target.transform;
        var distance = Vector2.Distance(transform.position, targetTransform.position);
        if (distance > stopDistance)
        {
            _velocity += _acceleration;
        }
        else
        {
            _velocity  = Mathf.MoveTowards(_velocity, 0, _friction);
        }
        
        _velocity = Mathf.Clamp(_velocity, 0, _speed);
    }

    void ApplyMovement()
    {
        transform.position =
            Vector2.MoveTowards(transform.position, target.transform.position, _velocity * Time.deltaTime);
    }

    void ApplyRotation()
    {
        // Rotate
        Vector2 direction = target.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion pointRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, pointRotation, turnSpeed * Time.deltaTime);    
    }
    
    #endregion
}
