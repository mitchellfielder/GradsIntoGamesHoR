using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioSpike : MonoBehaviour
{
    [SerializeField]
    private Transform _playerCamera;
    private AudioSource _audioSource;

    [SerializeField, Range(-1,1)]
    private float _angle = 0.95f;
    [SerializeField]
    private float _distance = 10f;

    /*
    [SerializeField]
    private bool _isInDark;*/
    [SerializeField]
    private UnityEvent _OnSpikEvent;


    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update()
    {
        int layerMask = 1 << 3;
        layerMask = ~layerMask;


        if(Vector3.Distance(transform.position, _playerCamera.transform.position) < _distance)
        {
            if (Physics.Linecast(transform.position, _playerCamera.transform.position, layerMask))
            {
                Debug.DrawLine(transform.position, _playerCamera.transform.position, Color.red);
            }
            else
            {

                float dotProduct = Vector3.Dot(_playerCamera.transform.forward.normalized,
                    (transform.position - _playerCamera.transform.position).normalized);

                
                if (dotProduct > _angle)
                {
                    _audioSource.Play();
                    _OnSpikEvent.Invoke();
                    this.enabled = false;
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
