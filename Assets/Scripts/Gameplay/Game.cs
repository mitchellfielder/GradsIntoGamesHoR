using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Game : MonoBehaviour
{

    [SerializeField]
    private UnityEvent [] _invokeableUnityEvents;


    [SerializeField] private StoryData _data;
    public static Game game ;
    private TextDisplay _output;
    private BeatData _currentBeat;
    private WaitForSeconds _wait;
    private int _lastDecision;
    [SerializeField]
    private Player _player; 
    private void Awake()
    {
        _output = GetComponentInChildren<TextDisplay>();
        _currentBeat = null;
        _wait = new WaitForSeconds(0.5f);
        game = this;
        this.enabled = false;

    }

    private void Update()
    {

        if(_output.IsAwaitingInput)
        {
            if (_currentBeat == null)
            {
                DisplayBeat(1);
            }
            else
            {
                UpdateInput();
            }
        }
    }

    public void StopConversation()
    {

        _lastDecision = -1;
        StartCoroutine(DoConversationEnd());
    }

    public bool StartConversation(StoryData data, bool isPlayerFreezing = true, int startingBeat = 1)
    {
        if (!_output.IsDisabled) //Still In Conversation
        {

            return false;
        }
        if(isPlayerFreezing)
            _player.StartConversation();


        _data = data;
        _output.Enable();
        DisplayBeat(startingBeat);

        this.enabled = true;
        return true;

    }

    private void UpdateInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(_currentBeat != null)
            {
                if (_currentBeat.ID == 1)
                {
                    Application.Quit();
                }
                else
                {
                    DisplayBeat(1);
                }
            }
        }


        if (_currentBeat.Decision.Count == 0)
        {
            StopConversation();
        }
        else
        {
            

            bool madeChoice = false;
            KeyCode alpha = KeyCode.Alpha1;
            KeyCode keypad = KeyCode.Keypad1;

            for (int count = 0; count < _currentBeat.Decision.Count; ++count)
            {
                if (alpha <= KeyCode.Alpha9 && keypad <= KeyCode.Keypad9)
                {
                    if (Input.GetKeyDown(alpha) || Input.GetKeyDown(keypad))
                    {
                        _lastDecision = count;
                        madeChoice = true;
                        break;
                    }
                }
                ++alpha;
                ++keypad;
            }

            if (Input.GetAxis("D-Pad Y") > 0.0f)
            {
                _lastDecision = 0;
                madeChoice = true;
            }
            if (Input.GetAxis("D-Pad X") < 0.0f)
            {
                _lastDecision = 1;
                madeChoice = true;
            }
            if (Input.GetAxis("D-Pad X") > 0.0f)
            {
                _lastDecision = 2;
                madeChoice = true;
            }

            if (Input.GetAxis("D-Pad Y") < 0.0f)
            {
                _lastDecision = 3;
                madeChoice = true;
            }




            if (madeChoice && _lastDecision < _currentBeat.Decision.Count)
            {
                ChoiceData choice = _currentBeat.Decision[_lastDecision];
                DisplayBeat(choice.NextID);
                if (choice.InvokesEvent)
                {
                    _invokeableUnityEvents[choice.InvokeEventID].Invoke();
                }
            }

        }






    }



    private void DisplayBeat(int id)
    {

        BeatData data = _data.GetBeatById(id);
        if (data == null)
        {
            StartCoroutine(DoConversationEnd());

            return;
        }
        StartCoroutine(DoDisplay(data));
        _currentBeat = data;
    }

    private IEnumerator DoConversationEnd()
    {
        _player.EndConversation();
        _output.Clear(_lastDecision);
        while (_output.IsBusy && !_output.IsDisabled)
        {
            yield return null;
        }
        _output.Disable();
        this.enabled = false;
    }

    private IEnumerator DoDisplay(BeatData data)
    {
        
        _output.Clear(_lastDecision);

        while (_output.IsBusy)
        {
            yield return null;
        }

        _output.Display(data.DisplayText);

        while(_output.IsBusy)
        {
            yield return null;
        }


        for (int count = 0; count < data.Decision.Count; ++count)
        {
            ChoiceData choice = data.Decision[count];
            _output.DisplayDecision(count, string.Format("{0}: {1}", (count + 1), choice.DisplayText));

            while (_output.IsBusy)
            {
                yield return null;
            }
        }

        _output.WaitForInput();

    }
}
