using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private float _openAngle =45.0f;
    [SerializeField]
    private float _speed = 45.0f;


    public void Open()
    {
        StartCoroutine(DoOpenDoor());
        
    }

    private IEnumerator DoOpenDoor()
    {
        float _currentAngle = 0.0f;
        while (Mathf.Abs(_openAngle - _currentAngle ) > 1.0f)
        {
            float angleThisFrame = Mathf.Lerp(_currentAngle, _openAngle, Time.deltaTime * _speed)- _currentAngle;
            transform.Rotate(new Vector3(0.0f, angleThisFrame,0.0f));
            _currentAngle += angleThisFrame;
            yield return null;

        }
    }
}
