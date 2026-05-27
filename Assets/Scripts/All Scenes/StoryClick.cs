using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryClick : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private AudioSource flip;
    void Awake()
    {
        button.onClick.AddListener(Click);
    }

    private void Click()
    {
        StoryManager.Instance.ClickStoryArea();
        flip.Play();
    }
}
