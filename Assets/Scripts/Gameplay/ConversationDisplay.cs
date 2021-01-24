using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConversationDisplay : MonoBehaviour
{

    public enum State { Speaking, ListeningForInput, Idle }
    public static ConversationDisplay Display { get; protected set; }
    public TMP_Text DisplayText;
    private Conversation _currentConversation;
    public State CurrentState { get; } = State.Idle;

    void Awake()
    {
        DisplayText = GetComponent<TMP_Text>();
        //Using singleton design pattern
        if (Display)
        {
            Debug.LogWarning(
                "Several ConversationDisplay components found. Please make sure there is only one in the world at any given time");
            Destroy(this);
        }

        Display = this;
    }

    // Update is called once per frame
    public void DisplayBeat(BeatData beat, Conversation conversation)
    {
        _currentConversation = conversation; 
        string text = beat.DisplayText;
        foreach (ChoiceData decision in beat.Decision)
        {
            text += "\n" + decision.DisplayText;
        }
        DisplayText.text = text;
    }
}
