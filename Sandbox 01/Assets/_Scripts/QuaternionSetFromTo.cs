using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuaternionSetFromTo : MonoBehaviour
{

    public Transform m_NextPoint;
    Quaternion m_MyQuaternion;
    Quaternion thisQuaternion;
    Vector3 startPos;
    float m_Speed = 1.0f;

    void Start()
    {
        m_MyQuaternion = new Quaternion();
        thisQuaternion = transform.rotation;
        startPos = transform.position;
    }

    void Update()
    {
        //Set the Quaternion rotation from the GameObject's position to the next GameObject's position
        m_MyQuaternion.SetFromToRotation(startPos, m_NextPoint.position);
        //Move the GameObject towards the second GameObject
        //transform.position = Vector3.Lerp(transform.position, m_NextPoint.position, m_Speed * Time.deltaTime);
        //Rotate the GameObject towards the second GameObject
        transform.rotation = m_MyQuaternion * transform.rotation;
        print(m_MyQuaternion);
    }
}
