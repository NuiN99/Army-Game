using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    Rigidbody2D rb;
    public BasicStats enemyStats;
    TargetFinder targetFinder;

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
        TryGetComponent(out targetFinder);
    }

    private void FixedUpdate()
    {
        UpdateState();
        ActOnState();
        rb.velocity = Vector3.zero;
    }

    void GoToTarget()
    {
        Vector2 targetDir = (targetFinder.currentTarget.transform.position - transform.position).normalized;
        rb.MovePosition((Vector2)transform.position + targetDir * enemyStats.MoveSpeed);
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
        if (targetFinder.currentTarget != null)
        {
            float distFromTarget = Vector2.Distance(targetFinder.currentTarget.transform.position, gameObject.transform.position);
            if (distFromTarget < enemyStats.AtkRange && !isAttacking)
            {
                currentState = CurrentState.ATTACKING;
                StartCoroutine(AttackTarget());
            }
            else if (distFromTarget > enemyStats.AtkRange)
            {
                currentState = CurrentState.GOING;
            }
            else if (distFromTarget > enemyStats.DeAggroRange)
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

    IEnumerator AttackTarget()
    {
        isAttacking = true;
        if (targetFinder.currentTarget.TryGetComponent<Health>(out var health))
        {
            health.TakeDamage(enemyStats.AtkDamage);
        }
        yield return new WaitForSeconds(enemyStats.AtkSpeed);
        isAttacking = false;
    }
}
