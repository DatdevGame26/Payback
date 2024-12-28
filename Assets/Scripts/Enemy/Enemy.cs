using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : LiveEntity
{
    [SerializeField] protected GameObject deathExplosion;
    protected ItemSpawner itemSpawner;

    [Header("Enemy Animator")]
    [SerializeField] protected float animationSpeed = 1;
    [SerializeField] protected Animator animator;

    [Header("ENemy: Negative Effect")]
    [SerializeField] protected GameObject negativeEffectObject;
    [SerializeField] protected float takeAdditionalDamagePercent = 0.5f;
    protected Transform playerTransform;
    protected float negativeTimer;
    protected bool hasNegativeEffect;


    protected override void Awake()
    {
        base.Awake();
        itemSpawner = GetComponent<ItemSpawner>();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();


        HandleNegativeEffect();
    }

    private void HandleNegativeEffect()
    {
        if (!hasNegativeEffect) return;

        hasNegativeEffect = negativeTimer > 0;
        negativeEffectObject.SetActive(hasNegativeEffect);
        negativeTimer -= Time.deltaTime;
    }

    public void setNegativeEffect()
    {
        hasNegativeEffect = true;
        negativeTimer = 3;
    }

    public override void Damage(int damage)
    {
        if (hasNegativeEffect)
        {
            int additionalDamage = Mathf.CeilToInt(damage * takeAdditionalDamagePercent);
            int totalDamage = damage + additionalDamage;
            base.Damage(totalDamage);
        }
        else
        {
            base.Damage(damage);
        }
    }

    protected bool isStopAction()
    {
        return playerTransform == null || isDead;
    }
}
