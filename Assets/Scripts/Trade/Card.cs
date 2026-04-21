using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : DragObject
{
    [SerializeField] private CardNode _cardNode;

    public List<GameObject> actions;

    private int hp;
    private int atk;
    private int spd;

    public delegate void IntDelegate(int x);
    public static event IntDelegate CardSold;

    private bool onTurn = false;
    private List<GameObject> activeActions = new List<GameObject>();

    void Start()
    {
        hp = _cardNode.cardHP;
        spd = _cardNode.cardSPD;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isDragging)
        {
            if (other.CompareTag("Merchant"))
            {
                Destroy(gameObject);
                Debug.Log("sold");
                CardSold.Invoke(20);
            }

            if ((other.CompareTag("CastField")) && (!onTurn))
            {
                transform.position = other.transform.position;
                Turn();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("CastField"))
        {
            OffTurn();
        }
    }

    public void Turn()
    {
        onTurn = true;
        Debug.Log("It's " + gameObject.name + "'s turn.");
        Debug.Log("Click on action card to cast.");

        foreach (GameObject action in actions)
        {
            GameObject newAction = Instantiate(action);
            activeActions.Add(newAction);
        }
    }

    public void OffTurn()
    {
        onTurn = false;
        foreach (GameObject activeAction in activeActions)
        {
            Destroy(activeAction);
        }
    }

}
