using Packages.Rider.Editor.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFinder : MonoBehaviour
{
    TroopBehaviour statsGetter;
    [NonSerialized] public Transform currentTarget;
    void Start()
    {
        TryGetComponent(out statsGetter);

        if (statsGetter == null) return;
        StartCoroutine(FindNearestTarget());
    }

    //circle cast every searchInterval, get closest enemy in cast, then change state
    public IEnumerator FindNearestTarget()
    {
        while (true)
        {
            yield return new WaitForSeconds(statsGetter.troopStats.SearchInterval);

            RaycastHit2D[] targetFinder = Physics2D.CircleCastAll(transform.position, statsGetter.troopStats.SearchRange, Vector3.zero, 0, statsGetter.troopStats.searchMask);
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
        }
    }
}
