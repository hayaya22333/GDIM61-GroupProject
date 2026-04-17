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

    // TODO: change list item types
    [SerializeField] private List<GeneralCombatCard> _allCards;
    [SerializeField] private EnemyCard[] _enemies;
    [SerializeField] private CombatCard[] _cards;
    [SerializeField] private int gameLevel;

    public event Action<int, int> EnemyAttackPlayer;
    public event Action<int, int> PlayerAttackEnemy;
    public event Action<int> EnemyDie;
    public event Action<int> PlayerDie;
    public event Action FightEnd;
    public event Action NextTurn;

    public event Action<int> PlayerTurn;

    public bool canCount;
    public bool fightEnd = false;
    public bool inTurn = false;

    private Dictionary<string, int> collectDrop = new Dictionary<string, int>();

    void Start()
    {
        int startCountDown = 1;

        foreach (GeneralCombatCard card in _allCards)
        {
            card.SetCountDown(startCountDown);
            startCountDown += 1;
        }
        /*foreach (CombatCard card in _cards)
        {
            _allCards.Add(card);
            card.InitOrder(startCountDown);
            startCountDown += 1;
        }*/
    }

    void Update()
    {
        if(fightEnd == true)
        {
            return;
        }
        TryNextTurn();

        //WatchEnemy();
        //WatchPlayer();
        //CheckEnd();
    }

    public void TryNextTurn()
    {
        if (inTurn)
        {
            return;
        }
        NextTurn.Invoke();
    }

    public void ScootCards(int skipID, int insertedCountDown)
    {
        foreach (GeneralCombatCard card in _allCards)
        {
            if (card.id != skipID)
            {
                card.Scoot(insertedCountDown);
            }
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

    public void Die(int ID)
    {
        EnemyDie?.Invoke(ID);
    }

    public void WatchEnemy()
    {
        if (inTurn)
        {
            return;
        }

        if (_enemies[0].CanAttack())
        {
            EnemyAttackPlayer?.Invoke(0, _enemies[0].attack);
            canCount = false;
            if(1f - Time.deltaTime <= 0f)
            {
                canCount = true;
            }
            Debug.Log("0 attack");
        }

        if (_enemies[0].CheckDeath())
        {
            EnemyDie?.Invoke(0);
            Debug.Log("0 die");
        }

        if (_enemies[1].CanAttack())
        {
            EnemyAttackPlayer?.Invoke(1, _enemies[1].attack);
            canCount = false;
            if(1f - Time.deltaTime <= 0f)
            {
                canCount = true;
            }
            Debug.Log("1 attack");
        }

        if (_enemies[1].CheckDeath())
        {
            EnemyDie?.Invoke(1);
            Debug.Log("1 die");
        }
    }

    public void Attack(int attackerID, int targetID, int damage)
    {
        foreach (GeneralCombatCard _card in _allCards)
        {
            if (_card.id == targetID)
            {
                _card.TakeDamage(damage);
            }
        }
        EndTurn();
    }

    private void EndTurn()
    {
        Debug.Log("End of turn.");
        inTurn = false;
    }

    public void PlayerAttack(int targetID, int damage)
    {
        PlayerAttackEnemy?.Invoke(targetID, damage);
        inTurn = false;
        Debug.Log("Player dealt " + damage + " damage to enemy " + targetID);
    }

    public void WatchPlayer()
    {
        if (_cards[0].CanAttack())
        {
            if (inTurn)
            {
                return;
            }
            inTurn = true;
            PlayerTurn.Invoke(0);

            canCount = false;
            if(1f - Time.deltaTime <= 0f)
            {
                canCount = true;
            }
        }

        if (_cards[0].die == true)
        {
            PlayerDie?.Invoke(0);
        }

        if (_cards[1].CanAttack())
        {
            if (inTurn)
            {
                return;
            }
            inTurn = true;
            PlayerTurn.Invoke(1);

            canCount = false;
            if(1f - Time.deltaTime <= 0f)
            {
                canCount = true;
            }
        }
        if (_cards[1].die == true)
        {
            PlayerDie?.Invoke(1);
        }
    }

    public void CheckEnd()
    {
        if((_enemies[0].die && _enemies[1].die) || (_cards[0].die && _cards[1].die))
        {
            FightEnd?.Invoke();
            fightEnd = true;
        }
    }

}
