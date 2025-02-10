using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("基本属性")]
    public float maxHealth;
    public float currentHealth;

    [Header("受伤无敌")]

    public float invulnerableDuration;
    private float invulnerableCounter;
    public bool invulnerable;
    public UnityEvent<Transform> OnTakeDamage;
    public UnityEvent OnDie;

    private void Start() {
        currentHealth = maxHealth;
    }

    private void Update() {
        if(invulnerable){
            invulnerableCounter -= Time.deltaTime; //减去计时器；
            if(invulnerableCounter <= 0 ){
                invulnerable = false;
            }
        }
    }

    private void TriggerInvulnerable()
    {
        if(!invulnerable)
        {
            invulnerable = true;
            invulnerableCounter = invulnerableDuration;
        }
    }
    public void TakeDamage(Attack attacker){
        if(invulnerable)
           return;
        if(currentHealth - attacker.damage > 0)
        {  
            currentHealth -= attacker.damage;
            TriggerInvulnerable();
            // 触发受伤
            OnTakeDamage?.Invoke(attacker.transform);

         
        }
        else
        {
            currentHealth = 0;
            //出发死亡
            OnDie.Invoke();

        }
        
    }
}
