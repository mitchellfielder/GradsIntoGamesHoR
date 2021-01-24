using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLightColour : MonoBehaviour
{
    [SerializeField]
    private Color _newColor; 


    public void Change()
    {
        GetComponent<Light>().color = _newColor;
    }
}
