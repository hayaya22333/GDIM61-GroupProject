using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrepareController : MonoBehaviour
{
    public TMP_Text detail;

    public void Start()
    {
        detail.enabled = false;
    }
    public void Click(int i)
    {
        SceneManager.LoadScene(i);
    }

    public void Click()
    {
        detail.enabled = true;
    }

}
