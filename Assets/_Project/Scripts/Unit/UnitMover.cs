using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class UnitMover : MonoBehaviour
{
    private const float StoppingDistance = 0.1f;

    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private float _speed = 5f;

    private Unit _unit;

    private void Awake()
    {
        _unit = GetComponent<Unit>();
    }

    public IEnumerator SmoothLookAt()
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

    public IEnumerator MoveToPosition()
    {
        Vector3 targetPosition = GetTargetPosition();
        float distance = (transform.position - targetPosition).sqrMagnitude;

        while (distance > StoppingDistance)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                _speed * Time.deltaTime);

            distance = (transform.position - targetPosition).sqrMagnitude;

            yield return null;
        }
    }

    private Vector3 GetTargetPosition()
    {
        return new Vector3(_unit.TargetPoint.x, transform.position.y, _unit.TargetPoint.z);
    }
}