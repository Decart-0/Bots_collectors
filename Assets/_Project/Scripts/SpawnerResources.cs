using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnerResources : MonoBehaviour
{
    private const float HalfDivider = 2f;

    [SerializeField] private DistributorResource _distributorResource;
    [SerializeField] private Map _map;
    [SerializeField] private Resource _resource;
    [SerializeField] private Transform _parentObject;

    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private int _quantityMax = 10;
    [SerializeField] private float _spawnInterval = 1f;

    private WaitForSeconds _waitForSeconds;
    private Coroutine _coroutine;
    private bool _isSpawning = false;

    private void Awake()
    {
        _waitForSeconds = new WaitForSeconds(_spawnInterval);
    }

    private void Start()
    {
        Active();
    }

    private void Update()
    {
        if (_distributorResource.AmountResources < _quantityMax && _isSpawning == false) 
        {
            Active();
        }
    }

    private void OnDestroy()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
    }

    private void Active() 
    {
        _isSpawning = true;
        _coroutine = StartCoroutine(RunSpawn());
    }

    private void Create(Bounds mapBounds)
    {
        Vector3 randomPosition = GetRandomPosition(mapBounds);

        if (IsEmptyPosition(randomPosition) == false) 
        {
            Resource newResource = Instantiate(_resource, randomPosition, Quaternion.identity, _parentObject);
            _distributorResource.AddResource(newResource);
        }           
    }

    private bool IsEmptyPosition(Vector3 position)
    {
        return Physics.CheckBox(
            position, _resource.transform.localScale / HalfDivider, 
            Quaternion.identity, _layerMask);
    }

    private Vector3 GetRandomPosition(Bounds mapBounds)
    {
        return new Vector3(
            Random.Range(mapBounds.min.x, mapBounds.max.x),
            mapBounds.max.y + _resource.transform.localScale.y / HalfDivider,
            Random.Range(mapBounds.min.z, mapBounds.max.z));
    }

    private IEnumerator RunSpawn() 
    {
        while (_distributorResource.AmountResources < _quantityMax) 
        {
            Create(_map.Bounds);

            yield return _waitForSeconds;
        }

        _isSpawning = false;
    }
}