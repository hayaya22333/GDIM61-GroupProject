using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PollController : MonoBehaviour
{
    public static PollController pollController {get; private set;}

    public List<GameObject> cardPoll = new List<GameObject>();

    public void Click(int i)
    {
        SceneManager.LoadScene(i);
    }
    [SerializeField] private GameObject button;
    void Start()
    {
        button.SetActive(true);
    }
    void Update()
    {
        if (cardPoll.Count == 0)
        {
            button.SetActive(true);
        }
    }


}
