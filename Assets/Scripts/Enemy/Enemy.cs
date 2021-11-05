
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject target;
    public float stopDistance;
    public float speed;
    public float acceleration;
    public float friction;
    public float turnSpeed;
    public int minAngle;
    public int maxAngle;

    public Vector3[] dashTargets;
    public GameObject dashEffect;
    public float dashSpeed;
    public float dashAcceleration;
    public float dashFriction;

    private float _speed;
    private float _velocity;
    private float _acceleration;
    private float _friction;
    private int _negOrPos = 9;
    private int _targetNumber = 1;
    private bool _isDashing;
    private float _dashTimer;
    
    
    void Start()
    {
        InitializeSpeedValues();
    }

    void Update()
    {
        if (!_isDashing) GetInput();
        CalculateMovement();
    }

    private void FixedUpdate()
    {
        if (_isDashing)
        {
            ApplyMovement();
            ApplyRotation();
        }
    }

    #region Input methods for Enemy
    
    void GetInput()
    {
        if (Input.GetButtonDown("Fire2") && !_isDashing && _targetNumber <= 4)
        {
            if (_targetNumber == 1 && dashTargets[1] == Vector3.zero)
            {
                // do just before beat
                FindDashTarget(transform.position, target.transform.GetChild(0), 1);
            }
            if (_targetNumber == 2 && dashTargets[2] == Vector3.zero)
            {
                // do just before beat
                FindDashTarget(dashTargets[1], target.transform.GetChild(0), 2);
            }
            if (_targetNumber == 3 && dashTargets[3] == Vector3.zero)
            {
                // dojust before beat
                FindDashTarget(dashTargets[2], target.transform.GetChild(0), 3);
            }
            if (_targetNumber == 4 && dashTargets[4] == Vector3.zero)
            {
                // do just before beat
                dashTargets[4] = target.transform.position;
            }
            SetStartsDashVariables();
        }
    }
    
    #endregion
    
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
        var distance = Vector2.Distance(transform.position, dashTargets[_targetNumber]);
        if (distance > stopDistance)
        {
            _velocity += _acceleration;
        }
        else
        {
            _velocity  = Mathf.MoveTowards(_velocity, 0, _friction);
            if (_velocity < _friction && _isDashing) SetEndDashVariables();
        }
        _velocity = Mathf.Clamp(_velocity, 0, _speed);
    }

    void ApplyMovement()
    {
        transform.position =
            Vector2.MoveTowards(transform.position, dashTargets[_targetNumber], _velocity * Time.deltaTime);
    }

    void ApplyRotation()
    {
        // Rotate
        if (dashTargets[_targetNumber] != transform.position)
        {
            Vector2 direction = dashTargets[_targetNumber] - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion pointRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, pointRotation, turnSpeed * Time.deltaTime);
        }
    }
    
    #endregion
    
    #region Dashing Methods
    void SetStartsDashVariables()
    {
        _isDashing = true;
        _speed = dashSpeed;
        _acceleration = dashAcceleration;
        _friction = dashFriction;

        var enemyTransform = transform;
        Quaternion dashRotation = GetTargetRotation();
        Instantiate(dashEffect, enemyTransform.position, dashRotation); // * Quaternion.Euler(0, 0, 90);
    }
    
    void SetEndDashVariables()
    {
        Debug.Log("ended");
        _isDashing = false;
        _speed = speed;
        _acceleration = acceleration;
        if (_targetNumber < 4) _targetNumber += 1;
        _friction = friction;
    }
    #endregion
    
    #region Dash Target Gathering

    private void FindDashTarget(Vector3 enemyPos, Transform playerTransform, int arrayIndex)
    {
        // store enemies first position
        if (arrayIndex == 1) dashTargets[0] = enemyPos;
        var playerPosition = playerTransform.position;
        if (_negOrPos == 9) _negOrPos = Random.Range(0, 2);
        
        var lineAngle = Random.Range(minAngle, maxAngle);
        if (_negOrPos == 0) lineAngle = -lineAngle;

        var directionFromPlayerToEnemy = enemyPos - playerPosition;
        var angleFromPlayerToEnemy = Mathf.Atan2(directionFromPlayerToEnemy.y, directionFromPlayerToEnemy.x) * Mathf.Rad2Deg;
        Quaternion pointRotation = Quaternion.AngleAxis(angleFromPlayerToEnemy, Vector3.forward);
        pointRotation *= Quaternion.Euler(Vector3.forward * lineAngle);
        playerTransform.rotation = pointRotation;
        
        var endPosition = new Vector3(playerPosition.x, playerPosition.y, playerPosition.z);
        endPosition += playerTransform.right * (4-arrayIndex);

        dashTargets[arrayIndex] = endPosition;
        if (_negOrPos == 0)
        {
            _negOrPos = 1;
        }
        else
        {
            _negOrPos = 0;
            
        }
        Debug.DrawLine(dashTargets[arrayIndex-1], dashTargets[arrayIndex], Color.green, 300);
    }
    
    private Quaternion GetTargetRotation()
    {
        Vector2 direction = dashTargets[_targetNumber] - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(angle, Vector3.forward);
        
    }
    
    #endregion
}
