using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyFarmland : ClickObject
{
   private bool hasPlant = false;
   private bool isReady = false;
   private GameObject resultCardPrefab;
   public SpriteRenderer visualRanderer;
   public Sprite dirtSprite;
   public Sprite growingSprite;
   public Sprite readySprite;
   public bool CanPlanted ()=> !hasPlant;
   public void Plant(GameObject cardPrefab)
    {
        hasPlant = true;
        isReady = false;
        resultCardPrefab = cardPrefab;
        visualRanderer.sprite = growingSprite;
        StartCoroutine(GrowRoutine());
    }
    IEnumerator GrowRoutine()
    {
        yield return new WaitForSeconds(3f);
        isReady = true;
        visualRanderer.sprite = readySprite;
        Debug.Log("Plant is ready to harvest!");
    }
    public override void HandleClicked()
    {
        if (hasPlant && isReady)
        {
            Harvest();
        }
    }
    private void Harvest()
    {
        Instantiate(resultCardPrefab, transform.position + Vector3.up, Quaternion.identity);
        hasPlant = false;
        isReady = false;
        visualRanderer.sprite = dirtSprite;
        Debug.Log("Harvested " + resultCardPrefab.name);
    }
}
