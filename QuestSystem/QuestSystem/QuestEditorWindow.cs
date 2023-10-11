using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class QuestEditorWindow : EditorWindow
{
    private Vector2 scrollPosition;
    private List<MainQuest> mainQuests; // List of main quests
    private int selectedMainQuestIndex = -1; // To indicate that no main quest is initially selected
    private int selectedSubQuestIndex = -1; // To indicate that no sub quest is initially selected
    private int swapQuestId1 = 0;
    private int swapQuestId2 = 0;

    [MenuItem("Window/Quest Editor")]
    public static void ShowWindow()
    {
        QuestEditorWindow window = GetWindow<QuestEditorWindow>("Quest Editor");
        window.minSize = new Vector2(400, 600);
        window.Show();
    }

    private void OnEnable()
    {
        LoadMainQuests(); // Load the main quests
    }

    private void LoadMainQuests()
    {
        mainQuests = FindObjectOfType<QuestHandler>().MainQuestList;
    }

    private void OnGUI()
    {
        GUILayout.Label("Quest Editor", EditorStyles.boldLabel);

        // Start the ScrollView
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(300));

        for (int i = 0; i < mainQuests.Count; i++)
        {
            MainQuest quest = mainQuests[i];

            // Set background color for the list item
            Color bgColor = (i % 2 == 0) ? new Color(0.8f, 0.8f, 0.8f) : new Color(0.9f, 0.9f, 0.9f);

            Rect rect = EditorGUILayout.BeginHorizontal();
            EditorGUI.DrawRect(rect, bgColor);

            GUIStyle labelStyle = new GUIStyle(EditorStyles.label);
            labelStyle.normal.textColor = Color.black;
            labelStyle.fontStyle = FontStyle.Bold;

            if (GUILayout.Button("➤", GUILayout.Width(30)))
            {
                SelectMainQuest(i);
            }

            // Add a variable to make the selected main quest's text color different
            if (i == selectedMainQuestIndex)
            {
                labelStyle.normal.textColor = Color.blue;
            }

            GUILayout.Label("ID: " + quest.QuestId, labelStyle, GUILayout.Width(60));
            GUILayout.Label("Quest: " + quest.QuestTitle, labelStyle);

            // Button to show/hide sub-quests
            if (GUILayout.Button(quest.ShowSubQuests ? "▼" : "▲", GUILayout.Width(20)))
            {
                quest.ShowSubQuests = !quest.ShowSubQuests;
            }

            // Up button
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.normal.background = null;
            buttonStyle.normal.textColor = Color.black;

            if (GUILayout.Button("↑", buttonStyle, GUILayout.Width(20)) && i > 0)
            {
                SwapMainQuests(i, i - 1);
            }

            // Down button
            if (GUILayout.Button("↓", buttonStyle, GUILayout.Width(20)) && i < mainQuests.Count - 1)
            {
                SwapMainQuests(i, i + 1);
            }

            EditorGUILayout.EndHorizontal();

            // If sub-quests are visible, list them
            if (quest.ShowSubQuests)
            {
                for (int j = 0; j < quest.SubQuests.Count; j++)
                {
                    SubQuest subQuest = quest.SubQuests[j];

                    // Set background color for the sub-quest list item
                    Color subQuestBgColor = (j % 2 == 0) ? new Color(0.8f, 0.8f, 0.8f) : new Color(0.9f, 0.9f, 0.9f);

                    Rect subQuestRect = EditorGUILayout.BeginHorizontal();
                    EditorGUI.DrawRect(subQuestRect, subQuestBgColor);

                    // Indent sub-quests
                    GUIStyle subQuestLabelStyle = new GUIStyle(EditorStyles.label);
                    subQuestLabelStyle.normal.textColor = Color.black;
                    subQuestLabelStyle.fontStyle = FontStyle.Bold;
                    subQuestLabelStyle.fontSize = 11;

                    GUILayout.Space(20);
                    if (GUILayout.Button("➤", GUILayout.Width(30)))
                    {
                        SelectSubQuest(i, j);
                    }

                    // Add a variable to make the selected sub-quest's text color different
                    if (i == selectedMainQuestIndex && j == selectedSubQuestIndex)
                    {
                        Color subQuestTextColor = new Color(0.0f, 0.5f, 0.0f, 1.0f); // R:0, G:0.5, B:0, A:1
                        subQuestLabelStyle.normal.textColor = subQuestTextColor;
                    }

                    GUILayout.Space(5); // Add some spacing around the ID
                    GUILayout.BeginHorizontal();

                    GUILayout.Label("ID: " + subQuest.QuestId, subQuestLabelStyle, GUILayout.Width(40));
                    GUILayout.Label("Sub Quest: " + subQuest.QuestTitle, subQuestLabelStyle);

                    GUILayout.EndHorizontal();

                    // Up and down buttons
                    if (GUILayout.Button("↑", GUILayout.Width(20)) && j > 0)
                    {
                        SwapSubQuests(quest, j, j - 1);
                    }
                    if (GUILayout.Button("↓", GUILayout.Width(20)) && j < quest.SubQuests.Count - 1)
                    {
                        SwapSubQuests(quest, j, j + 1);
                    }

                    EditorGUILayout.EndHorizontal();
                }
            }
        }

        EditorGUILayout.EndScrollView();

        GUILayout.Space(10);

        // Update IDs if the order has changed
        for (int i = 0; i < mainQuests.Count; i++)
        {
            mainQuests[i].QuestId = i;
        }

        // Add a panel to display the selected main and sub-quest information
        GUILayout.Space(10);
        GUILayout.Label("Selected Quest Info", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("box");
        EditorGUI.indentLevel++;
        if (selectedMainQuestIndex != -1)
        {
            MainQuest selectedMainQuest = mainQuests[selectedMainQuestIndex];
            EditorGUILayout.LabelField("Main Quest ID: " + selectedMainQuest.QuestId);
            EditorGUILayout.LabelField("Main Quest Title: " + selectedMainQuest.QuestTitle);

            // Display main quest description as a read-only LabelField
            EditorGUILayout.LabelField("Main Quest Description: ");
            EditorGUILayout.LabelField(selectedMainQuest.QuestDescription, EditorStyles.textArea);

            EditorGUILayout.Space(); // Add some spacing
        }

        if (selectedSubQuestIndex != -1)
        {
            SubQuest selectedSubQuest = mainQuests[selectedMainQuestIndex].SubQuests[selectedSubQuestIndex];
            EditorGUILayout.LabelField("Sub Quest ID: " + selectedSubQuest.QuestId);
            EditorGUILayout.LabelField("Sub Quest Title: " + selectedSubQuest.QuestTitle);

            // Display sub-quest description as a read-only LabelField
            EditorGUILayout.LabelField("Sub Quest Description: ");
            EditorGUILayout.LabelField(selectedSubQuest.QuestDescription, EditorStyles.textArea);

            EditorGUILayout.Space(); // Add some spacing
        }
        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();

        // Add an input field and "Swap" button for the swapping functionality
        GUILayout.Space(10);
        GUILayout.Label("Change Quest Orders", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("box");
        EditorGUI.indentLevel++;
        swapQuestId1 = EditorGUILayout.IntField("Quest ID 1: ", swapQuestId1);
        swapQuestId2 = EditorGUILayout.IntField("Quest ID 2: ", swapQuestId2);
        if (GUILayout.Button("Change"))
        {
            SwapQuestsByID(swapQuestId1, swapQuestId2);
        }
        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
    }

    private void SwapMainQuests(int index1, int index2)
    {
        if (index1 >= 0 && index1 < mainQuests.Count && index2 >= 0 && index2 < mainQuests.Count)
        {
            MainQuest tempMainQuest = mainQuests[index1];
            mainQuests[index1] = mainQuests[index2];
            mainQuests[index2] = tempMainQuest;
        }
        else
        {
            Debug.LogError("Invalid main quest index: " + index1 + " or " + index2);
        }
    }

    private void SwapSubQuests(MainQuest mainQuest, int index1, int index2)
    {
        if (mainQuest != null && index1 >= 0 && index1 < mainQuest.SubQuests.Count && index2 >= 0 && index2 < mainQuest.SubQuests.Count)
        {
            SubQuest tempSubQuest = mainQuest.SubQuests[index1];
            mainQuest.SubQuests[index1] = mainQuest.SubQuests[index2];
            mainQuest.SubQuests[index2] = tempSubQuest;

            // Update the IDs of sub-quests when swapping their positions
            for (int i = 0; i < mainQuest.SubQuests.Count; i++)
            {
                mainQuest.SubQuests[i].QuestId = i;
            }
        }
        else
        {
            Debug.LogError("Invalid sub-quest index: " + index1 + " or " + index2);
        }
    }

    private void SelectMainQuest(int index)
    {
        selectedMainQuestIndex = index;
        selectedSubQuestIndex = -1; // Clear sub-quest selection when a main quest is selected
    }

    private void SelectSubQuest(int mainQuestIndex, int subQuestIndex)
    {
        selectedMainQuestIndex = mainQuestIndex;
        selectedSubQuestIndex = subQuestIndex;
    }

    private void SwapQuestsByID(int questId1, int questId2)
    {
        MainQuest quest1 = mainQuests.Find(q => q.QuestId == questId1);
        MainQuest quest2 = mainQuests.Find(q => q.QuestId == questId2);
        if (quest1 != null && quest2 != null)
        {
            int index1 = mainQuests.IndexOf(quest1);
            int index2 = mainQuests.IndexOf(quest2);
            SwapMainQuests(index1, index2);
        }
    }
}
