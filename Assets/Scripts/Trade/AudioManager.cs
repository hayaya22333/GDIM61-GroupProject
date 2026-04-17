using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource sell;

    void Start()
    {
        TradeLocator.Instance.Controller.UpdateCoin += HandleUpdateCoin;
    }

    void HandleUpdateCoin(int x)
    {
        sell.Play();
    }
}
