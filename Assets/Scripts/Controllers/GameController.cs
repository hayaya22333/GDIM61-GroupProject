// GameController.cs
// Handles global scene management.
// Should only be accessed by the specific controller for each scene.
// For example, in the Combat scene, only CombatController.cs should access this instance.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    public GameState currentState { get; private set; }

    private void Awake()
    {
        currentState = GameState.Title;
        DontDestroyOnLoad(gameObject);
        

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public enum GameState
    {
        Title,
        Poll,
        PrepareTeam,
        Combat,

    }

    public void SwitchGameState(int stateIndex)
    {
        currentState = (GameState)stateIndex;
        Debug.Log("click button");
        switch(currentState)
        {
            case GameState.Title:
                Debug.Log("Entering Title Screen");
                SceneManager.LoadScene(0);
                break;
            case GameState.Poll:
                Debug.Log("Entering Poll");
                SceneManager.LoadScene(1);
                break;
            case GameState.PrepareTeam:
                Debug.Log("Entering Combat Prep");
                SceneManager.LoadScene(2);
                break;
            case GameState.Combat:
                Debug.Log("Entering Combat");
                SceneManager.LoadScene(3);
                break;
        }
    }

    /* TODO: card transfer:
    1. refer each card to a unique card ID
    2. record ID between scenes using GC
    3. once polled, tell GC to record ID
    4. in prepared, GC tell PC to show cards number (list count) and each card will show corresponding details when clicked
    5. cards will be presented in order from smallest ID to largest
    6. 2 selected ID will be recorded in GC in a new list
    7. in combat, GC tell CC to show specific selected cards data
    */

    //all card
    public List<CardNode> allCard = new List<CardNode>();
    //own card
    public List<int> ownCard = new List<int>();
    //combat card
    public List<int> combatCard = new List<int>();
    public List<int> combatCardStore = new List<int>();

    private Dictionary<int, CardNode> cardMenu = new Dictionary<int, CardNode>();

    //storing combat stage index
    public int combatIndex = 0;

    public void CardDictionary()
    {
        cardMenu.Clear();
        for(int i = 0; i < allCard.Count; i++)
        {
            cardMenu.Add(allCard[i].cardID, allCard[i]);
        }
    }

    public void OwnCard(int id)
    {
        if(ownCard.Contains(id) == false)
        {
            ownCard.Add(id);
            ownCard.Sort();
        }
    }


    public CardNode CheckID(int id)
    {
        if (cardMenu.ContainsKey(id))
        {
            return cardMenu[id];
        }
        return null;
    }

    public List<CardNode> GetOwnCard()
    {
        List<CardNode> result = new List<CardNode>();
        ownCard.Sort();
        Debug.Log("owncard:" + ownCard.Count);
        for (int i = 0; i < ownCard.Count; i++)
        {
            CardNode card = CheckID(ownCard[i]);
            Debug.Log("check id" + ownCard[i]);
            //if(card != null)
            {
                result.Add(card);
            }
        }
        return result;
    }

    public void SelectCard(int index, int id)
    {
        while (combatCard.Count <= index)
        {
            combatCard.Add(-1);
        }

        combatCard[index] = id;
    }

    public void ClearSelectCard(int index)
    {
        combatCard[index] = -1;
    }

    public void ClearAllCombatCard()
    {
        combatCard.Clear();
    }

    public int GetCombatCardID(int index)
    {
        if(index >= 0 && index < combatCard.Count)
        {
            return combatCard[index];
        }
        return -1;
    }

    public bool AlreadyHaveCard(int id)
    {
        if (combatCard.Contains(id))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public List<CardNode> GetCombatCard()
    {
        List<CardNode> result = new List<CardNode>();

        for(int i = 0; i < combatCard.Count; i++)
        {
            if(combatCard[i] != -1)
            {
                CardNode card = CheckID(combatCard[i]);
                if (card != null)
                {
                    result.Add(card);
                }
            }
        }

        result.Sort((a, b) => a.cardID.CompareTo(b.cardID));
        return result;
    }

    public void LoseCards(List<int> _ids)
    {
        foreach (int _id in _ids)
        {
            ownCard?.Remove(_id);
        }
    }
}
