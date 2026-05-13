using System.Collections;
using System.Collections.Generic;
// using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;

public class ChoiceCard : DragObject
{
    public CardNode card;
    public int ID;
    //[SerializeField] private Image image;
    [SerializeField] private SpriteRenderer spriteRenderer;
    public bool canStay = true;
    public bool alreadyStay = false;

    public Vector3 DockPosition;

    public void SetUp(CardNode data, Vector3 position)
    {
        card = data;
        DockPosition = position;
        spriteRenderer.sprite = data.skills[0].cardSprite;
    }

    public int GetID()
    {
        return card.cardID;
    }

    public CardNode GetData()
    {
        return card;
    }

    private void Update()
    {
        if(isDragging && Input.GetMouseButtonUp(0))
        {
            //DropCard();
        }
    }

    // Hayaya: temporary card position lock
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isDragging)
        {
            if(other.CompareTag("CardDock") && canStay)
            {
                transform.position = other.transform.position;
                transform.rotation = other.transform.rotation;
            }
        }
    }

}
