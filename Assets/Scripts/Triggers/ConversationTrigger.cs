using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationTrigger : MonoBehaviour
{


    [SerializeField]
    private StoryData _conversationData;
    [SerializeField]
    private bool _bIsSingleUse = true;
    [SerializeField]
    private bool _isPlayerMovementDisabled = true;

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (player)
        {
            if (_conversationData)
            {


                bool isSuccessful = Game.game.StartConversation(_conversationData, _isPlayerMovementDisabled);
                if (isSuccessful && _bIsSingleUse)
                    Destroy(this);
            }
            else
            {
                Game.game.StopConversation();
                Destroy(this);

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_isPlayerMovementDisabled)
        {
            Player player = other.gameObject.GetComponent<Player>();
            if (player)
            {
                 Game.game.StopConversation();
            }
        }
    }


}
