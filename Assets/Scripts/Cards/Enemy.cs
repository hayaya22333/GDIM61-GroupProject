using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    [SerializeField] private FightNode _fightNode;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private CombatController combatController;

    [SerializeField] private float life;
    public int attack;
    public int speed;
    public bool die;
    public int ID;

    public float av;

    private float attackColorInterval;
    private float hurtColorInterval;
    [SerializeField] private float flashDuration = 0.05f;

    void Awake()
    {
        life = _fightNode.enemyHP;
        attack = _fightNode.enemyATK;
        speed = _fightNode.enemySPD;
        ID = _fightNode.enemyName;

        av = 100/speed;
        die = false;
    }
    void Start()
    {
        combatController = Locator_Combat.Instance.CmbtController;
        combatController.EnemyAttackPlayer += HowEnemyAttack;
        combatController.PlayerAttackEnemy += HowEnemyHurt;
        combatController.EnemyDie += dying;
    }

    void Update()
    {
        if (CheckDeath())
        {
            combatController.Die(ID);
        }
    }

    public bool CheckDeath()
    {
        if(life <= 0)
        {
            die = true;
        }
            return die;
    }

    public void dying(int no)
    {
        if(no != ID)
        {
            return;
        }
        else
        {
            gameObject.SetActive(false);
            Debug.Log($"enemy{no} die");
        }
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

    public void HowEnemyAttack(int no, int harm)
    {
        if(no != ID)
        {
            return;
        }

        _spriteRenderer.color = Color.blue;
        attackColorInterval = flashDuration;
        Flash();
        Debug.Log($"{no} attacks");
    }
    
    public void Hurt(int harm)
    {
        if(die == true)
        {
            return;
        }
        life -= harm;
    }

    public void HowEnemyHurt(int no, int harm)
    {
        if(no != ID)
        {
            return;
        }

        _spriteRenderer.color = Color.red;
        hurtColorInterval = flashDuration;
        Flash();
        Debug.Log($"{no} hurts");

        Hurt(harm);
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
