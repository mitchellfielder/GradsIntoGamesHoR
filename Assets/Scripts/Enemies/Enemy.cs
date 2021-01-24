using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
     protected float _health = 100;

     public virtual float Damage(float damage)
     {
         _health -= damage;
         if(_health < 0)
             Death();
         return damage;
     }

     protected virtual void Death()
     {

     }


}
