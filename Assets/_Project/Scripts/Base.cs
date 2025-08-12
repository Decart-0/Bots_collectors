using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private DistributorResource _distributorResource;
    [SerializeField] private CounterResources _counterResources;
    [SerializeField] private Transform _unitsTransform;

    private List<Unit> _units = new List<Unit>();

    private void Awake()
    {
        InitializeUnits();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Resource resource))
        {
            _counterResources.AddScore(resource.Value);
            _distributorResource.DeleteResource(resource);
        }
    }

    public Unit GetUnit()
    {
        foreach(Unit unit in _units) 
        {
            if (unit.IsActive == false)
            {
                return unit;
            }
        }

        return null;
    }
    
    private void InitializeUnits()
    {
        foreach (Transform transform in _unitsTransform)
        {
            if (transform.TryGetComponent(out Unit unit)) 
            {
                _units.Add(unit);
            }
        }
    }
}