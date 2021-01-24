using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class CrawlingEnemy : Enemy
{
    [SerializeField] private Player _targetPlayer;
    private NavMeshAgent _navAgent;
    private float _strikeDistance = 4.0f;
    const float _slowSpeed = 1.0f;
    const float _fastSpeed = 2.0f;
    private bool _isAggressive = false;
    private bool _isPassive = false;
    private Animator _animator;


    private bool _striking = false;
    [SerializeField]
    private Transform _secondEncounterLocation;
    [SerializeField]
    private GameObject _secondEncounterConversations;
    [SerializeField]
    private AudioClip _deathSFX;
    [SerializeField]
    private AudioClip _strikeSFX;
    [SerializeField]
    private AudioSource _breathingAudioSource;
    [SerializeField]
    private AudioSource _speechAudioSource;


    // Start is called before the first frame update
    void Start()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }



    // Update is called once per frame
    void Update()
    {
        if(_navAgent.enabled)
            _navAgent.SetDestination(_targetPlayer.transform.position);



        if (!_striking && IsInRange())
        {
            Strike();
        }
      
    }

    //The character wants to strike the player
    private void Strike()
    {
        if(_isPassive)
            return;
        _animator.SetTrigger("Strike");
        _navAgent.speed = 0.0f;
        _speechAudioSource.clip = _strikeSFX;
        _speechAudioSource.Play();
        _striking = true;
    }

    //Character will move towards the player faster
    public void MakeAggressive()
    {
        Debug.Log("Aggressive");
        _navAgent.speed = _fastSpeed;
        _isAggressive = true;
        _animator.SetBool("Aggressive",true);

    }

    //The impact frame of the strike animation
    public void OnAnimStrikeImpact()
    {
        if (IsInRange())            //TODO change to a hit box in future
        {
            _targetPlayer.Damage(40.0f);
        }

    }

    public override float Damage(float damage)
    {
        MakeAggressive();
        if(_secondEncounterConversations)
            _secondEncounterConversations.SetActive(false); //you cannot speak to the character after hurting it
        Game.game.StopConversation();                       //specifically used here for this character as it is in a heavily scripted event 
        return base.Damage(damage);
    }

    //Called when strike is finished animating
    public void OnAnimContinueMoving()
    {
        if (!IsInRange())
        {
            _navAgent.speed = 1.0f;
            _striking = false;
        }
        else
        {
            Strike();
        }
    }

    protected override void Death()
    {
        _animator.SetBool("Alive", false);
        foreach (LookAtConstraint lookAtConstraint in GetComponentsInChildren<LookAtConstraint>())
        {
            StartCoroutine(DoRemoveLookAtConstraint(lookAtConstraint));
        }

        _breathingAudioSource.Stop();
        _speechAudioSource.clip = _deathSFX;
        _speechAudioSource.Play();

        _navAgent.enabled = false;
        base.Death();
        this.enabled = false;
    }

    //Will no longer strike the player
    public void MakePassive()
    {
        _isPassive = true;
    }


    bool IsInRange()
    {
        return Vector3.Distance(_targetPlayer.transform.position, transform.position) < _strikeDistance;
    }

    //Slow blend removing the look at components
    IEnumerator DoRemoveLookAtConstraint(LookAtConstraint lookAtConstraint)
    {
        const float speed = 2.0f;
        float startPosition = lookAtConstraint.weight;
        float currentPosition = startPosition;

        while (currentPosition > 0.0f)
        {
            currentPosition -= Time.deltaTime * speed;
            lookAtConstraint.weight = currentPosition;

            yield return null;
        }
    }

    //Called to position the character after gun is found
    public void SetupSecondEncounter()
    {

    }

}
