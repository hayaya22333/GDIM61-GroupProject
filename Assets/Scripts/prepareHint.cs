using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prepareHint : MonoBehaviour
{
    [SerializeField] private SpriteRenderer point;
    [SerializeField] private SpriteRenderer click;

    void Start()
    {
        point.enabled = true;
        click.enabled = false;
    }

    void Update()
    {
        if (GameController.Instance.combatCard.Count >= 1)
        {
            point.enabled = false;
        }

        if (GameController.Instance.combatCard.Count >= 3)
        {
            click.enabled = true;
        }
        else
        {
            click.enabled = false;
        }
    }
}
