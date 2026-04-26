using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCard : MonoBehaviour
{
    [SerializeField] private CardNode card;
    public int GetID()
    {
        return card.cardID;
    }
}
