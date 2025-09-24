using UnityEngine;

[RequireComponent(typeof(UnitMover))]
[RequireComponent(typeof(DetectorResource))]
[RequireComponent(typeof(ResourcePickerUp))]
public class Unit : MonoBehaviour
{
    [field:SerializeField] public bool IsActive { get; private set; }

    private UnitMover _unitMover;
    private DetectorResource _detectorResource;
    private ResourcePickerUp _resourcePickerUp;

    public int IdBase { get; private set; }

    public Resource Resource { get; private set; }

    private void Awake()
    {
        _unitMover = GetComponent<UnitMover>();
        _detectorResource = GetComponent<DetectorResource>();
        _resourcePickerUp = GetComponent<ResourcePickerUp>();

        ToggleActive(false);
    }

    private void OnEnable()
    {
        _detectorResource.ResourceFound += PickUpResource;
    }

    private void OnDisable()
    {
        _detectorResource.ResourceFound -= PickUpResource;
    }

    public void AssignId(int id)
    {
        IdBase = id;
    }

    public void AssignResource(Resource resource)
    {
        Resource = resource;
    }

    public void Active(Transform resource) 
    {   
        ToggleActive(true);
        StartMovement(resource);
    }

    public void ToggleActive(bool status)
    {
        IsActive = status;
    }

    private void StartMovement(Transform transform)
    {
        _unitMover.StartMovement(transform);
    }

    private void PickUpResource(Resource resource) 
    {
        _resourcePickerUp.PickUp(resource);
    }
}