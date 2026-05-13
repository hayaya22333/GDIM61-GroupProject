using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pollHint : MonoBehaviour
{
    private SpriteRenderer sr;
    public bool inStory = true;
    private GameObject story;

    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        sr.enabled = false;
    }

    void Update()
    {
        story = GameObject.Find("StoryController");
        if (story != null)
        {
            inStory = true;
        }
        else
        {
            inStory = false;
        }

        if (GameController.Instance.ownCard.Count >= 3 && !inStory)
        {
            sr.enabled = true;
        }
    }
}
