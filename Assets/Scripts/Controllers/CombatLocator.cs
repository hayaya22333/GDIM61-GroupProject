using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CombatLocator : MonoBehaviour
{
    public static CombatLocator Instance { get; private set; }
    public CombatController Controller { get; private set; }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        GameObject combatController = GameObject.FindWithTag("LocalController");
        Controller = combatController.GetComponent<CombatController>();
    }
}
