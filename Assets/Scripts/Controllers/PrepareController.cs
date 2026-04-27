using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrepareController : MonoBehaviour
{
    [SerializeField] private Transform[] dockPosition;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private TMP_Text cardCount;
    [SerializeField] private Slot[] slot;
    public TMP_Text detail;
    private List<ChoiceCard> choiceCards = new List<ChoiceCard>();

    public void Start()
    {
        detail.enabled = false;
        BuildDock();
    }
    public void Click(int i)
    {
        SceneManager.LoadScene(i);
    }

    public void Click()
    {
        detail.enabled = true;
    }

    public void BuildDock()
    {
        ClearDock();
        choiceCards.Clear();
        List<CardNode> ownedCards = GameController.Instance.GetOwnCard();
        cardCount.text = "Cards: " + ownedCards.Count;

        for (int i = 0; i < ownedCards.Count; i++)
        {
            if(i>= dockPosition.Length)
            {
                break;
            }
            Vector3 spawnPos = dockPosition[i].position;
            GameObject newCard = Instantiate(cardPrefab, spawnPos, Quaternion.identity);

            ChoiceCard card = newCard.GetComponent<ChoiceCard>();
            if (card != null)
            {
                card.SetUp(ownedCards[i], spawnPos);
                choiceCards.Add(card);
            }
        }
    }

    public void ClearDock()
    {
        for(int i = 0; i < choiceCards.Count; i++)
        {
            if(choiceCards[i] != null)
            {
                Destroy(choiceCards[i].gameObject);
            }
        }
    }

}
