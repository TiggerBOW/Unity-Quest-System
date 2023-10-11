using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestUiHandler : MonoSingleton<QuestUiHandler>
{
    [SerializeField] private QuestUiElementPanel _questUiElementPanel;

    public void ShowQuestsOnList(MainQuest mainQuest)
    {
        _questUiElementPanel.ListSubQuests(mainQuest);
    }
    public void SetQuestChecked(Quest quest,bool value)
    {
        if (quest.GetType() == typeof(SubQuest))
        {
            QuestUiElement uiElement = _questUiElementPanel.GetSubQuestElementWithConnectedQuest((SubQuest)quest);
            uiElement.SetCheckMark(value);
        }
        else
        {
            QuestUiElement uiElement = _questUiElementPanel.GetMainQuestUiElement();
            uiElement.SetCheckMark(value);
        }
    }
}
