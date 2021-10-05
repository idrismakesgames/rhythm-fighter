using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radius : MonoBehaviour
{
    public GameObject anchorObject;

    void FixedUpdate()
    {
        transform.position = anchorObject.transform.position;
    }
}
