using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pollHint : MonoBehaviour
{
    private SpriteRenderer sr;

    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        sr.enabled = false;
    }

    void Update()
    {
        if (GameController.Instance.ownCard.Count >= 3)
        {
            sr.enabled = true;
        }
    }
}
