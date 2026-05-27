using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartController : MonoBehaviour
{
    public static StartController Instance {get; private set;}
    [SerializeField] private AudioSource button;
    [SerializeField] private AudioClip buttonclip;
    public event Action<AudioSource, AudioClip> PlayAudioPlease;
    public void Click()
    {
        AudioManger.Instance.PlayClick(button, buttonclip);
        PlayAudioPlease?.Invoke(button, buttonclip);
        SceneManager.LoadScene(1);
        //button.Play();
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
}
