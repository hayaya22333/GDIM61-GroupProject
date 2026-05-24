using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class Pile : MonoBehaviour
{
    public PollController _pollController;
    public Vector3 spawnPlace = new Vector3(0f, 1f, 0f);
    public int spawnNumber;
    public List<Transform> spawnPosition;
    [SerializeField] private AudioSource click;

    void Awake()
    {
        gameObject.SetActive(true);
    }


    private void OnMouseDown()
    {   

        if (StoryManager.Instance != null)
        {
            StoryManager.Instance.PollButtonClicked();
        }
        click.Play();
        /*
        Collider2D pile = GetComponent<Collider2D>();

        for (int i = 0; i < spawnNumber; i++)
        {
            int randomCardID = Random.Range(0, _pollController.cardPoll.Count);
            GameObject selectedPile = _pollController.cardPoll[randomCardID];
        
            PollCard pollCard = _pollController.cardPoll[randomCardID].GetComponent<PollCard>();
 
            GameController.Instance.OwnCard(pollCard.cardID);
            Instantiate(selectedPile, spawnPosition[i].position, Quaternion.identity);
            _pollController.cardPoll.RemoveAt(randomCardID);
            Debug.Log(i);
        }*/
        


        for (int i = 0; i < spawnNumber; i++)
        {
            int selectedIndex = -1;
            if (GameController.Instance.firstPoll == true && i == 0)
            {
                List<int> attackCard = new List<int>();
                for (int j = 0; j < _pollController.cardPoll.Count; j++)
                {
                    PollCard _pollCard = _pollController.cardPoll[j].GetComponent<PollCard>();
                    if (_pollCard.cardNode.skills[0].skillEffects[0].effectType == EffectType.Damage)
                    {
                        attackCard.Add(j);
                    }
                }

                if(attackCard.Count > 0)
                {
                    int Attack = Random.Range(0, attackCard.Count);
                    selectedIndex = attackCard[Attack];
                }
                else
                {
                    selectedIndex = Random.Range(0, _pollController.cardPoll.Count);
                }

                GameController.Instance.firstPoll = false;
            }
            else
            {
                selectedIndex = Random.Range(0, _pollController.cardPoll.Count);
            }

            GameObject _pile = _pollController.cardPoll[selectedIndex];

            PollCard pollCard = _pile.GetComponent<PollCard>();

            GameController.Instance.OwnCard(pollCard.cardID);

            Instantiate(_pile, spawnPosition[i].position, Quaternion.identity);

            _pollController.cardPoll.RemoveAt(selectedIndex);
        }

        gameObject.SetActive(false);
        Debug.Log("Owncard");

    }
}

