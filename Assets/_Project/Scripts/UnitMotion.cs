using UnityEngine;

[RequireComponent(typeof(Unit))]
public class UnitMotion : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private float _speed;

    private Unit _unit;
    
    public Transform TargetPoint { get; private set; }

    private void Awake()
    {
        _unit = GetComponent<Unit>();
    }

    private void Update()
    {
        if (_unit.IsActive && TargetPoint != null)
        {
            Move();
        }
    }

    private void OnEnable()
    {
        _unit.Actived += SetTarget;
    }

    private void OnDestroy()
    {
        _unit.Actived -= SetTarget;
    }

    public void SetTarget(Transform transform)
    {
        TargetPoint = transform;
    }

    private void Move()
    {
        Vector3 targetPosition = GetTargetPosition();
        transform.position = Vector3.MoveTowards( transform.position, targetPosition, _speed * Time.deltaTime);
        Rotate(targetPosition);
    }

    private Vector3 GetTargetPosition()
    { 
        return new Vector3( TargetPoint.position.x, transform.position.y, TargetPoint.position.z);
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
}