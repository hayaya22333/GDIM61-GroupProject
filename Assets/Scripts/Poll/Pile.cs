using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class Pile : MonoBehaviour
{
    public PollController _pollController;
    public Vector3 spawnPlace = new Vector3(0f, 1f, 0f);

    private void OnMouseDown()
    {   
        Collider2D pile = GetComponent<Collider2D>();
        Vector3 spawnPosition = pile.transform.position + spawnPlace;

        int randomCardID = Random.Range(0, _pollController.cardPoll.Count);
        GameObject selectedPile = _pollController.cardPoll[randomCardID];
        
        PollCard pollCard = _pollController.cardPoll[randomCardID].GetComponent<PollCard>();
 
        //GameController.Instance.ownCard.Add(pollCard.cardID);
        Instantiate(selectedPile, spawnPosition, Quaternion.identity);
        _pollController.cardPoll.RemoveAt(randomCardID);

        gameObject.SetActive(false);
        Debug.Log("Owncard");
    }
}
