using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Locator_Trade : MonoBehaviour
{
    public static Locator_Trade Instance { get; private set; }
    public TradeController Controller { get; private set; }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        GameObject tradeController = GameObject.FindWithTag("LocalController");
        Controller = tradeController.GetComponent<TradeController>();
    }
}
