using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBase : MonoBehaviour
{
    [SerializeField] private Base _prefabBase;
    [SerializeField] private List<Base> _bases = new List<Base>();

    private void Awake()
    {
        for(int i = 0; i < _bases.Count; i++)
        {
            _bases[i].AssignId(i);
            _bases[i].SpawnNewBase += StartSpawn;
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
        Base baseUnit = Instantiate(_prefabBase, position, Quaternion.identity);
        baseUnit.SpawnNewBase += StartSpawn;
        _bases.Add(baseUnit);
        baseUnit.AssignId(_bases.Count - 1);

        yield return new WaitUntil(() => unit.IsActive == false);
        
        baseUnit.SetUnit(unit);
    }
}