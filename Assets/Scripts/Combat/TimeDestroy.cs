using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDestroy : MonoBehaviour
{
    public float aliveTime = 2.0f;

    void Start()
    {
        Destroy(gameObject, aliveTime);
    }
}
