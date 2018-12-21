using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftBlockSpawn : MonoBehaviour
{
    public string blockName = "myBlock";
    int spawnedNum = 0;
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, 250f))
            {
                Vector3 pos = hitInfo.transform.position + hitInfo.normal;
               
                GameObject block = new GameObject();
                block.transform.position = pos;
                block.name = blockName + spawnedNum.ToString("000");
                spawnedNum++;
                block.AddComponent<CraftBlockScript>();
            }
        }
    }
}
