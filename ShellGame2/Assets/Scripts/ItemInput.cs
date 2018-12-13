using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInput : MonoBehaviour, IInputClickHandler
{
    // \/ tells unity to take info into ram for faster gaming
    [SerializeField]
    private int itemId;

    //            \/ used when you tap an item (box/shell) in the game, such as when you click the trigger
    public void OnInputClicked(InputClickedEventData eventData)
    {
        // method in GameController that does if user clicked 
        GameController.Instance.CheckForItem(itemId);
    }

}
