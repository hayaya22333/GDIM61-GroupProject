using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class anchor : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }
}
