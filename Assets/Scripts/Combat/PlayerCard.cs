using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCard : DragObject
{
    [SerializeField] private CardNode _cardNode;
    private CombatController combatController;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [SerializeField] private float life;
    public int attack;
    public int speed;
    public int turnCountDown;
    public bool die;
    public bool onTurn = false;
    public int ID;

    public float av;

    private float attackColorInterval;
    private float hurtColorInterval;
    [SerializeField] private float flashDuration = 0.001f;

    private Vector3 startPos;

    void Awake()
    {
        life = _cardNode._cardHP;
        attack = _cardNode._cardATK;
        speed = _cardNode._cardSPD;
        ID = _cardNode._cardName;

        die = false;
        av = 100/speed;
        startPos = transform.position;
    }

    void Start()
    {
        combatController = CombatLocator.Instance.Controller;

        combatController.PlayerAttackEnemy += HandlePlayerAttack;
        combatController.EnemyAttackPlayer += HandlePlayerHurt;
        combatController.PlayerDie += HandlePlayerDie;
        combatController.PlayerTurn += HandlePlayerTurn;
        combatController.NextTurn += HandleNextTurn;

        locked = true;
    }

    void Update()
    {
        CheckDeath();
    }

    public void InitOrder(int order)
    {
        turnCountDown = order;
    }

    public bool PlayerOnTurn()
    {
        return onTurn;
    }

    public bool CanAttack()
    {
        if(die == true)
        {
            return false;
        }

        return true;
    }

    public void avIncrease()
    {
        av += 100/speed;
    }

    public void HandleNextTurn()
    {
        if (turnCountDown == 0)
        {
            HandlePlayerTurn(ID);
            return;
        }
        if (turnCountDown < 0)
        {
            Debug.Log("ERROR: Negative count down: " + turnCountDown);
            return;
        }
        turnCountDown -= 1;
    }

    public void Scoot(int insertedCountDown)
    {
        if (turnCountDown >= insertedCountDown)
        {
            turnCountDown += 1;
        }
    }


    public int GetCountDown()
    {
        return turnCountDown;
    }

    public void HandlePlayerTurn(int activeID)
    {
        combatController.inTurn = true;
        if (activeID != ID)
        {
            return;
        }
        Debug.Log("It's player card " + ID + "'s turn.");

        _spriteRenderer.color = Color.green;
        onTurn = true;
        locked = false;
        // End turn: uses OnTriggerStay2D() to call PlayerAttack()
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isDragging)
        {
            if (other.CompareTag("Enemy"))
            {
                // using 0 for now.
                combatController.PlayerAttack(0, attack);
                turnCountDown += speed;
                combatController.ScootCards(ID, turnCountDown);

                transform.position = startPos;
                locked = true;
            }
        }
    }

    public void HandlePlayerAttack(int no, int harm)
    {
        _spriteRenderer.color = Color.blue;
        attackColorInterval = flashDuration;
        Flash();
        Debug.Log($"{no} attacks");
    }

    public void HandlePlayerHurt(int no, int harm)
    {
        if(no != ID && die == false)
        {
            return;
        }
        else
        {
            life -= harm;
            _spriteRenderer.color = Color.red;
            hurtColorInterval = flashDuration;
            Flash();
            Debug.Log($"{no} hurts");
        }

        if(life <= 0)
        {
            die = true;
        }
    }

    public void HandlePlayerDie(int no)
    {
        if(no != ID)
        {
            return;
        }

        if(die == true)
        {
            gameObject.SetActive(false);
        }
    }

    public bool CheckDeath()
    {
        if(life <= 0)
        {
            die = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Flash()
    {
        if(attackColorInterval > 0)
        {
            attackColorInterval -= Time.deltaTime;
            if(attackColorInterval <= 0f)
            {
                _spriteRenderer.color = Color.white;
            }
        }
        
        if(hurtColorInterval > 0)
        {
            hurtColorInterval -= Time.deltaTime;
            if(hurtColorInterval <= 0f)
            {
                _spriteRenderer.color = Color.white;
            }
        }
    }
}
