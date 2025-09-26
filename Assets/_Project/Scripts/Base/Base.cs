using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Scanner))]
[RequireComponent(typeof(CounterResources))]
[RequireComponent(typeof(SpawnerUnit))]
public class Base : MonoBehaviour
{
    [SerializeField] private List<Resource> _resources = new List<Resource>();
    [SerializeField] private List<Unit> _units = new List<Unit>();
    [SerializeField] private FinderResource _nearestResourceScanner;
    [SerializeField] private BaseResources _baseResources;
    [SerializeField] private float _scanInterval = 0.5f;

    private Flag _flag;
    private CounterResources _counterResources;
    private SpawnerUnit _spawnerUnit;
    private Scanner _scanner;

    public int NumberUnits => _units.Count;

    public bool IsSetFlag { get; private set; } 

    public event Action<Vector3, Unit> SpawnNewBase;

    private void Awake()
    {
        _counterResources = GetComponent<CounterResources>();
        _spawnerUnit = GetComponent<SpawnerUnit>();
        _scanner = GetComponent<Scanner>();

        IsSetFlag = false;
    }

    private void OnEnable()
    {
        _counterResources.SpawnUnitAllowed += SpawnUnit;
        _counterResources.CreatedNewBase += CreateNewBase;
        _scanner.WorkedScanner += UpdateResourcesBase;
        _baseResources.SyncAllResources += UpdateResourcesBase;
    }

    private void Start()
    {
        StartCoroutine(SendingBotsRoutine());
    }

    private void OnDisable()
    {
        _counterResources.SpawnUnitAllowed -= SpawnUnit;
        _counterResources.CreatedNewBase -= CreateNewBase;
        _scanner.WorkedScanner -= UpdateResourcesBase;
        _baseResources.SyncAllResources -= UpdateResourcesBase;
    }

    private void OnDestroy()
    {
        foreach (Unit unit in _units)
        {
            unit.DeletedResource -= DeleteResource;
        }

        _units.Clear();
    }

    public void Initialize(BaseResources baseResources)
    {
        _baseResources = baseResources;
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
        if (unit == null) 
            return;

        unit.DeletedResource += DeleteResource;
        unit.AssignBasePosition(transform.position);
        _units.Add(unit);
    }

    public void BorrowResource(Resource resource)
    {
        if (resource == null)
            return;

        _resources.Remove(resource);
        _baseResources.BorrowResource(resource);
    }
 
    public IReadOnlyList<Resource> GetResources()
    {
        return _resources.AsReadOnly();
    }

    private void DeleteResource(Resource resource)
    {
        _baseResources.DeleteResource(resource);
        _counterResources.AddScore(resource.Value);
    }

    private void CreateNewBase() 
    {
        if (_units.Count <= 0) 
            return;

        Unit unit = _units[0];
        _units.RemoveAt(0);

        _flag?.Return();
        UpdateStatusFlag(false);

        unit.DeletedResource -= DeleteResource;
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

    private void ChangeUnitTarget(Transform target, Unit unit)
    {
        unit.Active(target.position);
    }

    private void UpdateResourcesBase()
    {
        UpdateResourcesBase(_resources);
    }

    private void UpdateResourcesBase(List<Resource> newResources)
    {
        _resources = newResources.Where(resource => _baseResources.GetBusyResources().Contains(resource) == false).ToList();
    }

    private IEnumerator SendingBotsRoutine()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(_scanInterval);

        while (enabled)
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