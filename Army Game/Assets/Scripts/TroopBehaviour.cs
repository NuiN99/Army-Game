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

    [SerializeField] float rotationSpeed;
    [SerializeField] LayerMask allyTroopMask;

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

        rb.velocity = Vector3.zero;
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

        RotateToDirection(targetDir);
        AvoidWalls();
    }

    //could rotate towards velocity instead, making it independant of target
    void RotateToDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, transform.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }

    void AvoidWalls()
    {
        RaycastHit2D rayHit = Physics2D.Raycast(transform.position, transform.right, 2.5f, allyTroopMask);
        Debug.DrawRay(transform.position, transform.right, Color.yellow);
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
