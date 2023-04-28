using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class TroopBehaviour : MonoBehaviour
{
    Rigidbody2D rb;
    public TroopStats troopStats;
    TargetFinder targetFinder;

    CurrentState currentState;
    enum CurrentState
    {
        WAITING,
        SEARCHING,
        GOING,
        ATTACKING
    }

    bool isAttacking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        TryGetComponent(out targetFinder);
    }

    private void FixedUpdate()
    {
        UpdateState();
    }

    void UpdateState()
    {
        if (targetFinder.currentTarget == null)
        {
            currentState = CurrentState.SEARCHING;
            return;
        }


        float distFromTarget = Vector2.Distance(targetFinder.currentTarget.transform.position, gameObject.transform.position);

        if (distFromTarget < troopStats.AtkRange)
        {
            currentState = CurrentState.ATTACKING;
        }
        else if (distFromTarget > troopStats.AtkRange)
        {
            currentState = CurrentState.GOING;
        }
        else if (distFromTarget > troopStats.DeAggroRange)
        {
            currentState = CurrentState.SEARCHING;
        }

        ActOnState();
    }

    void ActOnState()
    {
        switch (currentState)
        {
            case CurrentState.WAITING:
                break;
            case CurrentState.SEARCHING:
                break;

            case CurrentState.GOING:
                GoToTarget();
                break;

            case CurrentState.ATTACKING:
                if (isAttacking) return;
                StartCoroutine(AttackTarget());
                break;
        }
    }


    void GoToTarget()
    {
        Vector2 targetDir = (targetFinder.currentTarget.transform.position - transform.position).normalized;
        rb.MovePosition((Vector2)transform.position + targetDir * troopStats.MoveSpeed);
    }

    
    IEnumerator AttackTarget()
    {
        isAttacking = true;
        yield return new WaitForSeconds(troopStats.AtkSpeed);

        if (targetFinder.currentTarget != null && targetFinder.currentTarget.TryGetComponent<Health>(out var health))
        {
            health.TakeDamage(troopStats.AtkDamage);
            print("HIT");   
        }
        isAttacking = false;
    }
}
