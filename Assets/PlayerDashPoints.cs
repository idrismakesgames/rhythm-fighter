using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashPoints : MonoBehaviour
{
    public GameObject enemyObject;
    public float lineAngle;
    public float lineAngle2;
    private Vector2 _enemyDirection;
    private float _enemyAngle;
    

    void Update()
    {
        Vector3 enemyTransform = enemyObject.transform.position;
        _enemyDirection = enemyTransform - transform.position;
        
        _enemyAngle = Mathf.Atan2(_enemyDirection.y, _enemyDirection.x) * Mathf.Rad2Deg;
        Quaternion pointRotation = Quaternion.AngleAxis(_enemyAngle, Vector3.forward);
        pointRotation *= Quaternion.Euler(Vector3.forward * -lineAngle);
        transform.rotation = pointRotation;
        
        var startPosition = transform.position;
        var endPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        endPosition += transform.right * 3;
        
        Debug.Log(startPosition);
        Debug.Log(endPosition);
        Debug.DrawLine(startPosition, endPosition, Color.white);
        
        enemyTransform = enemyObject.transform.position;
        _enemyDirection = enemyTransform - transform.position;
        
        _enemyAngle = Mathf.Atan2(_enemyDirection.y, _enemyDirection.x) * Mathf.Rad2Deg;
        pointRotation = Quaternion.AngleAxis(_enemyAngle, Vector3.forward);
        pointRotation *= Quaternion.Euler(Vector3.forward * -lineAngle2);
        transform.rotation = pointRotation;
        
        startPosition = transform.position;
        endPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        endPosition += transform.right * 3;
        
        Debug.Log(startPosition);
        Debug.Log(endPosition);
        Debug.DrawLine(startPosition, endPosition, Color.red);
    }

}
