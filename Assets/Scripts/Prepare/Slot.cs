using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public int index;
    public ChoiceCard currentCard;

    [SerializeField] private TMP_Text text;

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

    void Start()
    {
        text.text = "";
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PutCard(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        PutCard(other);
    }

    private void PutCard(Collider2D other)
    {
        if (other.CompareTag("Card"))
        {
            ChoiceCard choiceCard = other.GetComponent<ChoiceCard>();
            if (choiceCard == null)
            {
                return;
            }

            int id = choiceCard.ID;
            if (currentCard != null && currentCard != choiceCard)
            {
                text.text = "You can't put it here!";
                return;
            }

            text.text = "";
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
        if (PrepareController.Instance.cardsLocked)
        {
            return;
        }

        if (other.CompareTag("Card"))
        {
            ChoiceCard choiceCard = other.GetComponent<ChoiceCard>();
            currentCard = null;
            int id = choiceCard.ID;

            GameController.Instance.combatCard.Remove(id);
            GameController.Instance.combatCardStore = new List<int>(GameController.Instance.combatCard);
        }
    }
}
