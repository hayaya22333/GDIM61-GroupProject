using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class combatHint : MonoBehaviour
{
    [SerializeField] private SpriteRenderer point;
    private bool wasInTurn = false;
    private int turnCnt = 0;

    void Start()
    {
        point = gameObject.GetComponent<SpriteRenderer>();
        point.enabled = false;
    }

    void FixedUpdate()
    {
        if(CombatController.Controller.inTurn)
        {
            if (!wasInTurn)
            {
                turnCnt += 1;
            }
            wasInTurn = true;
        }
        else
        {
            wasInTurn = false;
        }

        if (turnCnt == 2)
        {
            point.enabled = true;
        }

        if (CombatController.Controller.playerMoved)
        {
            Destroy(gameObject);
        }
        
    }
}
