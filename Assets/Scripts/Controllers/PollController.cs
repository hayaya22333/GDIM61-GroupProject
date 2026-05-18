using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PollController : MonoBehaviour
{
    public GameObject ufo;
    public static PollController pollController {get; private set;}

    public List<GameObject> cardPoll = new List<GameObject>();
    [SerializeField] private GameObject introstory;

    private bool inStory = false;

    public void Click(int i)
    {
        SceneManager.LoadScene(i);
    }

    [SerializeField] private GameObject button;
    
    void Start()
    {
        button.SetActive(false);
    }


    void Update()
    {
        if(introstory != null)
        {
            inStory = true;
            if(StoryManager.Instance.storyFinished == true)
            {
                introstory.SetActive(false);
                inStory = false;
                button.SetActive(true);
                Destroy(introstory);
            }
        }
        if (!ufo.activeSelf && !inStory)
        {
            button.SetActive(true);
        }
    }


}
