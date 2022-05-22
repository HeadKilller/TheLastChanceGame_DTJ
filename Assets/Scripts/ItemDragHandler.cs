using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler
{
    Vector3 defaultPosition;

    public void OnDrag(PointerEventData eventData)
    {

        transform.position = Input.mousePosition;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localPosition = defaultPosition;
    }

    // Start is called before the first frame update
    void Start()
    {
        defaultPosition = transform.localPosition;
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
