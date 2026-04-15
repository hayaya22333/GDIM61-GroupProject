using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActionCard : ClickObject
{
    public override void HandleClicked()
    {
        Debug.Log("Casted " + gameObject.name);
    }
}
