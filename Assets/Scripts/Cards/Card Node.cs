using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "CardNode", menuName = "ScriptableObjects/CardNode", order = 2)]
public class CardNode : ScriptableObject
{
    public int _cardName;
    public int _cardHP;
    public int _cardATK;
    public int _cardSPD;
}
