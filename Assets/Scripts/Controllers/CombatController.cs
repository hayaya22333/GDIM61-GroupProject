using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

public class CombatController : MonoBehaviour
{
    // Variables
    #region Variables
    public static CombatController Controller { get; private set; }
    List<int> deadPlayerCards = new List<int>();  

    [Header("Card Preparation")]
    public GameObject enemyPrefab;
    public List<FightNode> enemyPool;
    public GameObject playerPrefab;
    public List<CardNode> playerPool;

    public List<int> playerFixedIDs;
    public List<int> enemyFixedIDs;

    [Header("Active Components")]
    [SerializeField] private List<GeneralCombatCard> allCards;
    [SerializeField] public List<int> enemyIDs;
    [SerializeField] public List<int> playerIDs;

    [Header("Anchors")]
    [SerializeField] public List<Transform> playerAnchors;
    [SerializeField] public List<Transform> enemyAnchors;
    [SerializeField] public Transform actionCardSpawn;

    [Header("Game Status")]
    [SerializeField] private int gameLevel;
    public bool combatEnd = false;
    public bool inTurn = false;
    [SerializeField] private int activePlayerCnt;
    [SerializeField] private int activeEnemyCnt;

    [SerializeField] private GameObject gameWinUI;
    [SerializeField] private GameObject gameLoseUI;

    private Dictionary<string, int> collectDrop = new Dictionary<string, int>();
    #endregion
    
    // Events
    public event Action NextTurn;
    public event Action<int, int> TurnRotateScoot;
    public event Action<int, int> Slow;
    public event Action<int, int, int> Attack;

    void Awake()
    {
        if (Controller != null && Controller != this)
        {
            Destroy(gameObject);
            return;
        }
        Controller = this;
    }

    void Start()
    {
        playerFixedIDs = GameController.Instance.combatCard;

        GeneratePlayerCards(playerFixedIDs, 0);
        GenerateEnemyCards(enemyFixedIDs, activePlayerCnt);
        AssignTurnOrder();
    }

    void FixedUpdate()
    {
        if (combatEnd) return;
        AssignTurnOrder();
        TryNextTurn();
        CheckEnd();
    }

    void AssignTurnOrder()
    {
        var sorted = allCards.OrderBy(u => u.turnCountDown).ToList();

        for (int i = 0; i < sorted.Count; i++)
        {
            sorted[i].turnOrder = i;
        }
    }

    void GeneratePlayerCards(List<int> _fixedIDs, int _tempID)
    {
        int spawnedCnt = 0;

        foreach(int _fixedID in _fixedIDs)
        {
            if (spawnedCnt >= playerAnchors.Count) return;
            Transform _spawnPoint = playerAnchors[spawnedCnt];

            PlayerCard _newCard = Instantiate(playerPrefab, _spawnPoint.position, _spawnPoint.rotation).GetComponent<PlayerCard>();
            _newCard.AssignValues(playerPool[_fixedID]);
            _newCard.Initiate(_tempID);

            RegisterCard(_newCard, _tempID);
            allCards.Add(_newCard);
            _tempID += 1;
            spawnedCnt += 1;
        }
    }

    void GenerateEnemyCards(List<int> _fixedIDs, int _tempID)
    {
        int spawnedCnt = 0;

        foreach(int _fixedID in _fixedIDs)
        {
            if (spawnedCnt >= enemyAnchors.Count) return;
            Transform _spawnPoint = enemyAnchors[spawnedCnt];

            EnemyCard _newCard = Instantiate(enemyPrefab, _spawnPoint.position, _spawnPoint.rotation).GetComponent<EnemyCard>();
            _newCard.AssignValues(enemyPool[_fixedID]);
            _newCard.Initiate(_tempID);

            RegisterCard(_newCard, _tempID);
            allCards.Add(_newCard);
            _tempID += 1;
            spawnedCnt += 1;
        }
    }

    public void RegisterCard(GeneralCombatCard card, int i)
    {
        card.Initiate(i);
        switch(card.side)
        {
            case GameSide.Enemy:
                enemyIDs.Add(i);
                activeEnemyCnt += 1;
                break;
            case GameSide.Player:
                playerIDs.Add(i);
                activePlayerCnt += 1;
                break;
        }
    }

    public void TryNextTurn()
    {
        if (inTurn) return;
        NextTurn?.Invoke();
    }

    public void ScootCards(int rotatedID, int rotatedCountDown)
    {
        TurnRotateScoot.Invoke(rotatedID, rotatedCountDown);
    }

    public void KillCard(int cardID)
    {
        GeneralCombatCard card = allCards[cardID];

        switch(card.side)
        {
            case GameSide.Enemy:
                activeEnemyCnt -= 1;
                enemyIDs.Remove(cardID);
                break;
            case GameSide.Player:
                activePlayerCnt -= 1;
                playerIDs.Remove(cardID);
                deadPlayerCards.Add(card.fixedID);
                break;
        }
        card.enabled = false;
        card.gameObject.SetActive(false);
    }

    public void CollectDrop(string item, int amount)
    {
        if (collectDrop.ContainsKey(item))
        {
            collectDrop[item] += amount;
            collectDrop.Clear();
        }
    }

    public void SlowTarget(int _targetID, int _slowedCount)
    {
        Slow.Invoke(_targetID, _slowedCount);
    }

    public void InflictAttack(int _attackerID, int _targetID, int _damage)
    {
        Attack.Invoke(_attackerID, _targetID, _damage);
    }

    public void InflictAttackRandom(int _attackerID, List<int> _targetIDPool, int _damage)
    {
        var _targetID = -1;
        GeneralCombatCard attacker = allCards[_attackerID];
        _targetID = _targetIDPool[UnityEngine.Random.Range(0, _targetIDPool.Count)];
        Attack.Invoke(_attackerID, _targetID, _damage);
    }

#region End Combat
    private void CheckEnd()
    {
        if (activeEnemyCnt == 0)
        {
            EndCombatWin();
        }
        else if (activePlayerCnt == 0)
        {
            EndCombatLose();
        }
        else
        {
            return;
        }

        GameController.Instance.LoseCards(deadPlayerCards);
    }

    private void EndCombatWin()
    {
        combatEnd = true;
        gameWinUI.SetActive(true);
    }

    private void EndCombatLose()
    {
        combatEnd = true;
        gameLoseUI.SetActive(true);
    }

    public void Click(int i)
    {
        SceneManager.LoadScene(i);
    }

#endregion
}
