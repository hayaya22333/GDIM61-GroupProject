using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Locator_Combat : MonoBehaviour
{
    public static Locator_Combat Instance { get; private set; }
    public CombatController CmbtController { get; private set; }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        GameObject combatController = GameObject.FindWithTag("CombatController");
        CmbtController = combatController.GetComponent<CombatController>();
    }
}
