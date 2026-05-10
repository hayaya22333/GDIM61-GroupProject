using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.UI;

public class QuickIntro : MonoBehaviour
{
    public Sprite[] sprites;
    int index = 0;

    public void ClickNext()
    {
        index++;
        if (index < sprites.Length) GetComponent<Image>().sprite = sprites[index];
        else gameObject.SetActive(false); 
    }
}
