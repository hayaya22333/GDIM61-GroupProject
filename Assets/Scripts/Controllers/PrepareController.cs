using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrepareController : MonoBehaviour
{
    //[SerializeField] private Transform[] dockPosition;
    [SerializeField] private Transform dockPositionleft;
    [SerializeField] private Transform dockPositionright;
    [SerializeField] private GameObject cardChoice;
    //[SerializeField] private TMP_Text cardCount;
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
        //cardCount.text = "Cards: " + ownedCards.Count;

        float leftX = dockPositionleft.position.x;
        float rightX = dockPositionright.position.x;
        float y = dockPositionleft.position.y;
        float z = dockPositionleft.position.z;

        if (ownedCards.Count == 1)
        {
            Vector3 centerPos = new Vector3((leftX+rightX)/2f, y, z);
            SpawnOneCard(ownedCards[0], centerPos);
            return;
        }

        float step = (rightX - leftX) / (ownedCards.Count - 1);

        for (int i = 0; i < ownedCards.Count; i++)
        {
            
            Vector3 spawnPos = new Vector3(leftX + step * i, y, z);
            SpawnOneCard(ownedCards[i], spawnPos);
        }
    }

    public void SpawnOneCard(CardNode cardNode, Vector3 vector3)
    {
        GameObject newCard = Instantiate(cardChoice, vector3, Quaternion.identity);
        ChoiceCard card = newCard.GetComponent<ChoiceCard>();
        if (card != null)
            {
                card.SetUp(cardNode, vector3);
                choiceCards.Add(card);
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
