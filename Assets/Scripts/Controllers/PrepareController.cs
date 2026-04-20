using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrepareController : MonoBehaviour
{
    public void Click(int i)
    {
        SceneManager.LoadScene(i);
    }
}
