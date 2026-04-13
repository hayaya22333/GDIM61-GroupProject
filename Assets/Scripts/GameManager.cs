using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int coinCount = 0;

    public delegate void IntDelegate(int x);
    public delegate void EmptyDelegate();

    public event IntDelegate UpdateCoin;

    private void OnEnable()
    {
        Card.CardSold += HandleCardSold;
        Tradable.TradableBought += HandleTradableBought;
    }

    private void OnDisable()
    {
        Card.CardSold -= HandleCardSold;
        Tradable.TradableBought -= HandleTradableBought;
    }

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
