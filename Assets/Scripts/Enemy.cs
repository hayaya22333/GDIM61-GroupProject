using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private FightNode _fightNode;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private float life;
    public int attack;
    public int speed;
    public bool die;
    public int ID;
    private float interval = 0.01f;

    private float attackColorInterval;
    private float hurtColorInterval;
    [SerializeField] private float flashDuration = 0.05f;


    void Start()
    {
        life = _fightNode.enemyHP;
        attack = _fightNode.enemyATK;
        speed = _fightNode.enemySPD;
        ID = _fightNode.enemyName;

        Fightcontroller.Instance.EnemyAttackPlayer += HowEnemyAttack;
        Fightcontroller.Instance.PlayerAttackEnemy += HowEnemyHurt;
        Fightcontroller.Instance.EnemyDie += dying;
        die = false;
    }

    void Update()
    {
        CheckDeath();
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
    
    public int av;
    private float attackTime;
    public bool Attack()
    {
        if(die == true)
        {
            return false;
        }
        av = 10000/speed;
        for(int i = av; i <= 0; i --)
        {
            if(Fightcontroller.Instance.canCount == true)
            {
                attackTime -= Time.deltaTime;
                if(attackTime <= 0f)
                {
                    //HowEnemyAttack(ID);
                    attackTime = interval;
                    i -= 1;
                    return true;
                }
            }
        }
        return false;
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
