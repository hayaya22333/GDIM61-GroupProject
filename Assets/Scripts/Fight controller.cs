using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Fightcontroller : MonoBehaviour
{
    public static Fightcontroller Instance {get; private set;}

    void Awake()
    {
        Instance = this;
    }

    [SerializeField] private Enemy[] _enemies;
    [SerializeField] private CardInGame[] _cards;
    [SerializeField] private int gameLevel;

    public event Action<int, int> EnemyAttackPlayer;
    public event Action<int, int> PlayerAttackEnemy;
    public event Action<int> EnemyDie;
    public event Action<int> PlayerDie;
    public event Action FightEnd;

    public bool canCount;
    public bool fightEnd = false;

    private Dictionary<string, int> collectDrop = new Dictionary<string, int>();

    void Start()
    {
        
    }

    void Update()
    {
        if(fightEnd == true)
        {
            return;
        }
        WatchEnemy();
        WatchPlayer();
        CheckEnd();
    }
    public void CollectDrop(string item, int amount)
    {
        if (collectDrop.ContainsKey(item))
        {
            collectDrop[item] += amount;
            collectDrop.Clear();
        }
    }

    public void Die()
    {
        if(_enemies[0].die == true)
        {
            EnemyDie?.Invoke(0);  
        }
        else if(_enemies[1].die == true)
        {
            EnemyDie?.Invoke(1);
        }
    }

    public void WatchEnemy()
    {
        if (_enemies[0].Attack())
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

        if (_enemies[1].Attack())
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

    public void WatchPlayer()
    {
        if (_cards[0].Attack())
        {
            PlayerAttackEnemy?.Invoke(0, _cards[0].attack);
            canCount = false;
            if(1f - Time.deltaTime <= 0f)
            {
                canCount = true;
            }
            Debug.Log("0 attack");
        }

        if (_cards[0].die == true)
        {
            PlayerDie?.Invoke(0);
        }

        if (_cards[1].Attack())
        {
            PlayerAttackEnemy?.Invoke(1, _cards[1].attack);
            canCount = false;
            if(1f - Time.deltaTime <= 0f)
            {
                canCount = true;
            }
            Debug.Log("1 attack");
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
