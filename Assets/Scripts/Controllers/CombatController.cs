using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.SceneManagement;


[System.Serializable]
public class IntList
{
    public List<int> values = new List<int>();
}

public class CombatController : MonoBehaviour
{
#region Variables
    public static CombatController Controller { get; private set; }
    [SerializeField] List<GameObject> stageBackgrounds = new List<GameObject>();
    [SerializeField] SpriteRenderer background;

    [Header("Stage Info")]
    public List<IntList> enemySetups = new List<IntList>();
    public List<int> playerFixedIDs;
    public List<int> enemyFixedIDs;
    [SerializeField] List<int> deadPlayerCards = new List<int>();  

    [Header("Card Storage")]
    public GameObject enemyPrefab;
    public List<FightNode> enemyPool;
    public GameObject playerPrefab;
    public List<CardNode> playerPool;

    [Header("Active Components")]
    [SerializeField] private List<GeneralCombatCard> allCards;
    public List<int> enemyIDs;
    public List<int> playerIDs;

    [Header("Anchors")]
    public List<Transform> playerAnchors;
    public List<Transform> enemyAnchors;
    public Transform actionCardSpawn;

    [Header("Game Status")]
    [SerializeField] private int gameLevel;
    public bool combatEnd = false;
    public bool inTurn = false;
    [SerializeField] private int activePlayerCnt;
    [SerializeField] private int activeEnemyCnt;

    [SerializeField] private GameObject gameWinUI;
    [SerializeField] private GameObject gameLoseUI;

    // private Dictionary<string, int> collectDrop = new Dictionary<string, int>();
#endregion

#region Events
    // Events
    public event Action NextTurn;
    public event Action<int, int> TurnRotateScoot;
    public event Action<int, int> Slow;
    public event Action<int, int, int> Attack;
#endregion

    // Temporary var
    public bool playerMoved = false;

    [Header("Tina added")]
    [SerializeField] private GameObject instructionContent;

#region Start and Awake
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
        Instantiate(stageBackgrounds[GameController.Instance.combatIndex]);
        enemyFixedIDs = enemySetups[GameController.Instance.combatIndex].values;
        playerFixedIDs = GameController.Instance.combatCardStore;
        
        GeneratePlayerCards(playerFixedIDs, 0);
        GenerateEnemyCards(enemyFixedIDs, activePlayerCnt);
        AssignTurnOrder();

        foreach (int id in playerFixedIDs)
        {
            Debug.Log("player card - " + id);
        }
        //Tina
        instructionContent.SetActive(false);
    }
#endregion
    
    void FixedUpdate()
    {
        if (combatEnd) return;
        AssignTurnOrder();
        TryNextTurn();
        CheckEnd();
    }

#region Generate Cards
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
#endregion

#region Turns
    public void TryNextTurn()
    {
        if (inTurn) return;
        NextTurn?.Invoke();
    }

    public void ScootCards(int rotatedID, int rotatedCountDown)
    {
        TurnRotateScoot.Invoke(rotatedID, rotatedCountDown);
    }
#endregion

#region Affecting Cards
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

    public void KillCard(int cardID)
    {
        GeneralCombatCard card = allCards[cardID];

        switch(card.side)
        {
            case GameSide.Enemy:
                activeEnemyCnt -= 1;
                enemyIDs.Remove(cardID);
                GameController.Instance.victimCnt += 1;
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
#endregion

#region End Combat
    private void CheckEnd()
    {
        if (activeEnemyCnt == 0)
        {
            EndCombatWin();
            GameController.Instance.ClearAllCombatCard();
            GameController.Instance.LoseCards(deadPlayerCards);
        }
        else if (activePlayerCnt == 0)
        {
            EndCombatLose();
        }
        else
        {
            return;
        }
    }

    private void EndCombatWin()
    {
        combatEnd = true;
        if (GameController.Instance.combatIndex >= 2)
        {
            SceneManager.LoadScene(6);
        }
        else
        {
            gameWinUI.SetActive(true);
            GameController.Instance.combatIndex += 1;
        }
    }

    private void EndCombatLose()
    {
        combatEnd = true;
        // gameLoseUI.SetActive(true);
        SceneManager.LoadScene(5);
    }

    public void Click()
    {
        //Tina
        if (GameController.Instance.gameProcess == GameController.GameProcess.Level1)
        {
            SceneManager.LoadScene(7);
        }
        else if(GameController.Instance.gameProcess == GameController.GameProcess.Level2)
        {
            SceneManager.LoadScene(8);
        }
    }

#endregion


# region UI-Tina
    public void clickInstruction()
    {
        instructionContent.SetActive(true);
        Time.timeScale = 0f;
    }

    public void clickReturn()
    {
        instructionContent.SetActive(false);
        Time.timeScale = 0f;
    }
# endregion
}
