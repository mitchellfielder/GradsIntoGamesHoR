using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericPlayerTrigger : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _onTriggerEnter;
    [SerializeField]
    private bool isSingleUse;

    void OnTriggerEnter(Collider collider)
    {
        _onTriggerEnter.Invoke();
        if(isSingleUse)
            Destroy(gameObject);
    }
}
