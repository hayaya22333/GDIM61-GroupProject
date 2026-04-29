using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;

public class ChoiceCard : DragObject
{
    public CardNode card;
    //[SerializeField] private Image image;
    [SerializeField] private SpriteRenderer spriteRenderer;
    

    private Vector3 DockPosition;
    private Slot current;
    private Slot hover;
    private PrepareController prepareController;

    public void SetUp(CardNode data, Vector3 position)
    {
        card = data;
        DockPosition = position;
        spriteRenderer.sprite = data.skills[0].cardSprite;
        //image.sprite = data.skills[0].cardSprite;
    }
    public void ShowTextDetail()
    {
        //prepareController.detail.text = card.cardDesciption;
        Debug.Log("click");
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
            DropCard();
        }
    }

    private void DropCard()
    {
        if (hover.Empty())
        {
            transform.position = hover.transform.position;
            hover.AssignCard(this);
            current = hover;
        }
        else
        {
            transform.position = DockPosition;
        }
    }
/*
    private void OnMouseDown()
    {   
        if (locked) return;

        isDragging = true;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        offset = transform.position - mousePos;

        if (!locked && current != null)
        {
            current.ClearCard(this);
            current = null;
        }
        ShowTextDetail();
    }*/

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isDragging)
        {
            if(other.CompareTag("CardDock") && current == null)
            {
                Slot slot = other.GetComponent<Slot>();
                if(slot != null && slot.Empty())
                {
                    transform.position = other.transform.position;
                    slot.AssignCard(this);
                    slot.sprite = spriteRenderer.sprite;
                    current = slot;
                }
            }
        }

        if (other.CompareTag("CardDock"))
        {
            Slot slot = other.GetComponent<Slot>();
            if(slot!= null)
            {
                hover = slot;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("CardDock"))
        {
            Slot slot = other.GetComponent<Slot>();
            if(hover == slot)
            {
                hover = null;
            }
            if(current == slot && isDragging)
            {
                current.ClearCard(this);
                current = null;
            }

        }
    }
}
