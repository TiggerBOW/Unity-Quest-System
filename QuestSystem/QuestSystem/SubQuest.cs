using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sub Quest", menuName = "Scriptable Objects/Quests/Sub Quest")]
public class SubQuest : Quest
{
    // The main quest to which it is connected.
    public MainQuest MainParentQuest { get; private set; }

    // Method to set the main parent quest it's connected to.
    public void SetMainParentQuest(MainQuest mainParentQuest)
    {
        MainParentQuest = mainParentQuest;
    }
}
