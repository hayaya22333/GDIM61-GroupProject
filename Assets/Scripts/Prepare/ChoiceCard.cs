using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceCard : DragObject
{
    public CardNode cardNode;
    public string cardDesciption;
    //写一段对应的card description
    public PrepareController prepareController;
    public void ShowTextDetail()
    {
        prepareController.detail.text = cardDesciption;
        Debug.Log("click");
    }
}
