using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public int index;
    public ChoiceCard currentCard;
    public Sprite sprite;

    private PrepareController controller;

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
/*
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
*/
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("collided");
        if (other.CompareTag("Card"))
        {
            ChoiceCard choiceCard = other.GetComponent<ChoiceCard>();
            int id = choiceCard.ID;
            if (GameController.Instance.AlreadyHaveCard(id) == false)
            {
                GameController.Instance.combatCard.Add(id);
                GameController.Instance.combatCardStore = new List<int>(GameController.Instance.combatCard);
                Debug.Log(id);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Hayaya: debugging.
        if (PrepareController.Instance.cardsLocked)
        {
            return;
        }

        if (other.CompareTag("Card"))
        {
            ChoiceCard choiceCard = other.GetComponent<ChoiceCard>();
            int id = choiceCard.ID;
            GameController.Instance.combatCard.Remove(id);
            GameController.Instance.combatCardStore = new List<int>(GameController.Instance.combatCard);
        }
    }
}
