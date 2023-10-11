using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(QuestHandler))]
public class QuestHandlerEditor : Editor
{
    private string questNameFilter = "";
    private Vector2 scrollPosition;

    public override void OnInspectorGUI()
    {
        QuestHandler questHandler = (QuestHandler)target;

        if (questHandler == null)
        {
            EditorGUILayout.LabelField("Quest Handler is null.");
            return;
        }

        // Add a field for quest name filter
        GUILayout.BeginHorizontal();
        GUILayout.Label("Quest Name Filter:", GUILayout.Width(120));
        questNameFilter = EditorGUILayout.TextField(questNameFilter);
        GUILayout.EndHorizontal();

        // Convert the search text to lowercase
        string lowerCaseFilter = questNameFilter.ToLower();

        // Start the ScrollView
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(200));

        if (questHandler.MainQuestList != null)
        {
            // List filtered quests
            foreach (var quest in questHandler.MainQuestList)
            {
                if (quest != null)
                {
                    // Convert quest name to lowercase
                    string questTitleLower = quest.QuestTitle.ToLower();

                    // Compare with the search text converted to lowercase
                    if (string.IsNullOrEmpty(questNameFilter) || questTitleLower.Contains(lowerCaseFilter))
                    {
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("ID: " + quest.QuestId, GUILayout.Width(30));
                        EditorGUILayout.LabelField("Quest: " + quest.QuestTitle);

                        // When the "Select" button is clicked
                        if (GUILayout.Button("Select", GUILayout.Width(60)))
                        {
                            // Highlight the quest
                            Selection.activeObject = quest;
                            EditorGUIUtility.PingObject(quest);
                        }

                        GUILayout.EndHorizontal();
                    }
                }
            }
        }

        // End the ScrollView
        EditorGUILayout.EndScrollView();

        // Other inspector content
        DrawDefaultInspector();

        // Button to open the Quest Editor window
        if (GUILayout.Button("Open Quest Editor"))
        {
            QuestEditorWindow.ShowWindow(); // Open the Quest Editor window
        }
    }
}
