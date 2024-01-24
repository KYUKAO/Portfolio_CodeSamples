using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagableComponent : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(DamageSystem.Health);
        if(other.GetComponent<EnemyInputComponent>())
        {
            Evently.Instance.Publish(new DamageEvent(this));
            if(DamageSystem.Health<=0)
            {
                Evently.Instance.Publish(new GameOverEvent(false));
                DamageSystem.Health = DamageSystem.MaxHealth;
            }
        }
    }
}