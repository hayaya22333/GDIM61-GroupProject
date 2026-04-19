using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCard : GeneralCombatCard
{
    void Awake()
    {
        side = "Player";
    }

    public override void StartTurn()
    {
        Debug.Log("It's player card " + id + "'s turn.");
        _spriteRenderer.color = Color.green;
        startPos = transform.position;

        combatController.inTurn = true;
        onTurn = true;
        locked = false;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (isDragging || !onTurn) return;

        if (other.TryGetComponent<GeneralCombatCard>(out GeneralCombatCard target))
        {
            if (target.side == side) return;
            combatController.InflictAttack(id, target.id, atk);
            transform.position = startPos;
            onTurn = false;
            EndTurn();
        }
    }
}
