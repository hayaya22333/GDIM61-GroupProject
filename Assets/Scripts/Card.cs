using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] private CardNode _cardNode;

    public delegate void IntDelegate(int x);
    public static event IntDelegate CardSold;

    private DragObject _dragObject;

    private void Awake()
    {
        _dragObject = GetComponent<DragObject>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if ((other.CompareTag("Merchant")) && (!_dragObject.isDragging))
        {
            Destroy(gameObject);
            Debug.Log("sold");
            CardSold.Invoke(20);
        }
    }
}
