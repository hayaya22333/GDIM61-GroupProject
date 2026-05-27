using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartController : MonoBehaviour
{
    [SerializeField] private AudioSource button;
    public void Click()
    {
        SceneManager.LoadScene(1);
    }
}
