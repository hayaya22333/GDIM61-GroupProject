using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class story23Controller : MonoBehaviour
{
    [SerializeField] private List<Sprite> story = new List<Sprite>();
    //[SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject clickArea;
    [SerializeField] private Image image;
    [SerializeField] private AudioSource flip;

    private int imageIndex = 0;
    private bool storyFinish = false;
    void Start()
    {
        GameController.Instance.gameProcess = GameController.GameProcess.Level3;
        //continueButton.SetActive(false);
        clickArea.SetActive(true);
        imageIndex = 0;
        storyFinish = false;
        image.sprite = story[imageIndex];
    }

    public void NextSprite()
    {
        flip.Play();
        if (storyFinish == true)
        {
            return;
        }

        imageIndex += 1;

        if (imageIndex < story.Count)
        {
            image.sprite = story[imageIndex];
            Debug.Log(imageIndex);
        }
        else
        {
            NextScene();
        }
    }

    public void StoryEnd()
    {
        storyFinish = true;
        clickArea.SetActive(false);
        //continueButton.SetActive(true);
    }

    public void NextScene()
    {
        SceneManager.LoadScene(2);
    }

}
