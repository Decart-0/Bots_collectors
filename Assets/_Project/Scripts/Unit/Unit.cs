using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(UnitMover))]
public class Unit : MonoBehaviour
{
    [SerializeField] private Transform _resourcePosition;

    private UnitMover _unitMover;
    private Coroutine _movementCoroutine;
    [SerializeField] private Resource _resource;
    private Vector3 _basePosition;

    public bool IsActive { get; private set; }

    public Vector3 TargetPoint { get; private set; }

    public event Action<Resource> DeletedResource;

    private void Awake()
    {
        _unitMover = GetComponent<UnitMover>();
        ToggleActive(false);
    }

    private void OnDestroy()
    {
        StopMovement();
    }

    public void AssignBasePosition(Vector3 position)
    {
        _basePosition = position;
    }

    public void AssignResource(Resource resource)
    {
        _resource = resource;
    }

    public void Active(Vector3 resourcePosition) 
    {
        if (IsActive) 
            return;

        ToggleActive(true);
        TargetPoint = resourcePosition;
        StartMovement();
    }

    private void ToggleActive(bool status)
    {
        IsActive = status;
    }
  
    private void StartMovement()
    {
        StopMovement();
        _movementCoroutine = StartCoroutine(MovementCoroutine());
    }

    private void StopMovement()
    {
        if (_movementCoroutine != null)
        {
            StopCoroutine(_movementCoroutine);
            _movementCoroutine = null;
        }
    }

    private IEnumerator PickUpResource()
    {
        if (_resource != null)
        {
            _resource.transform.SetParent(transform);
            _resource.transform.position = _resourcePosition.position;
            TargetPoint = _basePosition;
        }
        
        yield return null;
    }

    private IEnumerator SubmitResource()
    {
        if (_resource != null)
        {
            DeletedResource?.Invoke(_resource);
            _resource = null;
        }

        ToggleActive(false);

        yield return null;
    }

    private IEnumerator MovementCoroutine()
    {
        while (IsActive)
        {
            yield return _unitMover.SmoothLookAt();
            yield return _unitMover.MoveToPosition();
            yield return PickUpResource();
            yield return _unitMover.SmoothLookAt();
            yield return _unitMover.MoveToPosition();
            yield return SubmitResource();

            yield return null;
        }
    }
}