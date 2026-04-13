using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : DragObject
{
    [SerializeField] private CardNode _cardNode;

    public delegate void IntDelegate(int x);
    public static event IntDelegate CardSold;

    private void OnTriggerStay2D(Collider2D other)
    {
        if ((other.CompareTag("Merchant")) && (!isDragging))
        {
            Destroy(gameObject);
            Debug.Log("sold");
            CardSold.Invoke(20);
        }
    }
}
