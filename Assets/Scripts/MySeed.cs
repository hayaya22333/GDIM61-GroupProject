using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MySeed : DragObject
{
    public string seedName;
    public GameObject cardToProduce;
    private void OnTriggerStay2D(Collider2D other)
    {
       if (other.CompareTag("Farmland")&& !isDragging)
        {
            MyFarmland farm = other.GetComponent<MyFarmland>();
            if(farm != null && farm.CanPlanted())
            {
                farm.Plant(cardToProduce);
                Destroy(gameObject);
                Debug.Log("Planted " + seedName);
            }
        }
    }
}
