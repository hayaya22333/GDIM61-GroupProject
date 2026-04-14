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
    [SerializeField] private Card[] _cards;
    [SerializeField] private int gameLevel;
    public event Action FightEnd;

    private Dictionary<string, int> collectDrop = new Dictionary<string, int>();

    void Start()
    {
        
    }
    public void CollectDrop(string item, int amount)
    {
        if (collectDrop.ContainsKey(item))
        {
            collectDrop[item] += amount;
            collectDrop.Clear();
        }
    }

    public void TellPlayerHurt(int no)
    {
        
    }

}
