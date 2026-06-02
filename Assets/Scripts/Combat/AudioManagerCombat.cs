using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerCombat : MonoBehaviour
{
    [SerializeField] private AudioSource heal;
    [SerializeField] private AudioSource hurt;
    [SerializeField] private AudioSource accelerate;
    [SerializeField] private AudioSource slow;

    public AudioSource combatBGM;
    public AudioSource bossBGM;

    void Start()
    {
        CombatController.Controller.Attack += HandleAttack;
        CombatController.Controller.Slow += HandleSlow;

        if (GameController.Instance.combatIndex < 2)
        {
            combatBGM.Play();
        }
        else {
            bossBGM.Play();
        }
    }

    void HandleAttack(int i, int j, int _damage)
    {
        if (_damage >= 0)
        {
            hurt.Play();
        }
        else
        {
            heal.Play();
        }
    }

    void HandleSlow(int i, int _count)
    {
        if (_count >= 0)
        {
            slow.Play();
        }
        else
        {
            accelerate.Play();
        }
    }
}
