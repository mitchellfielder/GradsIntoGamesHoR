using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlightRandomMovement : MonoBehaviour
{
    private Vector3 _startPosition;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _distance;

    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _startPosition + Vector3.right * Mathf.PerlinNoise(0.0f, Time.time * _speed) * _distance + Vector3.up * Mathf.PerlinNoise(3245.254f, Time.time * _speed) * _distance;
    }
}
