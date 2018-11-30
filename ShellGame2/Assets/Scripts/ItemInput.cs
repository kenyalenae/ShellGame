using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInput : MonoBehaviour, IInputClickHandler
{
    [SerializeField]
    private int itemId;

    public void OnInputClicked(InputClickedEventData eventData)
    {
        GameController.Instance.CheckForItem(itemId);
    }

}
