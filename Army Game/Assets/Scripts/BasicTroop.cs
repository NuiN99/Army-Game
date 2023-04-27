using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class BasicTroop : MonoBehaviour
{
    Transform currentTarget;
    Rigidbody2D rb;
    public BasicStats troopStats;



    bool isAttacking = false;
    enum CurrentState
    {
        WAITING,
        SEARCHING,
        GOING,
        ATTACKING
    }
    [SerializeField] CurrentState currentState;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(FindNearestTarget());
    }

    private void FixedUpdate()
    {
        UpdateState();
        ActOnState();
        rb.velocity = Vector3.zero;
    }

    void GoToTarget()
    {
        Vector2 targetDir = (currentTarget.transform.position - transform.position).normalized;
        rb.MovePosition((Vector2)transform.position + targetDir * troopStats.MoveSpeed);
    }

    void ActOnState()
    {
        switch (currentState)
        {
            case CurrentState.GOING:
                GoToTarget();
                break;

            case CurrentState.SEARCHING:

                break;
            case CurrentState.WAITING:

                break;
        }
    }

    void UpdateState()
    {
        if (currentTarget != null)
        {
            float distFromTarget = Vector2.Distance(currentTarget.transform.position, gameObject.transform.position);
            if (distFromTarget < troopStats.AtkRange && !isAttacking)
            {
                currentState = CurrentState.ATTACKING;
                StartCoroutine(AttackTarget());
            }
            else if (distFromTarget > troopStats.AtkRange)
            {
                currentState = CurrentState.GOING;
            }
            else if (distFromTarget > troopStats.DeAggroRange)
            {
                currentState = CurrentState.SEARCHING;
            }
        }
        else
        {
            currentState = CurrentState.SEARCHING;
        }
    }

    //circle cast every searchInterval, get closest enemy in cast, then change state
    IEnumerator FindNearestTarget()
    {
        while (true)
        {
            RaycastHit2D[] targetFinder = Physics2D.CircleCastAll(transform.position, troopStats.SearchRange, Vector3.zero, 0, troopStats.searchMask);
            float closestTargetDist = 999;
            foreach (RaycastHit2D target in targetFinder)
            {
                float dist = Vector2.Distance(target.collider.gameObject.transform.position, transform.position);
                if (dist < closestTargetDist)
                {
                    currentTarget = target.collider.gameObject.transform;
                    closestTargetDist = dist;
                }
            }
            yield return new WaitForSeconds(troopStats.SearchInterval);
        }
    }

    IEnumerator AttackTarget()
    {
        isAttacking = true;
        if (currentTarget.TryGetComponent<Health>(out var health))
        {
            health.TakeDamage(troopStats.AtkDamage);
            print("HIT");   
        }
        yield return new WaitForSeconds(troopStats.AtkSpeed);
        isAttacking = false;
    }
}
