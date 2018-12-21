using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shapecast : MonoBehaviour
{

    Vector3 shapeCenter;
    Vector3 shapeHalfSize;
    Vector3 castDir;
    RaycastHit hitInfo;
    RaycastHit[] hitInfos;

    private void Start()
    {
       
        shapeHalfSize = Vector3.one * 0.5f;
        castDir = Vector3.down;
    }
    void Update ()
    {
        shapeCenter = transform.position;
        //if (Physics.BoxCast(shapeCenter, shapeHalfSize, castDir, out hitInfo))
        //{
        //    print(hitInfo.triangleIndex);
        //}

        hitInfos = Physics.BoxCastAll(shapeCenter, shapeHalfSize, castDir);
        if (hitInfos.Length > 0)
        {
            for (int i = 0; i < hitInfos.Length; i++)
            {
                print(hitInfos[i].triangleIndex);
            }
        }
	}

    
}
