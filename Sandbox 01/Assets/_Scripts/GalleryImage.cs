using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GalleryImage : MonoBehaviour
{
    public GameObject content;
    public GameObject container;
        
    private RectTransform rectTransform;
    private bool isImageEdged = false;

    public void OnImageScrollValueChange(Vector2 pos)
    {
        isImageEdged = (pos.x >= 1f || pos.x <= 0f);
        container.GetComponent<ScrollRect>().enabled = !isImageEdged;
        if (isImageEdged)
        {
            rectTransform.localScale = Vector3.one;
            rectTransform.anchoredPosition = Vector2.zero;
        }
    }

    void Start ()
    {
        rectTransform = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        content.GetComponent<ScrollRect>().enabled = (rectTransform.localScale == Vector3.one || isImageEdged);
    }
}
