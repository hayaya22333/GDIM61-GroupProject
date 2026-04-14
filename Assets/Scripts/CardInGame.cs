using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInGame : MonoBehaviour
{
    [SerializeField] private CardNode _cardNode;
    private float life;
    public int attack;
    //public int speed;
    public bool die;
    public int ID;

    private float interval = 0.01f;

    void Start()
    {
        life = _cardNode._cardHP;
        attack = _cardNode._cardATK;
        //speed = _cardNode._cardSPD;
        ID = _cardNode._cardName;

        Fightcontroller.Instance.PlayerAttackEnemy += HowPlayerAttack;
        Fightcontroller.Instance.EnemyAttackPlayer += HowPlayerHurt;
        Fightcontroller.Instance.PlayerDie += dieing;
        die = false;
    }

    private int av;
    private float attackTime;
    public bool Attack()
    {
        if(die == true)
        {
            return false;
        }
        //av = 10000/speed;
        //for(int i = av; i <= 0; i --)
        {
            if(Fightcontroller.Instance.canCount == true)
            {
                attackTime -= Time.deltaTime;
                if(attackTime <= 0f)
                {
                    attackTime = interval;
                    //i -= 1;
                    return true;
                }
            }
        }
        return false;
    }
    public void HowPlayerAttack(int no, int harm)
    {
        
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
}
