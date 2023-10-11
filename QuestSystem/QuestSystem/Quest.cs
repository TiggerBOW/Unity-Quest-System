using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Quest : ScriptableObject
{
    // Quest title
    public string QuestTitle { get; set; }
    // Quest description (for developers only, not shown in the game)
    public string QuestDescription { get; set; }
    // Quest ID, automatically set based on its position in the list
    public int QuestId { get; set; }
    // Current progress of the quest
    public QuestProgress QuestProgress { get; set; }
    // Methods called when the quest is completed.
    public UnityEvent OnQuestCompleted { get; set; }

    // Data to be taken from the Inspector.
    [SerializeField] private string _questTitle;
    [SerializeField] private int _questId;
    [SerializeField]
    [TextArea(3, 10)]
    [Tooltip("For developers only, not shown in the game")]
    private string _questDescription;
    [SerializeField] private UnityEvent _onQuestCompleted;

    private void Awake()
    {
        // At the start of the game, quests begin as not available (closed).
        SetQuestProgress(QuestProgress.NOT_AVAILABLE);
    }

    // Called when changes are made in the editor.
    private void OnValidate()
    {
        QuestTitle = _questTitle;
        QuestId = _questId;
        QuestDescription = _questDescription;
        OnQuestCompleted = _onQuestCompleted;
    }

    // Changes the current progress of the quest.
    public void SetQuestProgress(QuestProgress progress)
    {
        QuestProgress = progress;

        // If the quest is completed, observer methods are called.
        if (progress == QuestProgress.COMPLETED)
        {
            OnQuestCompleted?.Invoke();
        }
    }
}

// AVAILABLE: Quests within this progress are listed in the UI and are incomplete quests.
// NOT_AVAILABLE: Quests within this progress are upcoming quests not yet listed in the UI.
// COMPLETED: Quests within this progress appear in the list as completed and are removed afterward.
public enum QuestProgress
{
    AVAILABLE, NOT_AVAILABLE, COMPLETED
}
