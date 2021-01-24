using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{

    private float _yaw = 0.0f;
    private float _pitch = 0.0f;
    private float _speed = 2.0f;
    private float _runSpeed = 4.0f;

    private float lookSpeed = 1.0f;
    private float _meshRotationSpeed = 15.0f;
    private bool _bIsRunning = false;

    private Camera _camera;

    private Camera GetCamera()
    {
        return _camera;
    }

    private Rigidbody _rigidbody;
    [SerializeField] private GameObject _mesh;

    private Interactable _hoveredOverInteractable;
    public bool _isLockedInCoversation = false;

    private Vector2 _movementInput;

    private SupportItem _supportItem;
    private PrimaryItem _primaryItem;

    [SerializeField] private GameObject _supportItemSocket;
    [SerializeField] private GameObject _primaryItemSocket;

    private float _footStepTracker;
    private AudioSource _footStepAudioSource;
    [SerializeField] private AudioClip[] _footStepSounds;

    [SerializeField] private DamageUI _damageUI;
    [SerializeField] private GameObject _pauseUI;
    [SerializeField] private GameObject _deathUI;
    private float _maxHealth = 100.0f;
    private float _health = 100.0f;

    public GameObject GetMesh()
    {
        return _mesh;
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _camera = GetComponentInChildren<Camera>();
        _rigidbody = GetComponent<Rigidbody>();
        _footStepAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForInteractables();
        HandleInput();

        if (_isLockedInCoversation)
        {
            ConversationUpdate();
        }
        else
        {
            UpdateRigidBody();
        }

        UpdateCamera();
        UpdateMesh();

        UpdateFootstepSounds();

    }



    Vector3 _positionLastFrame;

    void UpdateFootstepSounds()
    {
        _footStepTracker -= Vector3.Distance(transform.position, _positionLastFrame);
        if (_footStepTracker < 0)
        {

            _footStepTracker = 1.3f;
            _footStepAudioSource.clip = _footStepSounds[Random.Range(0, _footStepSounds.Length)];
            _footStepAudioSource.Play();
        }

        _positionLastFrame = transform.position;
    }



    void ConversationUpdate()
    {
        const float stoppingSpeed = 5.0f;
        _rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity, Vector3.zero, Time.deltaTime * stoppingSpeed);
    }



    private IEnumerator DoQuickTurn()
    {
        float rotationLeft = 180.0f;
        const float rotationAmount = 180.0f * 5.0f;
        while (rotationLeft > 0.0f)
        {
            rotationLeft -= rotationAmount * Time.deltaTime;
            _yaw += rotationAmount * Time.deltaTime;
            yield return null;
        }

        yield return null;

    }


    void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _pauseUI.SetActive(true);
        Time.timeScale = 0;
    }


void HandleInput()
    {

        if(Time.timeScale == 0)
            return;
       
        _pitch -= Input.GetAxis("Mouse Y") * lookSpeed + Input.GetAxis("Joystick Look Y") * lookSpeed* Time.timeScale;
        _pitch = Mathf.Clamp(_pitch, -90, 90);
        _yaw += Input.GetAxis("Mouse X") * lookSpeed + Input.GetAxis("Joystick Look X") * lookSpeed * Time.timeScale;


        _movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (_movementInput.magnitude > 1.0) //stop diagonal movement being so fast
            _movementInput = _movementInput.normalized;



        if (Input.GetButtonDown("Run") && Input.GetAxis("Vertical") < -0.5f)
            StartCoroutine(DoQuickTurn());

        if (Input.GetButtonDown("Interact"))
            Interact();




        if (Input.GetButtonDown("Fire"))
        {
            if (_primaryItem)
                
                _primaryItem.Use(this);




        }


        if (Input.GetKeyDown(KeyCode.Escape))
            Pause();

        if (Input.GetButtonDown("Torch") && _supportItem)
            _supportItem.Use(this);
        
        _bIsRunning = Input.GetButton("Run");
    }



    void UpdateCamera()
    {
        transform.eulerAngles = new Vector3(0.0f, _yaw, 0.0f);
        _camera.transform.eulerAngles = new Vector3(_pitch, _yaw, 0.0f);
    }



    void UpdateRigidBody()
    {

        _rigidbody.velocity = transform.forward * _movementInput.y * ((_bIsRunning) ? _runSpeed : _speed) +
                              transform.right * _movementInput.x * _speed +
                              new Vector3(0, _rigidbody.velocity.y, 0);


    }


    void CheckForInteractables()
    {
        RaycastHit hit;
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, Mathf.Infinity))
        {
            _hoveredOverInteractable = hit.collider.gameObject.GetComponent<Interactable>();

            Debug.DrawRay(_camera.transform.position, _camera.transform.forward * hit.distance, Color.yellow);
        }

    }

    public void StartConversation()
    {
        _isLockedInCoversation = true;
    }
    public void EndConversation()
    {
        _isLockedInCoversation = false;
    }

    public void EquipSupportItem(SupportItem supportItem)
    {
        _supportItem = supportItem;
        _supportItem.transform.parent = _supportItemSocket.transform;
        _supportItem.transform.localPosition = Vector3.zero;
        _supportItem.transform.localRotation = Quaternion.identity;
        _supportItem.Use(this);
    }
    public void EquipPrimaryItem(PrimaryItem primaryItem)
    {
        _primaryItem = primaryItem;
        _primaryItem.transform.parent = _primaryItemSocket.transform;
        _primaryItem.transform.localPosition = Vector3.zero;
        _primaryItem.transform.localRotation = Quaternion.identity;
    }
    void Interact()
    {
        if (!_hoveredOverInteractable)
            return;


        SupportItem supportItem = _hoveredOverInteractable as SupportItem;
        if (supportItem)
            EquipSupportItem(supportItem);

        PrimaryItem primaryItem= _hoveredOverInteractable as PrimaryItem;
        if (primaryItem)
            EquipPrimaryItem(primaryItem);

        _hoveredOverInteractable.OnClick(this);
        

    }
    
    public void Damage(float damage)
    {
        _health -= damage;
        _damageUI.UpdateHealth(_health,_maxHealth);
        if(_health < 0.0f)
            Death();
    }

    private void Death()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        this.enabled = false;
        _deathUI.SetActive(true);
        Time.timeScale = 0;
    }

    void UpdateMesh()
    {
         if (!_mesh)
            return;

        _mesh.transform.position = _camera.transform.position;
        _mesh.transform.rotation = Quaternion.Lerp(_mesh.transform.rotation, _camera.transform.rotation, _meshRotationSpeed * Time.deltaTime);

    }

}
