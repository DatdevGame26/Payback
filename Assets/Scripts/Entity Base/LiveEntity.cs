using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/* Đây là lớp thực thể sống, tức đối tượng sẽ có máu và có thể bị thương và bị tiêu diệt
 * Các đối tượng như là người chơi hay kẻ thù đều là thực thể sống và kế thừa từ lớp này
 */

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

    public bool IsDead() { return isDead; }

}
