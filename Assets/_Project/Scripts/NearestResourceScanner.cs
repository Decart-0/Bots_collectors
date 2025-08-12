using System.Collections.Generic;
using UnityEngine;

public class NearestResourceScanner : MonoBehaviour
{
    public Resource SendResource(Dictionary<Resource, bool> resources)
    {
        float minSqrDistance = float.MaxValue;
        Resource closestResource = null;

        foreach (KeyValuePair<Resource, bool> resource in resources)
        {
            if (resource.Value == true) 
            {
                float sqrDistance = CalculateSqrDistance(resource.Key);

                if (sqrDistance < minSqrDistance)
                {
                    minSqrDistance = sqrDistance;
                    closestResource = resource.Key;
                }
            }
        }

        if (closestResource != null)
        {
            resources[closestResource] = false;
        }

        return closestResource;
    }

    private float CalculateSqrDistance(Resource resource)
    {
        return (resource.transform.position - transform.position).sqrMagnitude;
    }
}