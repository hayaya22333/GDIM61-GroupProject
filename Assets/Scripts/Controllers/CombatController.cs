using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CombatController : MonoBehaviour
{
    public static CombatController Instance {get; private set;}

    void Awake()
    {
        Instance = this;
    }

    [Header("Components")]
    [SerializeField] private List<GeneralCombatCard> allCards;
    [SerializeField] protected List<int> enemyIDs;
    [SerializeField] protected List<int> playerIDs;
    [SerializeField] private GameObject GameEndUI;

    [Header("Game Status")]
    [SerializeField] private int gameLevel;
    public bool combatEnd = false;
    public bool inTurn = false;
    [SerializeField] private int activePlayerCnt;
    [SerializeField] private int activeEnemyCnt;

    private Dictionary<string, int> collectDrop = new Dictionary<string, int>();

    public event Action NextTurn;
    public event Action<int, int> TurnScoot;
    public event Action<int, int, int> Attack;

    void Start()
    {
        GameEndUI.SetActive(false);
        int i = 0;

        // Register cards for game system
        foreach (GeneralCombatCard card in allCards)
        {
            AddCard(card, i);
            i += 1;
        }
    }

    void Update()
    {
        if (combatEnd) return;

        TryNextTurn();
        CheckEnd();
    }

    public void TryNextTurn()
    {
        if (inTurn) return;
        NextTurn.Invoke();
    }

    public void ScootCards(int insertedID, int insertedCountDown)
    {
        TurnScoot.Invoke(insertedID, insertedCountDown);
    }

    public void KillCard(int cardID)
    {
        GeneralCombatCard card = allCards[cardID];

        switch(card.side)
        {
            case "Enemy":
                activeEnemyCnt -= 1;
                enemyIDs.Remove(cardID);
                break;
            case "Player":
                activePlayerCnt -= 1;
                playerIDs.Remove(cardID);
                break;
        }
        card.enabled = false;
        card.gameObject.SetActive(false);
    }

    public void AddCard(GeneralCombatCard card, int i)
    {
        card.Initiate(i);
        switch(card.side)
        {
            case "Enemy":
                enemyIDs.Add(i);
                activeEnemyCnt += 1;
                break;
            case "Player":
                playerIDs.Add(i);
                activePlayerCnt += 1;
                break;
        }
    }

    public void CollectDrop(string item, int amount)
    {
        if (collectDrop.ContainsKey(item))
        {
            collectDrop[item] += amount;
            collectDrop.Clear();
        }
    }

    public void InflictAttack(int attackerID, int targetID, int damage)
    {
        Attack.Invoke(attackerID, targetID, damage);
    }

    public void InflictAttackRandom(int attackerID, int damage)
    {
        var targetID = -1;
        GeneralCombatCard attacker = allCards[attackerID];
        if (attacker.side == "Player")
        {
            targetID = enemyIDs[UnityEngine.Random.Range(0, playerIDs.Count)];
        }
        else if (attacker.side == "Enemy")
        {
            targetID = playerIDs[UnityEngine.Random.Range(0, playerIDs.Count)];
        }
        Attack.Invoke(attackerID, targetID, damage);
    }

#region End Combat
    private void CheckEnd()
    {
        if (activeEnemyCnt == 0 || activePlayerCnt == 0)
        {
            EndCombat();
        }
    }

    private void EndCombat()
    {
        combatEnd = true;
        GameEndUI.SetActive(true);
    }
#endregion
}
