using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Platform : MonoBehaviour
{
    public bool isDraggable = true;
    public bool isDragged = false;
    public GameObject gameObject;

    private void Start()
    {
        
    }

    private void Update()
    {
        if(isDragged)
        {
            gameObject.transform.position = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private void OnMouseOver()
    {
        if(isDraggable && Input.GetMouseButtonDown(0))
        {
            isDragged = true;
        }
    }

    private void OnMouseUp()
    {
        isDragged = false;
        Debug.Log("position: " + gameObject.transform.position);
        gameObject.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
    }
}
