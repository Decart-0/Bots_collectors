using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CounterResources))]
[RequireComponent(typeof(SpawnerUnit))]
public class Base : MonoBehaviour
{
    [SerializeField] private List<Unit> _units = new List<Unit>();
    [SerializeField] private FinderResource _nearestResourceScanner;
    [SerializeField] private float _scanInterval = 0.5f;

    private Flag _flag;
    private CounterResources _counterResources;
    private SpawnerUnit _spawnerUnit;

    public int NumberUnits => _units.Count;

    public bool IsSetFlag { get; private set; } 

    public int Id { get; private set; }

    public event Action<Vector3, Unit> SpawnNewBase;

    private void Awake()
    {
        _counterResources = GetComponent<CounterResources>();
        _spawnerUnit = GetComponent<SpawnerUnit>();

        IsSetFlag = false;
    }

    private void OnEnable()
    {
        _counterResources.SpawnUnitAllowed += SpawnUnit;
        _counterResources.CreatedNewBase += CreateNewBase;
    }

    private void Start()
    {
        StartCoroutine(SendingBotsRoutine());
    }

    private void OnDisable()
    {
        _counterResources.SpawnUnitAllowed -= SpawnUnit;
        _counterResources.CreatedNewBase -= CreateNewBase;
    }

    private void OnDestroy()
    {
        foreach (Unit unit in _units)
        {
            if (unit != null && unit.TryGetComponent(out DetectorResource detectorResource))
            {
                detectorResource.ChangeTarget -= OnUnitTargetChange;
            }
        }

        _units.Clear();
    }

    public void AssignId(int id) 
    {
        Id = id;
    }

    public void AssignFlag(Flag flag)
    {
        _flag = flag;
        UpdateStatusFlag(true);
    }

    public void RearrangeFlag(Vector3 position) 
    {
        _flag.transform.position = position;
    }

    public void SetUnit(Unit unit) 
    {
        if (unit == null) return;

        if (unit.TryGetComponent(out DetectorResource detectorResource))
        {
            detectorResource.ChangeTarget += OnUnitTargetChange;
        }

        unit.AssignId(Id);
        _units.Add(unit);
    }

    private void CreateNewBase() 
    {
        if (_units.Count <= 0) return;

        Unit unit = _units[0];
        _units.RemoveAt(0);

        _flag?.ReturnToPool();
        UpdateStatusFlag(false);

        SpawnNewBase?.Invoke(_flag.transform.position, unit);
        _counterResources.SubtracteCostSpawnBase();
    }

    private void UpdateStatusFlag(bool status)
    {
        IsSetFlag = status;
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

    private void SpawnUnit() 
    {
        _spawnerUnit.Spawn();
        _counterResources.SubtracteCostSpawnUnit();
    }

    private void AppointResource(Unit unit)
    {
        Resource resource = _nearestResourceScanner.FindNearestResource();

        if (resource != null)
        {
            unit.AssignResource(resource);
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