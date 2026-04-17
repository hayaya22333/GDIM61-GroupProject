using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;

    private void Start()
    {
        TradeLocator.Instance.Controller.UpdateCoin += HandleUpdateCoin;
    }

    private void HandleUpdateCoin(int coinCount)
    {
        coinText.text = coinCount.ToString();
    }
}
