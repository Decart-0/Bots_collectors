using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class SpawnerResources : MonoBehaviour
{
    private const float HalfDivider = 2f;

    [SerializeField] private Map _map;
    [SerializeField] private Resource _resource;
    [SerializeField] private Transform _parentObject;
    [SerializeField] private BaseResources _baseResources;

    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private int _quantityMax = 10;
    [SerializeField] private float _spawnInterval = 1f;

    private WaitForSeconds _waitForSeconds;
    private Coroutine _coroutine;
    private ObjectPool<Resource> _pool;

    private bool _isSpawning = false;

    private void Awake()
    {
        _waitForSeconds = new WaitForSeconds(_spawnInterval);

        _pool = new ObjectPool<Resource>(
            createFunc: CreateResource,
            actionOnGet: OnGetResource,
            actionOnRelease: OnReleaseResource,
            actionOnDestroy: OnDestroyResource,
            collectionCheck: true,
            defaultCapacity: _quantityMax,
            maxSize: _quantityMax
        );
    }

    private void Start()
    {
        Active();
    }

    private void OnEnable()
    {
        _baseResources.ChangedListResources += Active;
    }

    private void OnDisable()
    {
        _baseResources.ChangedListResources -= Active;
    }

    private void OnDestroy()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        if (_pool != null)
        {
            _pool?.Dispose();
        }
    }

    public void ReleaseResource(Resource resource)
    {
        if (resource != null)
        {
            _baseResources.DeleteResource(resource);
            _pool.Release(resource);
        }
    }

    private void Active()
    {
        if (_baseResources.NumberResources < _quantityMax && _isSpawning == false)
        {
            _isSpawning = true;
            _coroutine = StartCoroutine(RunSpawn());
        }
    }

    private Resource CreateResource()
    {
        Resource resource = Instantiate(_resource, _parentObject);
        resource.gameObject.SetActive(false);

        return resource;
    }

    private void OnGetResource(Resource resource)
    {
        Vector3 randomPosition = GetRandomPosition(_map.Bounds);

        if (IsEmptyPosition(randomPosition))
        {
            resource.transform.position = randomPosition;
            resource.transform.SetParent(_parentObject);
            resource.gameObject.SetActive(true);
        }
        else
        {
            _pool.Release(resource);
        }
    }

    private void OnReleaseResource(Resource resource)
    {
        resource.gameObject.SetActive(false);
    }

    private void OnDestroyResource(Resource resource)
    {
        if (resource != null)
        {
            Destroy(resource.gameObject);
        }
    }

    private bool IsEmptyPosition(Vector3 position)
    {
        return Physics.CheckBox(
            position, _resource.transform.localScale / HalfDivider,
            Quaternion.identity, _layerMask) == false;
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
        while (_baseResources.NumberResources < _quantityMax)
        {
            Resource resource = _pool.Get();
            yield return _waitForSeconds;
        }

        _isSpawning = false;
    }
}