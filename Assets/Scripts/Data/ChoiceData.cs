using System;
using UnityEngine;

[Serializable]
public class ChoiceData
{
    [SerializeField] private string _text;
    [SerializeField] private int _beatId;
    [SerializeField] private bool _isAvailable = true;
    [SerializeField] private int _invokeEvent = -1;

    public string DisplayText { get { return _text; } }
    public int NextID { get { return _beatId; } }
    public bool IsAvailable { get { return _isAvailable; } }
    public bool InvokesEvent { get { return _invokeEvent != -1; } }
    public int InvokeEventID { get { return _invokeEvent; } }

}
