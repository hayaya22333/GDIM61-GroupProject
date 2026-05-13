using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pollHint : MonoBehaviour
{
    private SpriteRenderer sr;
    public bool inStory = true;
    private GameObject story;

    int ownCardBefore;

    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        sr.enabled = false;
        ownCardBefore = GameController.Instance.ownCard.Count;
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

        if (GameController.Instance.ownCard.Count > ownCardBefore && !inStory)
        {
            sr.enabled = true;
        }
    }
}
