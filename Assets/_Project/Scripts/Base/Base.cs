using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private CounterResources _counterResources;
    [SerializeField] private FinderResource _nearestResourceScanner;
    [SerializeField] private Transform _unitsTransform;
    [SerializeField] private SpawnerResources _spawnerResources;

    [SerializeField] private List<Unit> _units = new List<Unit>();

    private void Start()
    {
        InitializeUnits();
    }

    private void Update()
    {
        Unit freeUnit = GetUnit();

        if (freeUnit != null)
        {
            AppointResource(freeUnit);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Resource resource))
        {
            _counterResources.AddScore(resource.Value);
            _spawnerResources.ReleaseResource(resource);
        }
    }

    private Unit GetUnit()
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

    private void AppointResource(Unit unit)
    {
        Resource resource = _nearestResourceScanner.FindNearestResource();

        if (unit != null && resource != null)
        {
            unit.Active(resource.transform);
        }
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