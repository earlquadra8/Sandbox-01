using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GetBound : MonoBehaviour
{
    public Transform target;
    public Transform indicator;

    int layermask = (1 << 10) + (1 << 8);

    private void Start()
    {
        
    }

    private void Update()
    {
        RaycastHit hitInfo;
        Physics.Raycast(transform.position, target.position - transform.position, out hitInfo, Mathf.Infinity, layermask);

        print(hitInfo.point);
        indicator.position = hitInfo.point;
    }

}
