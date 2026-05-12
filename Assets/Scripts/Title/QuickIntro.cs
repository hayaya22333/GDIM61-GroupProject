using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuickIntro : MonoBehaviour
{
    public Sprite[] sprites;
    int index = 0;

    public void ClickNext()
    {
        if(index == 24)
        {
            SceneManager.LoadScene(1);
        }
        index++;
        if (index < sprites.Length) GetComponent<Image>().sprite = sprites[index];
        else gameObject.SetActive(false); 
    }
}
