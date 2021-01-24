using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationInteractable : Interactable
{
    public StoryData ConversationData;

    protected override void Interact(Player player)
    {
        Game.game.StartConversation(ConversationData);
        base.Interact(player);
    }
}
