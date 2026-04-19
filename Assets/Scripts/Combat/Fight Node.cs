using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu (fileName = "FightNode", menuName = "ScriptableObjects/FightNode", order = 1)]
public class FightNode : ScriptableObject
{
    public int enemyName;
    public int enemyHP;
    public int enemyATK;
    public int enemySPD;

    public EnemySkill enemySkill;
    public List<Drop> drops = new List<Drop>();
}

public enum EnemySkill
{
    AttackOne,
    AttackAll
}

[System.Serializable]
public class Drop
{
    public string dropName;
    public float dropChance;
}