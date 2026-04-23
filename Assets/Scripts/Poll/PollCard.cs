using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollCard : MonoBehaviour
{
    public PollController _pollController;
    public int cardID;

    public void OnMouseDown()
    {
        Debug.Log("Get card");
        gameObject.SetActive(false);
    }
}
