using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum GameSide
{
    Neutral,
    Player,
    Enemy
}

public class GeneralCombatCard : MonoBehaviour
{
    [Header("Game Status")]
    public int turnCountDown = 10000;
    public bool onTurn = false;

    [Header("Components")]
    [SerializeField] protected SpriteRenderer _spriteRenderer;
    [SerializeField] protected TextMeshPro turnText;
    [SerializeField] protected TextMeshPro hpText;
    [SerializeField] protected GameObject damageTextPrefab;
    [SerializeField] protected GameObject damagePopAnchor;

    [Header("Card Attributes")]
    public GameSide side = GameSide.Neutral;
    public int spd;
    public int hp;
    public int id;
    public int atk;

    protected CombatController combatController;

    void Start()
    {
        combatController = CombatLocator.Instance.Controller;
        combatController.NextTurn += HandleNextTurn;
        combatController.Attack += HandleAttack;
        combatController.TurnRotateScoot += HandleTurnScoot;
    }

    void FixedUpdate()
    {
        if (hp <= 0)
        {
            combatController.KillCard(id);
        }

        if (turnCountDown <= 0)
        {
            turnText.text = "0";
        }
        else
        {
            turnText.text = (turnCountDown/100).ToString();
        }
        hpText.text = hp.ToString();
    }

    public void Initiate(int x)
    {
        id = x;
    }

    void HandleNextTurn()
    {
        if (hp <= 0) return;

        turnCountDown -= spd;
        if (turnCountDown <= 0)
        {
            // Compare countdown with other cards on turn
            StartTurn();
        }
    }

    public void HandleTurnScoot(int skipID, int insertedCountDown)
    {
        if (skipID == id) return;

        if (turnCountDown >= insertedCountDown)
        {
            turnCountDown += 1;
        }
    }

    public virtual void StartTurn()
    {
        Debug.Log("It's card " + id + "'s turn.");
        _spriteRenderer.color = Color.green;

        combatController.inTurn = true;
        onTurn = true;
    }

    public virtual void EndTurn()
    {
        _spriteRenderer.color = Color.white;
        turnCountDown += 10000;
        combatController.ScootCards(id, turnCountDown);
        combatController.inTurn = false;
        onTurn = false;
    }

    public void TakeDamage(int dmg)
    {
        if (hp <= 0) return;
        hp -= dmg;
        //Debug.Log("Card " + id + " took " + dmg + "damage!!!");

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
}
