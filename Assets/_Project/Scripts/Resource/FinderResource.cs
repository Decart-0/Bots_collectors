using System.Collections.Generic;
using UnityEngine;

public class FinderResource : MonoBehaviour
{
    [SerializeField] private BaseResources _baseResources;

    public Resource FindNearestResource()
    {
        IReadOnlyList<Resource> resources = _baseResources.GetResources();
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

        _baseResources.BorrowResource(closestResource);

        return closestResource;
    }

    private float CalculateSqrDistance(Resource resource)
    {
        return (resource.transform.position - transform.position).sqrMagnitude;
    }
}