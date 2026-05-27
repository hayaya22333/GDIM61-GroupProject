using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class PollController : MonoBehaviour
{
    public GameObject ufo;
    public static PollController pollController {get; private set;}

    public List<GameObject> cardPoll = new List<GameObject>();
    [SerializeField] private GameObject introstory;
    [SerializeField] private GameObject instructionContent;
    [SerializeField] private AudioSource buttonAudio;
    [SerializeField] private AudioClip buttonclip;
    public event Action<AudioSource, AudioClip> PlayAudioPleaseP;

    private bool inStory = false;

    public void Click(int i)
    {
        //click.Play();
        AudioManger.Instance.PlayClick(buttonAudio,buttonclip);
        PlayAudioPleaseP?.Invoke(buttonAudio, buttonclip);
        Debug.Log("Audio Played");
        SceneManager.LoadScene(i);
        
    }

    [SerializeField] private GameObject button;
    
    void Start()
    {
        button.SetActive(false);
        instructionContent.SetActive(false);
    }
    public void clickInstruction()
    {
        instructionContent.SetActive(true);
        //Time.timeScale = 0f;
        //click.Play();
        AudioManger.Instance.PlayClick(buttonAudio,buttonclip);
    }

    public void clickReturn()
    {
        instructionContent.SetActive(false);
        //Time.timeScale = 0f;
        //click.Play();
        AudioManger.Instance.PlayClick(buttonAudio,buttonclip);
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
