using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private CounterResources _counterResources;
    [SerializeField] private FinderResource _nearestResourceScanner;
    [SerializeField] private Transform _unitsTransform;
    [SerializeField] private SpawnerResources _spawnerResources;
    [SerializeField] private List<Unit> _units = new List<Unit>();
    [SerializeField] private float _scanInterval = 0.5f;

    private void Start()
    {
        InitializeUnits();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Resource resource))
        {
            _counterResources.AddScore(resource.Value);
            _spawnerResources.ReleaseResource(resource);
        }
    }

    private void OnDestroy()
    {
        foreach (Unit unit in _units)
        {
            if (unit.TryGetComponent(out DetectorResource detectorResource))
            {
                detectorResource.ChangeTarget -= OnUnitTargetChange;
            }
        }
    }

    private Unit GetUnit()
    {
        foreach (Unit unit in _units)
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

        if (resource != null)
        {
            ChangeUnitTarget(resource.transform, unit);
        }
    }

    private void OnUnitTargetChange(Unit unit)
    {
        ChangeUnitTarget(transform, unit);
    }

    private void ChangeUnitTarget(Transform target, Unit unit)
    {
        unit.Active(target);
    }

    private void InitializeUnits()
    {
        foreach (Transform transform in _unitsTransform)
        {
            if (transform.TryGetComponent(out Unit unit))
            {
                if (unit.TryGetComponent(out DetectorResource detectorResource))
                {
                    _units.Add(unit);
                    detectorResource.ChangeTarget += OnUnitTargetChange;
                }
            }
        }

        StartCoroutine(CheckForResourcesRoutine());
    }

    private IEnumerator CheckForResourcesRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_scanInterval);

            Unit freeUnit = GetUnit();

            if (freeUnit != null)
            {
                AppointResource(freeUnit);
            }
        }
    }
}