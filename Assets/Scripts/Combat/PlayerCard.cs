using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCard : GeneralCombatCard
{
    [Header("Player Card Components")]
    [SerializeField] protected CardNode scriptableObj;
    public GameObject actionCardPrefab;
    [SerializeField] List<ActionCard> actionCards;

    void Awake()
    {
        side = GameSide.Player;

        hp = scriptableObj.cardHP;
        spd = scriptableObj.cardSPD;
        atk = scriptableObj.cardATK;
    }

    public override void StartTurn()
    {
        Debug.Log("It's player card " + id + "'s turn.");
        SpawnActionCards();
        _spriteRenderer.color = Color.green;

        combatController.inTurn = true;
        onTurn = true;
    }

    public override void EndTurn()
    {
        _spriteRenderer.color = Color.white;

        ClearActionCards();

        turnCountDown += spd + 1;
        combatController.ScootCards(id, turnCountDown);
        combatController.inTurn = false;
        onTurn = false;
        Debug.Log("Ending turn");
    }

    public void SpawnActionCards()
    {
        Debug.Log("Spawning action cards");
        for (int i = 0; i < scriptableObj.skills.Count; i++)
        {
            ActionCard actionCard = Instantiate(
                actionCardPrefab,
                combatController.actionCardSpawn.position,
                combatController.actionCardSpawn.rotation
            ).GetComponent<ActionCard>();
            actionCard.SetAttributes(scriptableObj.skills[i], this, id);
            actionCards.Add(actionCard);
        }
        if (actionCards.Count == 0)
        {
            Debug.Log("No action cards in this scriptable object");
            EndTurn();
        }
    }

    public void ClearActionCards()
    {
        foreach (ActionCard actionCard in actionCards)
        {
            if (actionCard != null)
            {
                Destroy(actionCard.gameObject);
            }
        }
        actionCards?.Clear();
    }
}
