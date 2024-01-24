using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageSystem:MonoBehaviour
{
    public int Damage;
    public static int Health;
    public static int MaxHealth = 100;
    public Text HealthText;
    private void OnEnable()
    {
        Health = MaxHealth;
        Evently.Instance.Subscribe<DamageEvent>(OnDamaged);
    }
    private void Update()
    {
        HealthText.text="Health:"+Health;
    }
    private void OnDisable()
    {
        Evently.Instance.Unsubscribe<DamageEvent>(OnDamaged);
    }
    private void OnDamaged(DamageEvent damageEvent)
    {
        Health -= Damage ;
    }
}