using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FarmLocator : MonoBehaviour
{
    public static FarmLocator Instance { get; private set; }
    public FarmController Controller { get; private set; }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        GameObject farmController = GameObject.FindWithTag("LocalController");
        Controller = farmController.GetComponent<FarmController>();
    }
}
