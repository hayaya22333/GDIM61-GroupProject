using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TradeController : MonoBehaviour
{
    #region Variables
    // Events
    public delegate void IntDelegate(int x);
    public delegate void EmptyDelegate();

    public event IntDelegate UpdateCoin;

    // Variables
    public int coinCount = 0;
    #endregion

    void Awake()
    {
    }

    void OnEnable()
    {
        Debug.Log("Entered Trade.");
        Card.CardSold += HandleCardSold;
        Tradable.TradableBought += HandleTradableBought;
    }
    void OnDisable()
    {
        Debug.Log("Left Trade.");
        Card.CardSold -= HandleCardSold;
        Tradable.TradableBought -= HandleTradableBought;
    }

    void HandleCardSold(int cardValue)
    {
        coinCount += cardValue;
        Debug.Log("Gained " + cardValue + " coins :D");
        UpdateCoin.Invoke(coinCount);
    }

    void HandleTradableBought(int price)
    {
        coinCount -= price;
        Debug.Log("Spent " + price + " coins :O");
        UpdateCoin.Invoke(coinCount);
    }

    public void Click(int i)
    {
        SceneManager.LoadScene(i);
    }
}
