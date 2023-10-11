using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Main Quest", menuName = "Scriptable Objects/Quests/Main Quest")]
public class MainQuest : Quest
{
    // List of sub-quests associated with this main quest.
    public List<SubQuest> SubQuests = new List<SubQuest>();

    // Used for displaying the list of sub-quests in the editor.
    [HideInInspector] public bool ShowSubQuests = false;

    // Returns true if all sub-quests are completed.
    public bool CheckSubQuestsIsDone()
    {
        for (int i = 0; i < SubQuests.Count; i++)
        {
            // If there is an incomplete quest, return false.
            if (SubQuests[i].QuestProgress != QuestProgress.COMPLETED)
            {
                return false;
            }
        }
        return true;
    }

    // Set the progress of all sub-quests to the specified progress state.
    public void SetAllSubQuestsProgress(QuestProgress progress)
    {
        for (int i = 0; i < SubQuests.Count; i++)
        {
            SubQuests[i].QuestProgress = progress;
        }
    }

    // Set the main parent quest for all sub-quests.
    public void SetSubQuestsMainQuestParent()
    {
        for (int i = 0; i < SubQuests.Count; i++)
        {
            SubQuests[i].SetMainParentQuest(this);
        }
    }
}
