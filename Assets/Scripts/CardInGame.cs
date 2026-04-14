using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInGame : DragObject
{
    [SerializeField] private CardNode _cardNode;
    [SerializeField] private Fightcontroller _fightcontroller;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private float life;
    public int attack;
    public int speed;
    public bool die;
    public int ID;

    public float av;

    private float attackColorInterval;
    private float hurtColorInterval;
    [SerializeField] private float flashDuration = 0.05f;

    void Start()
    {
        life = _cardNode._cardHP;
        attack = _cardNode._cardATK;
        speed = _cardNode._cardSPD;
        ID = _cardNode._cardName;

        _fightcontroller.PlayerAttackEnemy += HowPlayerAttack;
        _fightcontroller.EnemyAttackPlayer += HowPlayerHurt;
        _fightcontroller.PlayerDie += dieing;

        die = false;
        av = 100/speed;
    }

    void Update()
    {
        CheckDeath();
    }

    public bool Attack()
    {
        if(die == true)
        {
            return false;
        }

        return false;
    }

    public void avIncrease()
    {
        av += 100/speed;
    }
    public void HowPlayerAttack(int no, int harm)
    {
        _spriteRenderer.color = Color.blue;
        attackColorInterval = flashDuration;
        Flash();
        Debug.Log($"{no} attacks");
    }

    public void HowPlayerHurt(int no, int harm)
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

    public void dieing(int no)
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
