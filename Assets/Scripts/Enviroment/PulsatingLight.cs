using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsatingLight : MonoBehaviour
{

    public enum PulsatingLightStyle
    {
        Pulsating,
        Perlin,
        Random
    }
    public PulsatingLightStyle style;

    private Light _light;
    public float _minIntensity = 1.0f;
    public float _maxIntensity = 3.0f;
    public float _speed = 3.0f;

    public float _perlinThreshold = 0.1f;


    // Start is called before the first frame update
    void Start()
    {
        _light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (style)
        {
            case PulsatingLightStyle.Perlin:
                _light.intensity  = (Mathf.PerlinNoise(Time.time * _speed, 0.0f) > _perlinThreshold)? _maxIntensity : _minIntensity;
                
                //_light.intensity = Mathf.Lerp(_minIntensity, _maxIntensity, Mathf.PerlinNoise(Time.time * _speed,0.0f));
                break;
            case PulsatingLightStyle.Pulsating:
                _light.intensity = Mathf.Lerp(_minIntensity, _maxIntensity, Mathf.Sin(Time.time * _speed) * 0.5f + 0.5f);
                break;
            case PulsatingLightStyle.Random:
                _light.intensity = Mathf.Lerp(_minIntensity, _maxIntensity, Random.Range(0.0f,1.0f));

                break;
        }
    }
}
