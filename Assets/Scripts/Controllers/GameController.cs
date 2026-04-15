using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    #region Variables
    // Events
    public delegate void IntDelegate(int x);
    public delegate void EmptyDelegate();

    public event IntDelegate UpdateCoin;

    // Variables
    public int coinCount = 0;
    public GameState currentState;
    #endregion

    private void Awake()
    {
        currentState = GameState.Farm;
    }

    #region Game State Management
    // The default game state is Farm, initialized in Awake().
    // Switch state with state enum index.
    public enum GameState
    {
        Farm, // Index = 0
        Combat, // Index = 1
        Trade // Index = 2
    }

    public void SwitchGameState(int stateIndex)
    {
        currentState = (GameState)stateIndex;
    }
    #endregion

    #region Initialization For Different Game States
    private void OnEnable()
    {
        switch(currentState)
        {
            case GameState.Farm:
                Debug.Log("Entering Farm");
                Card.CardSold += HandleCardSold;
                Tradable.TradableBought += HandleTradableBought;
                break;
            case GameState.Combat:
                Debug.Log("Entering Combat");
                break;
            case GameState.Trade:
                Debug.Log("Entering Trade");
                break;
        }

    }

    private void OnDisable()
    {
        switch(currentState)
        {
            case GameState.Farm:
                Debug.Log("Leaving Farm");
                Card.CardSold -= HandleCardSold;
                Tradable.TradableBought -= HandleTradableBought;
                break;
            case GameState.Combat:
                Debug.Log("Leaving Combat");
                break;
            case GameState.Trade:
                Debug.Log("Leaving Trade");
                break;
        }
    }
    #endregion

    private void HandleCardSold(int cardValue)
    {
        coinCount += cardValue;
        Debug.Log("Gained " + cardValue + " coins :D");
        UpdateCoin.Invoke(coinCount);
    }

    private void HandleTradableBought(int price)
    {
        coinCount -= price;
        Debug.Log("Spent " + price + " coins :O");
        UpdateCoin.Invoke(coinCount);
    }
}
