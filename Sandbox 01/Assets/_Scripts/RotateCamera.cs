using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    Transform target;
    private void Start()
    {
       //target = GameObject.Find("Origin Cube").transform;

    }
    void Update ()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            Camera.main.transform.RotateAround(Vector3.zero, Vector3.up, 50 * Time.deltaTime);
        }
        else if(Input.GetKey(KeyCode.Alpha3))
        {
            Camera.main.transform.RotateAround(Vector3.zero, Vector3.up, -50 * Time.deltaTime);
        }
    }
}
