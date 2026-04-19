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
    [SerializeField] protected SpriteRenderer _spriteRenderer;
    [SerializeField] protected TextMeshPro turnText;
    [SerializeField] protected TextMeshPro hpText;
    [SerializeField] protected GameObject damageTextPrefab;
    [SerializeField] protected GameObject damagePopAnchor;

    [Header("Cards Stats")]
    public string side = "Neutral";
    public int spd;
    public int hp;
    public int id;
    public int atk;

    protected CombatController combatController;
    protected Vector3 startPos;

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
        hpText.text = hp.ToString();
    }

    public void Initiate(int x)
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

        GameObject dmgTxtObj = Instantiate(damageTextPrefab, damagePopAnchor.transform.position, damagePopAnchor.transform.rotation);
        DamageText dmgTxt = dmgTxtObj.GetComponent<DamageText>();
        dmgTxt.PopDamage(dmg);
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

    public virtual void StartTurn()
    {
        Debug.Log("It's card " + id + "'s turn.");
        _spriteRenderer.color = Color.green;
        startPos = transform.position;

        combatController.inTurn = true;
        onTurn = true;
        locked = false;
    }

    protected void EndTurn()
    {
        // Debug.Log("End of card " + id + "'s turn.");
        _spriteRenderer.color = Color.white;

        turnCountDown += spd + 1;
        combatController.ScootCards(id, turnCountDown);
        combatController.inTurn = false;
        onTurn = false;
        locked = true;
    }
}
