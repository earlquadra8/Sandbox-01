using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildBlock : MonoBehaviour
{
    public GameObject newBlock;
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitInfo, 500))
            {
                Vector3 blockPos = hitInfo.point + hitInfo.normal * transform.localScale.x * 0.5f;
                //Vector3 blockPos = hitInfo.transform.position + hitInfo.normal * transform.localScale.x;

                blockPos.x = Mathf.Round(blockPos.x);
                blockPos.y = Mathf.Round(blockPos.y);
                blockPos.z = Mathf.Round(blockPos.z);

                GameObject block = (GameObject)Instantiate(newBlock, blockPos, Quaternion.identity);
                //block.transform.localScale = transform.localScale;
                block.transform.parent = this.transform;

                CombineBlock(block);
            }
        }
	}

    void CombineBlock(GameObject newBlock)
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combines = new CombineInstance[meshFilters.Length];
        Destroy(gameObject.GetComponent<MeshCollider>());
        for (int i = 0; i < meshFilters.Length; i++)
        {
            combines[i].mesh = meshFilters[i].sharedMesh;
            combines[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
        }
        Mesh mesh = transform.GetComponent<MeshFilter>().mesh = new Mesh();
        mesh.CombineMeshes(combines, true);
        mesh.RecalculateBounds();
        mesh.RecalculateBounds();

        gameObject.AddComponent<MeshCollider>();
        gameObject.SetActive(true);

        Destroy(newBlock);
    }
}
