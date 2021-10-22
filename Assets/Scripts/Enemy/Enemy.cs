
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject target;
    public float stopDistance;
    public float speed;
    public float acceleration;
    public float friction;
    public float turnSpeed;

    public Vector3[] dashTargets;

    // private Rigidbody2D _rigidBody;
    private float _speed;
    private float _velocity;
    private float _acceleration;
    private float _friction;
    private int _negOrPos = 9;
    
    
    void Start()
    {
        // _rigidBody = GetComponent<Rigidbody2D>();
        InitializeSpeedValues();
        FindDashTarget(transform.position, target.transform.GetChild(0), 1);
        FindDashTarget(dashTargets[1], target.transform.GetChild(0), 2);
        FindDashTarget(dashTargets[2], target.transform.GetChild(0), 3);
    }

    void Update()
    {
        CalculateMovement();
        Debug.Log(Random.Range(0,2));
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
    
    #region Dash Target Gathering

    private void FindDashTarget(Vector3 enemyPos, Transform playerTransform, int arrayIndex)
    {
        // store enemies first position
        if (arrayIndex == 1) dashTargets[0] = enemyPos;
        var playerPosition = playerTransform.position;
        if (_negOrPos == 9) _negOrPos = Random.Range(0, 2);
        
        var lineAngle = Random.Range(5, 45);
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
    
    #endregion
}
