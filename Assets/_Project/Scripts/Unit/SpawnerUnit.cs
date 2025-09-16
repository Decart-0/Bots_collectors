using UnityEngine;

[RequireComponent (typeof(Base))] 
public class SpawnerUnit : MonoBehaviour
{
    [SerializeField] private Unit _prefab;
    [SerializeField] private Transform _parentObject;
    [SerializeField] private int _quantityMax = 3;

    private Base _base;

    private void Awake()
    {
        _base = GetComponent<Base>();
        Unit unit;

        for (int i = 0; i < _quantityMax; i++)
        {
            unit = Instantiate(_prefab, _parentObject);
            _base.GetUnit(unit);
        }
    }
}