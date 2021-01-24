using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Conversation : MonoBehaviour
{
    [SerializeField] private StoryData _data;
    private BeatData _currentBeat;



    // Start is called before the first frame update
    void Start()
    {
        _currentBeat = _data.GetBeatById(1);

        ConversationDisplay.Display.DisplayBeat(_currentBeat, this);


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
