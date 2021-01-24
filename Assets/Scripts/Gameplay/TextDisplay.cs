using System.Collections;
using System.Linq;
using UnityEngine;
using TMPro;

public class TextDisplay : MonoBehaviour
{
    public enum State { Initialising, Idle, Busy, AwaitingInput, Disabled }

    private TMP_Text _displayText;
    [SerializeField]
    private TMP_Text [] _decisionText;
    [SerializeField]
    private TMP_Text _textHistory;

    private string _displayString;
    private WaitForSeconds _shortWait;
    private WaitForSeconds _longWait;
    [SerializeField]

    public State _state = State.Initialising;

    public bool IsIdle { get { return _state == State.Idle; } }
    public bool IsBusy { get { return _state != State.Idle; } }
    public bool IsDisabled { get { return _state == State.Disabled; } }
    public bool IsAwaitingInput { get { return _state == State.AwaitingInput; } }

    private void Awake()
    {
        _displayText = GetComponent<TMP_Text>();
        _shortWait = new WaitForSeconds(0.01f);
        _longWait = new WaitForSeconds(0.8f);

        _displayText.text = string.Empty;

        Disable();
    }

    private IEnumerator DoShowText(string text)
    {
        int currentLetter = 0;
        char[] charArray = text.ToCharArray();

        while (currentLetter < charArray.Length)
        {
            _displayText.text += charArray[currentLetter++];
            yield return _shortWait;
        }

        _displayText.text += "\n";
        _displayString = _displayText.text;
        _state = State.Idle;
    }

    private IEnumerator DoShowDecisionText(int decisionID, string text)
    {

        int currentLetter = 0;
        char[] charArray = text.ToCharArray();

        while (currentLetter < charArray.Length)
        {
            _decisionText[decisionID].text += charArray[currentLetter++];
            yield return _shortWait;
        }

        _decisionText[decisionID].text += "\n";
        // _displayString += _decisionText[decisionID].text;
        _state = State.Idle;   
    }

    private IEnumerator DoAwaitingInput()
    {

        bool on = true;

        while (enabled)
        {
            //_displayText.text = string.Format( "{0}> {1}", _displayString, ( on ? "|" : " " ));
            on = !on;
            yield return _longWait;
        }
    }

    private IEnumerator DoClearDecisionText(int selectedResponse)
    {

        bool isAllTextCleared = false;
        while (!isAllTextCleared)
        {
            isAllTextCleared = true;


            for (int i = 0; i < _decisionText.Length; i++)
            {
                if (i == selectedResponse)
                    continue;
                if (_decisionText[i].text.Length > 0)
                {
                    isAllTextCleared = false;
                    _decisionText[i].text = _decisionText[i].text.Substring(0, _decisionText[i].text.Length - 1);
                }
            }

            yield return null;

        }
        yield return _longWait;

        StartCoroutine(DoClearText());

    }

    private IEnumerator DoClearText()
    {


        while (_displayText.text.Length > 0)
        {
            _textHistory.text += _displayText.text[0];
            _displayText.text = _displayText.text.Substring(1, _displayText.text.Length - 1);
            yield return null;

        }


        bool isAllTextCleared = false;
        while (!isAllTextCleared)
        {
            isAllTextCleared = true;
            

            for (int i = 0; i < _decisionText.Length; i++)
            {
                if (_decisionText[i].text.Length > 0)
                {
                    isAllTextCleared = false;
                    _textHistory.text += _decisionText[i].text[0];
                    _decisionText[i].text = _decisionText[i].text.Substring(1, _decisionText[i].text.Length - 1);
                }
            }

            yield return null;

        }
        _state = State.Idle;

    }

    public void Display(string text)
    {
        if (_state == State.Idle)
        {
            StopAllCoroutines();
            _state = State.Busy;
            StartCoroutine(DoShowText(text));
        }
    }

    public void DisplayDecision(int decisionID, string text)
    {
        if (_state == State.Idle)
        {
            StopAllCoroutines();
            _state = State.Busy;
            StartCoroutine(DoShowDecisionText(decisionID, text));
        }
    }

    public void WaitForInput()
    {
        if (_state == State.Idle)
        {
            StopAllCoroutines();
            _state = State.AwaitingInput;
            //StartCoroutine(DoAwaitingInput());
        }
    }

    public void Clear(int lastDecision)
    {
        if (_state == State.Idle || _state == State.AwaitingInput)
        {
            StopAllCoroutines();
            _state = State.Busy;
            StartCoroutine(DoClearDecisionText(lastDecision));
        }
    }

    public void Enable()
    {
        _state = State.Idle;
        SetVisible(true);
    }
    public void Disable()
    {

        StopAllCoroutines();
        _state = State.Disabled;
        SetVisible(false);
    }

    void SetVisible(bool bIsVisible)
    {

        _displayText.enabled = bIsVisible;
        foreach (var decision in _decisionText)
        {
            decision.gameObject.transform.parent.gameObject.SetActive(bIsVisible);
        }
        _textHistory.enabled = bIsVisible;

    }
    /*
    public void ShowUI()
    {
        _displayText.enabled = true;
        foreach (var decision in _decisionText)
        {
            decision.enabled = true;
            decision.gameObject.transform.parent.gameObject.SetActive(true);
        }
        _textHistory.enabled = true;
    }*/
}
