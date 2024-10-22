using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LiveEntity : MonoBehaviour, IDamageable
{
    [SerializeField] protected int maxHealth = 100;
    protected int currentHealth;
    protected bool isDead;


    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
    }


    public virtual void Damage(int damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0 && !isDead) { 
            doSomethingThenDie();
        }
    }

    public virtual void doSomethingThenDie()
    {
        isDead = true;
    }


    public void Die()
    {
        Destroy(gameObject);
    }

    public int getMaxHealth()
    {
        return maxHealth;
    }

    public int getCurrentHealth()
    {
        return currentHealth;
    }

}
