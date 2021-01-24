using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : Interactable
{
    public bool isOn;
    public Light[] _lights;
    public Light _inverseLight;

    // Start is called before the first frame update
    void Start()
    {
        UpdateLights();
    }

    // Update is called once per frame
    protected override void Interact(Player player)
    {
        base.Interact(player);
        isOn = !isOn;

        UpdateLights();

    }

    void UpdateLights()
    {
        foreach (Light light in _lights)
        {
            if (light)
                light.enabled = isOn;
        }
        if (_inverseLight)
            _inverseLight.enabled = !isOn;
    }
}
