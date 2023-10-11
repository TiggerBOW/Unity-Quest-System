using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUiElement : MonoBehaviour
{
    public Quest ConnectedQuest { get; set; }

    [SerializeField] private Toggle _questCompletedCheck;

    public void SetCheckMark(bool value) 
    { 
        _questCompletedCheck.isOn = value;
    }
}
