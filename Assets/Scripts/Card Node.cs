using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "CardNode", menuName = "ScriptableObjects/CardNode", order = 2)]
public class CardNode : ScriptableObject
{
    [SerializeField] private string _cardName;
    [SerializeField] private float _cardHP;
    [SerializeField] private float _cardATK;
    [SerializeField] private int _cardSPD;
}
