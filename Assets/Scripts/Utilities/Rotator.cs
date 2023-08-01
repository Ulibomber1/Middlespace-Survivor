using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private Vector3 rotationValues = new Vector3(0, 0, 0);

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.rotation *= Quaternion.Euler(rotationValues);
    }
}
