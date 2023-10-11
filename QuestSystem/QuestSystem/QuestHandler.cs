using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuestHandler : MonoBehaviour
{
    // List of all quests
    public List<MainQuest> MainQuestList = new List<MainQuest>();

    // Current quest (the quest seen in the UI)
    public MainQuest AvailableMainQuest { get; private set; }

    private void Awake()
    {
        SetAvailableMainQuest(MainQuestList[0]);

        // All quests start as not completed at the beginning. This is a temporary system until a save system is implemented...
        for (int i = 1; i < MainQuestList.Count; i++)
        {
            MainQuestList[i].SetQuestProgress(QuestProgress.NOT_AVAILABLE);
            MainQuestList[i].SetAllSubQuestsProgress(QuestProgress.NOT_AVAILABLE);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            CheckAndCompleteSubQuest(GetSubQuestWithQuestId(0));
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            CheckAndCompleteSubQuest(GetSubQuestWithQuestId(1));
        }
    }

    // If the given subquest exists in the current main quest, it marks it as completed.
    public void CheckAndCompleteSubQuest(SubQuest subQuest = null)
    {
        if (AvailableMainQuest.SubQuests.Count == 0) // If there are no subquests, the quest is immediately completed.
        {
            StartCoroutine(MainQuestCompletedNumerator());
            return;
        }

        for (int i = 0; i < AvailableMainQuest.SubQuests.Count; i++)
        {
            if (AvailableMainQuest.SubQuests[i] == subQuest)
            {
                subQuest.SetQuestProgress(QuestProgress.COMPLETED);
                QuestUiHandler.Instance.SetQuestChecked(subQuest, true);

                // If all subquests are completed, move on to the next main quest.
                if (AvailableMainQuest.CheckSubQuestsIsDone())
                {
                    StartCoroutine(MainQuestCompletedNumerator());
                }

                return;
            }
        }
        Debug.LogError("The given subquest could not be found in this main quest: " + subQuest);
    }

    // Finds a subquest with the given ID.
    public SubQuest GetSubQuestWithQuestId(int id)
    {
        for (int i = 0; i < AvailableMainQuest.SubQuests.Count; i++)
        {
            if (AvailableMainQuest.SubQuests[i].QuestId == id)
            {
                return AvailableMainQuest.SubQuests[i];
            }
        }

        Debug.LogError("The entered ID is invalid.");
        return null;
    }

    // Sets the selected main quest as the current main quest.
    // This method is not healthy; it jumps directly to a quest, so be careful when using it.
    private void SetAvailableMainQuest(MainQuest quest)
    {
        // If there's an old quest, it is no longer available.
        if (AvailableMainQuest != null)
        {
            AvailableMainQuest.SetQuestProgress(QuestProgress.NOT_AVAILABLE);
            AvailableMainQuest.SetAllSubQuestsProgress(QuestProgress.NOT_AVAILABLE);
        }

        AvailableMainQuest = quest;

        SetAvailableMainQuestAndShowList();
    }

    // Gets the next main quest in line and sets it as the current main quest.
    private void GetNextMainQuestToAvailable()
    {
        // If the limit is exceeded (i.e., a quest is obtained after the last quest), it returns.
        if (MainQuestList.Count - 1 < AvailableMainQuest.QuestId + 1)
            return;

        MainQuest newQuest = MainQuestList[AvailableMainQuest.QuestId + 1];

        AvailableMainQuest = newQuest;

        SetAvailableMainQuestAndShowList();
    }

    // Sets the progress of the selected main quest and lists it in the UI.
    private void SetAvailableMainQuestAndShowList()
    {
        AvailableMainQuest.SetAllSubQuestsProgress(QuestProgress.AVAILABLE);
        AvailableMainQuest.SetQuestProgress(QuestProgress.AVAILABLE);

        QuestUiHandler.Instance.ShowQuestsOnList(AvailableMainQuest);
    }

    // Code that will run animations, waits, etc. when the main quest is completed.
    private IEnumerator MainQuestCompletedNumerator()
    {
        QuestUiHandler.Instance.SetQuestChecked(AvailableMainQuest, true);
        yield return new WaitForSeconds(2);
        GetNextMainQuestToAvailable();
        QuestUiHandler.Instance.SetQuestChecked(AvailableMainQuest, false);
    }
}
