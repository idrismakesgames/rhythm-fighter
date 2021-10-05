using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float acceleration;
    public float friction;
    public float turnSpeed;
    public float analogDeadzone;
        
    private Rigidbody2D _rigidBody;
    private Vector2 _velocity;
    private float _horizontalInput;
    private float _verticalInput;
    private float _direction;
    
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _velocity = new Vector2(0, 0);
    }

    void Update()
    {
        GetInput();
        CalculateMovement();
    }

    private void FixedUpdate()
    {
        ApplyMovementAndRotation();
    }

    #region Movement Methods for Player
    void GetInput()
    {
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
            horizontalSpeed = Mathf.MoveTowards(horizontalSpeed, 0, friction);
        }
        horizontalSpeed += acceleration * _horizontalInput;
        
        if (MyMethods.Sign(veticalSpeed) != MyMethods.Sign(_verticalInput)) {
            veticalSpeed = Mathf.MoveTowards(veticalSpeed, 0, friction);
        }
        veticalSpeed += acceleration * _verticalInput;

        _velocity = new Vector2(horizontalSpeed,veticalSpeed);
        _velocity = Vector2.ClampMagnitude(_velocity, speed);
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
}
