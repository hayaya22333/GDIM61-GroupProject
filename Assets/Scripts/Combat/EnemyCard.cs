using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCard : GeneralCombatCard
{
    private int turnDuration = 2;

    void Awake()
    {
        side = "Enemy";
    }

    public override void StartTurn()
    {
        Debug.Log("It's enemy card " + id + "'s turn.");
        _spriteRenderer.color = Color.green;
        startPos = transform.position;

        combatController.inTurn = true;
        onTurn = true;
        StartCoroutine(EnemyAutoAttack(turnDuration));
    }

    IEnumerator EnemyAutoAttack(int seconds)
    {
        while (seconds > 0)
        {
            Debug.Log(seconds);
            yield return new WaitForSeconds(1f);
            seconds--;
        }
        combatController.InflictAttackRandom(id, atk);
        Debug.Log("Attack ended.");
        EndTurn();
    }
}
