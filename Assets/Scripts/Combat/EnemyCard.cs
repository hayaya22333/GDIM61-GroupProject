using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCard : GeneralCombatCard
{
    private float turnDuration = 2;
    [SerializeField] protected FightNode scriptableObj;

    void Awake()
    {
        side = GameSide.Enemy;

        hp = scriptableObj.enemyHP;
        spd = scriptableObj.enemySPD;
        atk = scriptableObj.enemyATK;
    }

    public override void StartTurn()
    {
        Debug.Log("It's enemy card " + id + "'s turn.");
        _spriteRenderer.color = Color.green;

        combatController.inTurn = true;
        onTurn = true;
        StartCoroutine(EnemyAutoAttack(turnDuration));
    }

    public override void EndTurn()
    {
        _spriteRenderer.color = Color.white;

        turnCountDown += spd + 1;
        combatController.ScootCards(id, turnCountDown);
        combatController.inTurn = false;
        onTurn = false;
    }

    IEnumerator EnemyAutoAttack(float seconds)
    {
        while (seconds > 0)
        {
            yield return new WaitForSeconds(0.1f);
            seconds -= 0.1f;
        }
        combatController.InflictAttackRandom(id, combatController.playerIDs, atk);
        EndTurn();
    }
}
