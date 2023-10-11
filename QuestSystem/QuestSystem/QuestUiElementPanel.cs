using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestUiElementPanel : MonoBehaviour
{
    [HideInInspector] public List<GameObject> ListedSubQuestsObjects = new List<GameObject>();

    [SerializeField] private GameObject _mainQuestUiElementObejct;
    [SerializeField] private GameObject _subQuestListElementObject;

    [SerializeField] private GameObject _subQuestElementPrefabObject;

    public void ListSubQuests(MainQuest mainQuest)
    {
        ChangeMainQuestText(mainQuest.QuestTitle);

        ClearSubQuestsList();

        for (int i = 0;i < mainQuest.SubQuests.Count; i++)
        {
            AddSubQuestOnList(mainQuest.SubQuests[i]);
        }

        mainQuest.SetSubQuestsMainQuestParent();
    }
    public List<QuestUiElement> GetListedSubQuestObjectElements()
    {
        List<QuestUiElement> uiElements = new List<QuestUiElement>();
        for (int i = 0;i < ListedSubQuestsObjects.Count;i++)
        {
            ListedSubQuestsObjects[i].TryGetComponent(out QuestUiElement uiElement);

            uiElements.Add(uiElement);
        }

        return uiElements;
    }
    public QuestUiElement GetSubQuestElementWithConnectedQuest(SubQuest quest)
    {
        List<QuestUiElement> uiElements =  GetListedSubQuestObjectElements();
        for(int i = 0;i < uiElements.Count;i++)
        {
            if (uiElements[i].ConnectedQuest == quest)
            {
                return uiElements[i];
            }
        }

        return null;
    }
    public QuestUiElement GetMainQuestUiElement()
    {
        return _mainQuestUiElementObejct.GetComponent<QuestUiElement>();
    }
    private void ChangeMainQuestText(string text)
    {
        _mainQuestUiElementObejct.GetComponentInChildren<TMP_Text>().text = text;
    }
    private void AddSubQuestOnList(SubQuest subQuest)
    {
        GameObject elementObj = Instantiate(_subQuestElementPrefabObject,Vector2.zero, Quaternion.identity);
        elementObj.transform.SetParent(_subQuestListElementObject.transform,false);

        elementObj.GetComponent<QuestUiElement>().ConnectedQuest = subQuest;
        elementObj.GetComponentInChildren<TMP_Text>().text = subQuest.QuestTitle;
        ListedSubQuestsObjects.Add(elementObj);
    }
    private void ClearSubQuestsList()
    {
        // ListedSubQuestsObjects listesini ters sýrayla dönerek güncelleyin.
        for (int i = ListedSubQuestsObjects.Count - 1; i >= 0; i--)
        {
            Destroy(ListedSubQuestsObjects[i]); // Önce nesneyi yok edin.
            ListedSubQuestsObjects.RemoveAt(i);   // Sonra listeden kaldýrýn.
        }
    }

}
