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
    }

    // TODO: Create game scenes. //fin
    // Title and PrepareTeam scenes don't exist.
    // For now, create an empty scene and just implement UI for scene switching. // fin
    public enum GameState
    {
        Title,
        Farm,
        PrepareTeam,
        Combat,
        Trade
    }

    // TODO: Switch scenes in here. // fin
    public void SwitchGameState(int stateIndex)
    {
        currentState = (GameState)stateIndex;
        switch(currentState)
        {
            case GameState.Title:
                Debug.Log("Entering Title Screen");
                SceneManager.LoadScene(0);
                break;
            case GameState.Farm:
                Debug.Log("Entering Farm");
                SceneManager.LoadScene(1);
                break;
            case GameState.PrepareTeam:
                Debug.Log("Entering Combat Prep");
                SceneManager.LoadScene(2);
                break;
            case GameState.Combat:
                Debug.Log("Entering Combat");
                SceneManager.LoadScene(1);
                break;
            case GameState.Trade:
                Debug.Log("Entering Trade");
                SceneManager.LoadScene(4);
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

    private Dictionary<int, CardNode> cardMenu = new Dictionary<int, CardNode>();

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

    public void RemoveOwnCard(int id)
    {
        ownCard.Remove(id);
    }

}
