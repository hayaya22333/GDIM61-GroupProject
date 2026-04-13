using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (fileName = "FightNode", menuName = "ScriptableObjects/FightNode", order = 1)]
public class FightNode : ScriptableObject
{
    [SerializeField] private bool enemy;
    [SerializeField] private bool player;
    [SerializeField] private string characterName;
    [SerializeField] private float _HP;
    [SerializeField] private float _ATK;
    [SerializeField] private int _SPD;
}
