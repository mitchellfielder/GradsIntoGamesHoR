using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class OnLookEvent : MonoBehaviour
{
    [SerializeField]
    private Transform _playerCamera;
    private AudioSource _audioSource;

    [SerializeField, Range(-1, 1)]
    private float _angle = 0.95f;

    [SerializeField]
    private float _distance = 10f;
    [SerializeField]
    private bool _isSingleUse = true;
    [SerializeField]
    private bool _isInDark = false;
    [SerializeField]
    private UnityEvent _onLookEvent;




    // Update is called once per frame
    void Update()
    {

        int ignoreUILayer = 2;
        int playerLayer = 3;
        int layerMask = ~((1 << ignoreUILayer) | (1 << playerLayer));

        if (Vector3.Distance(transform.position, _playerCamera.transform.position) < _distance)
        {
            if (Physics.Linecast(transform.position, _playerCamera.transform.position, layerMask))
            {
                Debug.DrawLine(transform.position, _playerCamera.transform.position, Color.red);
            }
            else
            {

                float dotProduct = Vector3.Dot(_playerCamera.transform.forward.normalized,
                    (transform.position - _playerCamera.transform.position).normalized);
                //_currentAngle = dotProduct;

                if (dotProduct > _angle)
                {
                    _onLookEvent.Invoke();
                    if (_isSingleUse)
                    {
                        this.enabled = false;
                    }
                }
            }
        }


    }

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _distance);
    }
}