using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private FightNode _fightNode;
    [SerializeField] private MeshRenderer _meshRenderer;
    private float life;
    private int attack;
    private int speed;
    public bool die;

    private float attackColorInterval;
    private float hurtColorInterval;
    [SerializeField] private float flashDuration = 0.05f;

    public event Action<int> EnemyAttack;
    public event Action<int> EnemyHurt;
    public event Action<int> EnemyDie;

    void Start()
    {
        life = _fightNode.enemyHP;
        attack = _fightNode.enemyATK;
        speed = _fightNode.enemySPD;
        EnemyAttack += HowEnemyAttack;

        die = false;
    }

    void Update()
    {
        CheckDeath();
    }

    void CheckDeath()
    {
        if(life <= 0)
        {
            die = true;
            EnemyDie?.Invoke(_fightNode.enemyName);
        }
    }

    public void Attack()
    {
        if(die == true)
        {
            return;
        }
        else
        {
            EnemyAttack?.Invoke(_fightNode.enemyName);
        }
    }

    public void HowEnemyAttack(int no)
    {
        _meshRenderer.material.color = Color.blue;
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

    public void HowEnemyHurt(int no)
    {
        _meshRenderer.material.color = Color.red;
        hurtColorInterval = flashDuration;
        Flash();
        Debug.Log($"{no} hurts");
    }

    private void Flash()
    {
        if(attackColorInterval > 0)
        {
            attackColorInterval -= Time.deltaTime;
            if(attackColorInterval <= 0f)
            {
                _meshRenderer.material.color = Color.white;
            }
        }
        
        if(hurtColorInterval > 0)
        {
            hurtColorInterval -= Time.deltaTime;
            if(hurtColorInterval <= 0f)
            {
                _meshRenderer.material.color = Color.white;
            }
        }
    }

}
