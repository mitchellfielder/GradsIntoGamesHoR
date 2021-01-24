using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchPickup : Interactable
{
    protected override void Interact(Player player)
    {
        base.Interact(player);
        Destroy(gameObject);
    }
}
