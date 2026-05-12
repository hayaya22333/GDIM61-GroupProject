using UnityEngine;
using UnityEngine.UI;

public class StoryPageView : MonoBehaviour
{
    public Image pageImage;

    private void Start()
    {
        if (PlayerPrefs.GetInt("StoryPlayed", 0) == 1)
        {
            pageImage.gameObject.SetActive(false);
        }
    }
}