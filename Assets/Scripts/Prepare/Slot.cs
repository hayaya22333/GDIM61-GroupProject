using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public int index;
    public ChoiceCard currentCard;
    public Sprite sprite;

    public bool Empty()
    {
        if (currentCard == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AssignCard(ChoiceCard card)
    {
        currentCard = card;
        GameController.Instance.SelectCard(index, card.GetID());
    }

    public void ClearCard(ChoiceCard card)
    {
        if(currentCard == card)
        {
            currentCard = null;
            GameController.Instance.ClearSelectCard(index);
        }
    }
}
