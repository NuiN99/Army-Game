using Packages.Rider.Editor.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFinder : MonoBehaviour
{
    public BasicStats basicStats;
    [NonSerialized] public Transform currentTarget;
    void Start()
    {
        StartCoroutine(FindNearestTarget());
    }

    //circle cast every searchInterval, get closest enemy in cast, then change state
    public IEnumerator FindNearestTarget()
    {
        while (true)
        {
            yield return new WaitForSeconds(basicStats.SearchInterval);

            RaycastHit2D[] targetFinder = Physics2D.CircleCastAll(transform.position, basicStats.SearchRange, Vector3.zero, 0, basicStats.searchMask);
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
