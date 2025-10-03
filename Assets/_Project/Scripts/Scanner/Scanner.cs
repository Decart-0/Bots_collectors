using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private Transform _size;
    [SerializeField] private float _scanInterval = 2f;
    [SerializeField] private LayerMask _layerMask;

    private WaitForSeconds _waitForSeconds;
    private Coroutine _coroutine;

    public event Action<List<Resource>> WorkedScanner;

    private void Awake()
    {
        _waitForSeconds = new WaitForSeconds(_scanInterval);
    }

    private void Start()
    {
        _coroutine = StartCoroutine(RecurringScan());
    }

    private void OnDestroy()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
    }

    private void Scan()
    {
        Collider[] colliders = Physics.OverlapBox(
            transform.position, 
            _size.lossyScale, 
            Quaternion.identity, 
            _layerMask);

        List<Resource> resources = new List<Resource>();

        foreach (Collider collider in colliders) 
        {
            if (collider.TryGetComponent(out Resource resource)) 
            { 
                resources.Add(resource);
            }
        }

        if (resources.Count > 0)
        {
            WorkedScanner?.Invoke(resources);
        }
    }

    private IEnumerator RecurringScan()
    {      
        while (enabled)
        {
            Scan();
            yield return _waitForSeconds;
        }
    }
}