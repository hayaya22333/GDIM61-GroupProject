using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public int index;
    public ChoiceCard currentCard;

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
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("collided");
        if (other.CompareTag("Card"))
        {
            ChoiceCard choiceCard = other.GetComponent<ChoiceCard>();
            int id = choiceCard.ID;
            if (currentCard != null && currentCard != choiceCard)
            {
                int oldid = currentCard.ID;
                GameController.Instance.combatCard.Remove(oldid);
                GameController.Instance.combatCardStore = new List<int>(GameController.Instance.combatCard);

                currentCard.transform.position = currentCard.DockPosition;
                Debug.Log(currentCard.transform.position);
                currentCard = null;
                Debug.Log($"Clear {oldid}");
            }

            currentCard = choiceCard;

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
