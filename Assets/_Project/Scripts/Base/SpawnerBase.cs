using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BaseResources))]
public class SpawnerBase : MonoBehaviour
{
    [SerializeField] private Base _prefabBase;
    [SerializeField] private List<Base> _bases = new List<Base>();

    private BaseResources _baseResources;

    private void Awake()
    {
        _baseResources = GetComponent<BaseResources>();

        foreach (Base baseUnit in _bases)
        {
            baseUnit.SpawnNewBase += StartSpawn;
        }
    }

    private void OnDestroy()
    {
        foreach (Base baseUnit in _bases)
        {
            if (baseUnit != null)
            {
                baseUnit.SpawnNewBase -= StartSpawn;
            }
        }
    }

    public void StartSpawn(Vector3 position, Unit unit) 
    {
        StartCoroutine(SpawnBase(position, unit));
    }

    private IEnumerator SpawnBase(Vector3 position, Unit unit)
    {
        _prefabBase.gameObject.SetActive(false);
        Base baseUnit = Instantiate(_prefabBase, position, Quaternion.identity);
        baseUnit.Initialize(_baseResources);
        baseUnit.SpawnNewBase += StartSpawn;
        baseUnit.gameObject.SetActive(true);
        baseUnit.SetUnit(unit);
        _bases.Add(baseUnit);

        yield return new WaitUntil(() => unit.IsActive == false);
    }
}