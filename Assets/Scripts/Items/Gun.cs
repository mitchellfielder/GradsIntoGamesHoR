using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : PrimaryItem
{

    [SerializeField]
    private AudioClip[] _fireSoundEffects;

    private AudioSource _audioSource;
    [SerializeField]
    private Light _muzzleFlashLight;
    [SerializeField]
    private GameObject _bulletTrail;
    private GameObject _playerCamera;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

     
    protected override void Interact(Player player)  //abstract
    {
        Debug.Log("Setting player camera");

        _playerCamera = player.gameObject.GetComponentInChildren<Camera>().gameObject;
        OnInteract.Invoke();
    }

    public override void Use(Player player)
    {
        if(!_playerCamera)
            Debug.Log("No player camera set");


        Debug.Log("BANG");
        _audioSource.clip = _fireSoundEffects[Random.Range(0, _fireSoundEffects.Length)];
        _audioSource.Play();
        StopAllCoroutines();
        StartCoroutine(DoMuzzleFlash());
        GameObject bulletTrail = Instantiate(_bulletTrail) as GameObject;
        bulletTrail.transform.position = _muzzleFlashLight.gameObject.transform.position;
        bulletTrail.transform.rotation = gameObject.transform.rotation;

        RaycastHit hit;

        if (Physics.Raycast(_playerCamera.transform.position, _playerCamera.transform.forward, out hit, Mathf.Infinity))
        {

            //Create bullet trail SFX
            bulletTrail.transform.localScale = new Vector3(1, 1, Vector3.Distance(hit.point, _muzzleFlashLight.gameObject.transform.position));
            bulletTrail.transform.forward = hit.point - _muzzleFlashLight.gameObject.transform.position;

            Enemy hitEnemy = hit.collider.gameObject.GetComponentInParent<Enemy>();

            if (hitEnemy)   //Damaging enemy
            {
                hitEnemy.Damage(30);
            }

        }



    }

    IEnumerator DoMuzzleFlash()
    {
        _muzzleFlashLight.intensity = 1.0f;
        yield return  new WaitForSeconds(0.05f);
        const float speed = 20.0f;
        float brighness = 1.0f;
        do
        {
            brighness -= Time.deltaTime * speed;
            _muzzleFlashLight.intensity = brighness;
            yield return null;

        } while (brighness > 0);


    }



}
