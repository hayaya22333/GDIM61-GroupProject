using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ClickObject : MonoBehaviour
{
    private void OnMouseDown()
    {
        HandleClicked();
    }

    public abstract void HandleClicked();
}
