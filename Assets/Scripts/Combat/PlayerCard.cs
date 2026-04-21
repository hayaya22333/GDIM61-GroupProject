using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCard : GeneralCombatCard
{
    [Header("Player Card Components")]
    [SerializeField] protected CardNode scriptableObj;
    public GameObject actionCardPrefab;
    [SerializeField] List<ActionCard> actionCards;
    private float cardSpacing = 2.5f;

    void Awake()
    {
        side = GameSide.Player;

        hp = scriptableObj.cardHP;
        spd = scriptableObj.cardSPD;
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

        turnCountDown += 10000;
        combatController.ScootCards(id, turnCountDown);
        combatController.inTurn = false;
        onTurn = false;
    }

    public void SpawnActionCards()
    {
        Debug.Log("Spawning action cards");
        
        int skillCount = scriptableObj.skills.Count;
        if (skillCount == 0)
        {
            Debug.Log("No action cards in this scriptable object");
            EndTurn();
        }
        // calculate 
        float totalWidth = (skillCount - 1) * cardSpacing;

        for (int i = 0; i < skillCount; i++)
        {
            // Centering formula
            float offsetX = i * cardSpacing - (totalWidth / 2f);

            Vector3 spawnPos = combatController.actionCardSpawn.position + new Vector3(offsetX, 0, 0);

            ActionCard actionCard = Instantiate(
                actionCardPrefab,
                spawnPos,
                combatController.actionCardSpawn.rotation
            ).GetComponent<ActionCard>();

            actionCard.SetAttributes(scriptableObj.skills[i], this, id);
            actionCards.Add(actionCard);
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
