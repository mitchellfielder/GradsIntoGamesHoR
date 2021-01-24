using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrail : MonoBehaviour
{
    [SerializeField]
    private float _lifetime = 0.2f;
    private float _lifetimeTimer = 0.0f;



    void Update()
    {
        if (_lifetimeTimer < _lifetime)
        {
            _lifetimeTimer += Time.deltaTime;
            float scale = 1 - (_lifetimeTimer/ _lifetime);
            transform.localScale = new Vector3(scale, scale, transform.localScale.z);
        }
        else
        {
            Destroy(gameObject);
        }


    }
}
