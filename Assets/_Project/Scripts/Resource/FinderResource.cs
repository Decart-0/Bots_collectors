using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Base))]
public class FinderResource : MonoBehaviour
{
    private Base _base;

    private void Awake()
    {
        _base = GetComponent<Base>();
    }

    public Resource FindNearestResource()
    {
        IReadOnlyList<Resource> resources = _base.GetResources();
        Resource closestResource = null;
        float minSqrDistance = float.MaxValue;

        foreach (Resource resource in resources)
        {
            float sqrDistance = CalculateSqrDistance(resource);

            if (sqrDistance < minSqrDistance)
            {
                minSqrDistance = sqrDistance;
                closestResource = resource;
            }
        }

        _base.BorrowResource(closestResource);

        return closestResource;
    }

    private float CalculateSqrDistance(Resource resource)
    {
        return (resource.transform.position - transform.position).sqrMagnitude;
    }
}