using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageUI : MonoBehaviour
{
    [SerializeField]
    private Image _damageOverlay; 

    public void UpdateHealth(float health, float maxHealth)
    {
        _damageOverlay.color = new Color(_damageOverlay.color.r, _damageOverlay.color.g, _damageOverlay.color.b, 1 - health / maxHealth);
    }
}
