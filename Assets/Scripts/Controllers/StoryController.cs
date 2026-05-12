/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoryController : MonoBehaviour
{
    public StoryController Instance {get; private set;}
    public int titleScene = 0;
    public int pollScene = 1;
    public Sprite[] storyPage;
    [SerializeField] private int pollPage;
    private int currentPage;
    private bool waitToPoll;
    private bool storyEnd;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

    public void ClickNext()
    {
        if(currentPage == 24)
        {
            SceneManager.LoadScene(1);
        }
        currentPage ++;
        if (currentPage < storyPage.Length) GetComponent<Image>().sprite = storyPage[currentPage];
        else gameObject.SetActive(false); 
    }

}
*/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    public static StoryManager Instance;
    public List<Sprite> storyPages = new List<Sprite>();
    public Image pageImage;
    public int pollStartPage = 24;

    private int currentPage = 0;
    private bool waitingForPollButton = false;
    private bool storyFinished = false;

    private const string StoryPlayedKey = "StoryPlayed";

    private void Awake()
    {
        if (PlayerPrefs.GetInt(StoryPlayedKey, 0) == 1)
        {
            Destroy(gameObject);
            return;
        }

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        FindPageImage();
        ShowCurrentPage();
    }

    private void Update()
    {
        if (storyFinished)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if(waitingForPollButton == false)
            {
                NextPage();
            }

        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (storyFinished)
        {
            return;
        }

        FindPageImage();
        ShowCurrentPage();
    }

    private void FindPageImage()
    {
        StoryPageView view = FindObjectOfType<StoryPageView>(true);

        if (view != null)
        {
            pageImage = view.pageImage;
        }
    }


    private void NextPage()
    {
        currentPage++;

        if (currentPage == pollStartPage)
        {
            SceneManager.LoadScene(1);
            return;
        }

        if (currentPage >= storyPages.Count)
        {
            FinishStory();
            return;
        }

        ShowCurrentPage();
    }

    private void ShowCurrentPage()
    {
        if (pageImage == null)
        {
            return;
        }

        pageImage.gameObject.SetActive(true);
        pageImage.sprite = storyPages[currentPage];

        if (currentPage == pollStartPage)
        {
            waitingForPollButton = true;
        }
    }

    public void PollButtonClicked()
    {
        if (currentPage == pollStartPage)
        {
            waitingForPollButton = false;
        }
    }

    private void FinishStory()
    {
        storyFinished = true;

        PlayerPrefs.SetInt(StoryPlayedKey, 1);
        PlayerPrefs.Save();

        if (pageImage != null)
        {
            pageImage.gameObject.SetActive(false);
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;

        SceneManager.LoadScene(0);

        Destroy(gameObject);
    }

    public void ResetStoryForTesting()
    {
        PlayerPrefs.DeleteKey(StoryPlayedKey);
        PlayerPrefs.Save();
    }
}
