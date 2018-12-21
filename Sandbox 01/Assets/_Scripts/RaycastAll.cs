using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastAll : MonoBehaviour
{
    public Transform rayEnd;
    RaycastHit[] hitInfos;
	void Start ()
    {
		
	}
	
	void Update ()
    {
        Ray ray = new Ray(transform.position, rayEnd.position - transform.position);
        hitInfos = Physics.RaycastAll(ray, Vector3.Distance(rayEnd.position, transform.position));
        if (hitInfos.Length > 0)
        {
            for (int i = 0; i < hitInfos.Length; i++)
            {
                if (hitInfos[i].transform.name == rayEnd.name)
                {
                    continue;
                }
                //print(hitInfos[i].transform.name);
                hitInfos[i].transform.gameObject.SetActive(false);
            }
        }
	}
}
