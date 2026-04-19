using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCard : GeneralCombatCard
{
    private float turnDuration = 2;

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

    IEnumerator EnemyAutoAttack(float seconds)
    {
        while (seconds > 0)
        {
            yield return new WaitForSeconds(0.1f);
            seconds -= 0.1f;
        }
        combatController.InflictAttackRandom(id, atk);
        EndTurn();
    }
}
