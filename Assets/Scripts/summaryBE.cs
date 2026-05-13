using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class summaryBE : MonoBehaviour
{
    public TextMeshProUGUI line1;
    public TextMeshProUGUI line2;

    void Start()
    {
        line1.text = "You killed <size=150%>" + GameController.Instance.victimCnt + "</size> innocent citizens";
        line2.text = "Accepted <size=150%>" + GameController.Instance.totalOwnCard.Count + "</size> dangerous gifts from outer space";
    }
}
