using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float acceleration;
    public float friction;
    public float turnSpeed;
    public float analogDeadzone;
    public GameObject dashEffect;
    public float dashSpeed;
    public float dashAcceleration;
    public float dashFriction;
    public float dashTime;
    

    private Rigidbody2D _rigidBody;
    private TrailRenderer _trail;
    private float _speed;
    private Vector2 _velocity;
    private float _acceleration;
    private float _friction;
    private float _horizontalInput;
    private float _verticalInput;
    private bool _isDashing;
    private float _dashTimer;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _trail = GetComponent<TrailRenderer>();
        InitializeSpeedValues();
    }

    void Update()
    {
        if (!_isDashing) GetInput();
        else DepleteDash();
        CalculateMovement();
    }

    private void FixedUpdate()
    {
        ApplyMovementAndRotation();
    }

    #region Movement Methods for Player

    void InitializeSpeedValues()
    {
         _velocity = new Vector2(0, 0);
        _speed = speed;
        _acceleration = acceleration;
        _friction = friction;
    }
    
    void GetInput()
    {
        if (Input.GetButtonDown("Fire1") && !_isDashing) SetStartsDashVariables();

        _horizontalInput = 0;
        _verticalInput = 0;

        var tempHorizontalAxis = Input.GetAxisRaw("Horizontal");
        var tempVerticalAxis = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(tempHorizontalAxis) > analogDeadzone) _horizontalInput = tempHorizontalAxis;
        else _horizontalInput = 0;        
        if (Mathf.Abs(tempVerticalAxis) > analogDeadzone) _verticalInput = tempVerticalAxis;
        else _verticalInput = 0;
    }

    void CalculateMovement()
    {
        var horizontalSpeed = _velocity.x;
        var veticalSpeed = _velocity.y;
        
        if (MyMethods.Sign(horizontalSpeed) != MyMethods.Sign(_horizontalInput)) {
            horizontalSpeed = Mathf.MoveTowards(horizontalSpeed, 0, _friction);
        }
        horizontalSpeed += _acceleration * _horizontalInput;
        
        if (MyMethods.Sign(veticalSpeed) != MyMethods.Sign(_verticalInput)) {
            veticalSpeed = Mathf.MoveTowards(veticalSpeed, 0, _friction);
        }
        veticalSpeed += _acceleration * _verticalInput;

        _velocity = new Vector2(horizontalSpeed,veticalSpeed);
        _velocity = Vector2.ClampMagnitude(_velocity, _speed);
    }

    void ApplyMovementAndRotation()
    {
        // Move
        Vector2 currentPosition = transform.position;
        Vector2 finalPosition = _rigidBody.position + _velocity * Time.deltaTime;
        _rigidBody.MovePosition(finalPosition);

        // Rotate
        Vector2 direction = finalPosition - currentPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion pointRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
        if (Mathf.Abs(_horizontalInput) > 0 || Mathf.Abs(_verticalInput) > 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, pointRotation, turnSpeed * Time.deltaTime);
        }
    }
    #endregion
    
    #region Dashing Methods
    void SetStartsDashVariables()
    {
        _isDashing = true;
        _dashTimer = dashTime;
        _speed = dashSpeed;
        _acceleration = dashAcceleration;
        _friction = dashFriction;

        var playerTransform = transform; 
        Instantiate(dashEffect, playerTransform.position, playerTransform.rotation); // * Quaternion.Euler(0, 0, 90);
    }
    
    void SetEndDashVariables()
    {
        _isDashing = false;
        _dashTimer = 0;
        _speed = speed;
        _acceleration = acceleration;
        _friction = friction;
    }

    void DepleteDash()
    {
        _dashTimer -= Time.deltaTime;
        if (_dashTimer <= 0) SetEndDashVariables();
    }
    #endregion
}
