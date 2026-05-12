using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class story : MonoBehaviour
{
    public Sprite[] pollSprite;
    private int index;


    void Start()
    {
        {
            Destroy(this);
        }
    }

    void Update()
    {
        if (index == 0)
        {
            
        }
    }
    public void ClickNext()
    {
        index++;
        if (index < pollSprite.Length) 
        {
            GetComponent<Image>().sprite = pollSprite[index];
        }
        else 
        {
            gameObject.SetActive(false); 
        }
    }
}
