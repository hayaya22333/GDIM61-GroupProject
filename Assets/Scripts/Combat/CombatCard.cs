using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GeneralCombatCard : DragObject
{
    [Header("Game Status")]
    public int turnCountDown;
    public bool onTurn = false;

    [Header("Components")]
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private TextMeshPro turnText; 

    [Header("Cards Stats")]
    public string side = "Neutral";
    public int spd;
    public int hp;
    public int id;
    public int atk;

    private CombatController combatController;
    private Vector3 startPos;

    void Start()
    {
        combatController = CombatLocator.Instance.Controller;
        combatController.NextTurn += HandleNextTurn;
        combatController.Attack += HandleAttack;
        combatController.TurnScoot += HandleTurnScoot;

        locked = true;
    }

    void Update()
    {
        if (hp <= 0) combatController.KillCard(id);
        turnText.text = turnCountDown.ToString();
    }

    public void SetCountDown(int x)
    {
        turnCountDown = x + 1;
        id = x;
    }

    public void HandleTurnScoot(int skipID, int insertedCountDown)
    {
        if (skipID == id) return;

        if (turnCountDown >= insertedCountDown)
        {
            Debug.Log("card " + id + " Scooted down.");
            turnCountDown += 1;
        }
    }

    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        Debug.Log("Card " + id + " took " + dmg + "damage!!!");
    }

    void HandleAttack(int attackerID, int targetID, int damage)
    {
        if (targetID == id)
        {
            TakeDamage(damage);
        }
    }

    void HandleNextTurn()
    {
        if (hp <= 0) return;

        turnCountDown -= 1;
        if (turnCountDown == 0)
        {
            StartTurn();
        }
        else if (turnCountDown < 0)
        {
            Debug.Log("ERROR: Negative count down [" + turnCountDown + "] on card " + id);
        }
    }

    private void StartTurn()
    {
        Debug.Log("It's card " + id + "'s turn.");
        _spriteRenderer.color = Color.green;
        startPos = transform.position;

        combatController.inTurn = true;
        onTurn = true;
        locked = false;
    }

    private void EndTurn()
    {
        Debug.Log("End of card " + id + "'s turn.");
        _spriteRenderer.color = Color.white;

        turnCountDown += spd + 1;
        combatController.ScootCards(id, turnCountDown);
        combatController.inTurn = false;
        onTurn = false;
        locked = true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (isDragging || !onTurn) return;

        if (other.TryGetComponent<GeneralCombatCard>(out GeneralCombatCard target))
        {
            if (target.side == side) return;
            combatController.InflictAttack(id, target.id, atk);
            transform.position = startPos;
            onTurn = false;
            EndTurn();
        }
    }
}
