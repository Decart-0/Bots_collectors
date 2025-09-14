using UnityEngine;

public class SpawnerUnit : MonoBehaviour
{
    [SerializeField] private Unit _prefab;
    [SerializeField] private Transform _parentObject;
    [SerializeField] private int _quantityMax = 3;

    private void Awake()
    {
        Unit unit;

        for (int i = 0; i < _quantityMax; i++)
        {
            unit = Instantiate(_prefab, _parentObject);
        }
    }
}