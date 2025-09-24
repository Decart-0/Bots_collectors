using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class UnitMover : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private float _speed = 5f;

    private Unit _unit;
    private Coroutine _movementCoroutine;
    private Transform _targetPoint;

    private void Awake()
    {
        _unit = GetComponent<Unit>();
    }

    private void OnDestroy()
    {
        StopMovement();
    }

    public void StartMovement(Transform transform)
    {
        _targetPoint = transform;
        StopMovement();

        if (_targetPoint != null && _movementCoroutine == null)
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
        return new Vector3(_targetPoint.position.x, transform.position.y, _targetPoint.position.z);
    }

    private IEnumerator SmoothLookAt()
    {
        Vector3 targetPosition = GetTargetPosition();

        if (targetPosition == null) yield break;

        Vector3 direction = (targetPosition - transform.position).normalized;

        if (direction.sqrMagnitude > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            float angleDifference = Quaternion.Angle(transform.rotation, targetRotation);

            while (angleDifference > 0)
            {
                if (targetPosition == null) yield break;

                targetPosition = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
                direction = (targetPosition - transform.position).normalized;

                if (direction.sqrMagnitude > 0)
                {
                    targetRotation = Quaternion.LookRotation(direction);

                    transform.rotation = Quaternion.Slerp(
                        transform.rotation,
                        targetRotation,
                        _rotationSpeed * Time.deltaTime);

                    angleDifference = Quaternion.Angle(transform.rotation, targetRotation);
                }

                yield return null;
            }
        }
    }

    private IEnumerator MoveToPosition()
    {
        Vector3 targetPosition = GetTargetPosition();
        float distance = Vector3.Distance(transform.position, targetPosition);

        while (distance > 0 && _unit.IsActive)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                _speed * Time.deltaTime);

            distance = Vector3.Distance(transform.position, targetPosition);

            yield return null;
        }
    }

    private IEnumerator MovementCoroutine()
    {
        while(_unit.IsActive) 
        {
            yield return SmoothLookAt();
            yield return MoveToPosition();

            yield return null;
        }
    }
}