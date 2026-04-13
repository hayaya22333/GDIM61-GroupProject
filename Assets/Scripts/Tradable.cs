using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tradable : ClickObject
{
    public int price = 40;

    public delegate void IntDelegate(int x);
    public static event IntDelegate TradableBought;

    public override void HandleClicked(){
        if (Locator.Instance.Controller.coinCount >= price)
        {
            TradableBought.Invoke(price);
            Destroy(gameObject);
        }else{
            Debug.Log("Not enough money");
        }
    }
}
