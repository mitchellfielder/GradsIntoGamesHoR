using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceConversation : MonoBehaviour
{
    [SerializeField]
    private StoryData _storyData;
    [SerializeField]
    private bool _isPlayerFreezing = false;
    public void Activate()
    {
        StartCoroutine(DoForceConversation());
    }
    private IEnumerator DoForceConversation()
    {
        Game.game.StopConversation();
        while (!Game.game.StartConversation(_storyData, _isPlayerFreezing))
        {
            yield return null;
        }
        Debug.Log("I think ive started the conversation");
    }
}
