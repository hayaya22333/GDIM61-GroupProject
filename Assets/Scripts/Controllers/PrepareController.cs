using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PrepareController : MonoBehaviour
{
    [SerializeField] private Transform dockPositionleft;
    [SerializeField] private Transform dockPositionright;
    [SerializeField] private GameObject[] cardChoice;
    //[SerializeField] private TMP_Text cardCount;
    [SerializeField] private Slot[] slot;
    //public TMP_Text detail;
    private List<ChoiceCard> choiceCards = new List<ChoiceCard>();

    // Hayaya: debugging....
    public bool cardsLocked = false;
    public static PrepareController Instance { get; private set; }
    public AudioSource click;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Start()
    {
        //detail.enabled = false;
        //BuildDock();
        ShowCard();
    }
    public void Click(int i)
    {
        // Hayaya: added the following line to lock card slot on click scene change button.
        cardsLocked = true;
        click.Play();
        SceneManager.LoadScene(i);
    }

    //public void Click()
    //{
        //detail.enabled = true;
    //}

    public void ShowCard()
    {
        Debug.Log("show");
        float leftX = dockPositionleft.position.x;
        float rightX = dockPositionright.position.x;
        float y = dockPositionleft.position.y;
        float z = dockPositionleft.position.z;
        float step = (rightX - leftX) / (GameController.Instance.ownCard.Count - 1);
        int count = GameController.Instance.ownCard.Count;
        for(int i = 0; i < count; i++)
        {
            cardChoice[GameController.Instance.ownCard[i]].SetActive(true);
            cardChoice[GameController.Instance.ownCard[i]].transform.position = new Vector3(leftX + step * i, y,z);
            ChoiceCard choiceCard = cardChoice[i].GetComponent<ChoiceCard>();
            choiceCard.DockPosition = cardChoice[GameController.Instance.ownCard[i]].transform.position;
        }
    }

    public void StopCard(int id)
    {
        for( int i = 0; i < cardChoice.Length; i++)
        {
            ChoiceCard choiceCard = cardChoice[i].GetComponent<ChoiceCard>();
            if (choiceCard.alreadyStay == true)
            {
                choiceCard.canStay = true;
            }
            else
            {
                choiceCard.canStay = false;
            }
        }
        ChoiceCard choiceCard1 = cardChoice[id].GetComponent<ChoiceCard>();
        choiceCard1.canStay = true;
    }

    public void LetCard()
    {
        for (int i = 0; i < cardChoice.Length; i++)
        {
            ChoiceCard choiceCard = cardChoice[i].GetComponent<ChoiceCard>();
            choiceCard.canStay = true;
        }
    }

}