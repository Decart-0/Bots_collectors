using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private List<Unit> _units = new List<Unit>();
    [SerializeField] private CounterResources _counterResources;
    [SerializeField] private FinderResource _nearestResourceScanner;
    [SerializeField] private SpawnerResources _spawnerResources;
    [SerializeField] private float _scanInterval = 0.5f;

    private void Start()
    {
        StartCoroutine(SendingBotsRoutine());
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

    public void GetUnit(Unit unit) 
    {
        _units.Add(unit);

        if (unit.TryGetComponent(out DetectorResource detectorResource))
        {
            detectorResource.ChangeTarget += OnUnitTargetChange;
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

    private IEnumerator SendingBotsRoutine()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(_scanInterval);

        while (true)
        {
            yield return waitForSeconds;

            Unit freeUnit = GetUnit();

            if (freeUnit != null)
            {
                AppointResource(freeUnit);
            }
        }
    }
}