using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ClickObject : MonoBehaviour
{
    private bool mouseDowned;

    private void OnMouseDown()
    {
        mouseDowned = true;
    }

    private void OnMouseExit()
    {
        mouseDowned = false;
    }

    private void OnMouseUp()
    {
        if (mouseDowned)
        {
            HandleClicked();
        }
    }

    public abstract void HandleClicked();
}
