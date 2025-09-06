using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class UnitMotion : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private float _speed = 5f;

    private Unit _unit;
    private Coroutine _movementCoroutine;

    public Transform TargetPoint { get; private set; }

    private void Awake()
    {
        _unit = GetComponent<Unit>();
    }

    private void OnEnable()
    {
        _unit.Actived += SetTarget;
    }

    private void OnDisable()
    {
        _unit.Actived -= SetTarget;
        StopMovement();
    }

    private void OnDestroy()
    {
        _unit.Actived -= SetTarget;
        StopMovement();
    }

    public void SetTarget(Transform transform)
    {
        TargetPoint = transform;

        StopMovement();

        if (TargetPoint != null)
        {
            _movementCoroutine = StartCoroutine(MovementCoroutine());
        }
    }

    private void StopMovement()
    {
        if (_movementCoroutine != null)
        {
            StopCoroutine(_movementCoroutine);
            _movementCoroutine = null;
        }
    }

    private Vector3 GetTargetPosition()
    { 
        return new Vector3( TargetPoint.position.x, transform.position.y, TargetPoint.position.z);
    }

    private void Move(Vector3 targetPosition)
    {
        transform.position = Vector3.MoveTowards(
            transform.position, 
            targetPosition, 
            _speed * Time.deltaTime);      
    }   

    private void Rotate(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;

        if (direction.sqrMagnitude > 0)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(direction),
                _rotationSpeed * Time.deltaTime);
        }
    }

    private IEnumerator MovementCoroutine()
    {
        while(_unit.IsActive) 
        {
            Vector3 targetPosition = GetTargetPosition();

            Move(targetPosition);
            Rotate(targetPosition);

            yield return null;
        }
    }
}