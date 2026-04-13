using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (fileName = "FightNode", menuName = "ScriptableObjects/FightNode", order = 1)]
public class FightNode : ScriptableObject
{
    [SerializeField] private string characterName;
    [SerializeField] private float _enemyHP;
    [SerializeField] private float _enemyATK;
    [SerializeField] private int _enemySPD;
}
