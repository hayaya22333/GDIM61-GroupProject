using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private GameManager gameManager;

    private void Start()
    {
        gameManager.UpdateCoin += HandleUpdateCoin;
    }

    private void HandleUpdateCoin(int coinCnt)
    {
        coinText.text = coinCnt.ToString();
    }
}
