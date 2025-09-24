using System.Collections.Generic;
using UnityEngine;

public class FinderResource : MonoBehaviour
{
    private ListResources _listResources;

    private void Awake()
    {
        _listResources = FindAnyObjectByType<ListResources>();
    }

    public Resource FindNearestResource()
    {
        IReadOnlyList<Resource> resources = _listResources.GetResources();
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

        _listResources.BorrowResource(closestResource);

        return closestResource;
    }

    private float CalculateSqrDistance(Resource resource)
    {
        return (resource.transform.position - transform.position).sqrMagnitude;
    }
}