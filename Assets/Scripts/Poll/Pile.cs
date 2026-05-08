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
    

    private void OnMouseDown()
    {   
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
        }
        
        gameObject.SetActive(false);
        Debug.Log("Owncard");
    }
}
