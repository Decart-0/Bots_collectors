using UnityEngine;

[RequireComponent (typeof(Base))] 
public class SpawnerUnit : MonoBehaviour
{
    [SerializeField] private Unit _prefab;
    [SerializeField] private Transform _parentObject;
    [SerializeField] private int _startQuantityUnits = 3;

    private Base _base;

    private void Awake()
    {
        _base = GetComponent<Base>();

        for (int i = 0; i < _startQuantityUnits; i++)
        {
            Spawn();
        }
    }

    public void Spawn() 
    {
        Unit unit = Instantiate(_prefab, _parentObject);
        _base.SetUnit(unit);
    }
}