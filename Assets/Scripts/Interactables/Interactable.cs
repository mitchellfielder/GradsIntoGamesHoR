using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField]
    protected UnityEvent OnInteract;

    [SerializeField]
    protected bool _isActive = true;
    public void SetActive(bool isActive){_isActive = isActive; }

    public void OnClick(Player player)
    {
        if (!_isActive)
            return;
        Interact(player);
    }

    protected virtual void Interact(Player player)  //abstract
    {
        OnInteract.Invoke();
    }
}
