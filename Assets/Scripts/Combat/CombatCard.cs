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
    public int turnOrder = -1;
    public bool onTurn = false;
    public bool prepared = false;

    [Header("Components")]
    [SerializeField] protected SpriteRenderer _spriteRenderer;
    [SerializeField] protected TextMeshPro turnText;
    [SerializeField] protected TextMeshPro hpText;
    [SerializeField] protected GameObject damageTextPrefab;
    [SerializeField] protected GameObject damagePopAnchor;
    [SerializeField] protected GameObject onTurnCue;
    // Visual effects
    [SerializeField] private GameObject healVFX;
    [SerializeField] private GameObject damageVFX;
    [SerializeField] private GameObject slowVFX;

    [Header("Card Attributes")]
    public GameSide side = GameSide.Neutral;
    public int spd;
    public int hp;
    public int id;
    public int atk;
    public int fixedID = -1;
    protected CombatController combatController;

    void Start()
    {
        combatController = CombatController.Controller;
        combatController.NextTurn += HandleNextTurn;
        combatController.Attack += HandleAttack;
        combatController.Slow += HandleSlow;
        combatController.TurnRotateScoot += HandleTurnScoot;

        onTurnCue.SetActive(false);
    }

    void FixedUpdate()
    {
        if (!prepared) return;
        
        if (hp <= 0)
        {
            combatController.KillCard(id);
        }

        if (turnCountDown <= 0)
        {
            turnText.text = "GO";
        }
        else
        {
            turnText.text = turnOrder.ToString();
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
        //_spriteRenderer.color = Color.green;
        onTurnCue.SetActive(true);

        combatController.inTurn = true;
        onTurn = true;
    }

    public virtual void EndTurn()
    {
        //_spriteRenderer.color = Color.white;
        onTurnCue.SetActive(true);

        turnCountDown += 10000;
        combatController.ScootCards(id, turnCountDown);
        combatController.inTurn = false;
        onTurn = false;
    }

    public void TakeDamage(int dmg)
    {
        if (hp <= 0) return;
        hp -= dmg;

        GameObject dmgTxtObj = Instantiate(damageTextPrefab, damagePopAnchor.transform.position, damagePopAnchor.transform.rotation);
        DamageText dmgTxt = dmgTxtObj.GetComponent<DamageText>();

        if (dmg > 0)
        {
            Instantiate(damageVFX, transform.position, transform.rotation);
        }
        else if (dmg < 0)
        {
            Instantiate(healVFX, transform.position, transform.rotation);
        }

        dmgTxt.PopDamage(dmg);
    }

    void HandleAttack(int attackerID, int targetID, int damage)
    {
        if (targetID == id)
        {
            TakeDamage(damage);
        }
    }

    void HandleSlow(int _targetID, int _slowedCount)
    {
        if (_targetID == id)
        {
            turnCountDown += _slowedCount * 100;

            Instantiate(slowVFX, transform.position, transform.rotation);
        }
    }
}
