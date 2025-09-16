using UnityEngine;

[RequireComponent(typeof(UnitMover))]
[RequireComponent(typeof(DetectorResource))]
[RequireComponent(typeof(ResourcePickerUp))]
[RequireComponent(typeof(DetectorBase))]
public class Unit : MonoBehaviour
{
    [field:SerializeField] public bool IsActive { get; private set; }

    private UnitMover _unitMover;
    private DetectorResource _detectorResource;
    private ResourcePickerUp _resourcePickerUp;
    private DetectorBase _detectorBase;

    private void Awake()
    {
        _unitMover = GetComponent<UnitMover>();
        _detectorResource = GetComponent<DetectorResource>();
        _resourcePickerUp = GetComponent<ResourcePickerUp>();
        _detectorBase = GetComponent<DetectorBase>();

        ToggleActive(false);
    }

    private void OnEnable()
    {
        _detectorResource.ResourceFound += PickUpResource;
        _detectorBase.BaseFound += ToggleActive;
    }

    private void OnDisable()
    {
        _detectorResource.ResourceFound -= PickUpResource;
        _detectorBase.BaseFound -= ToggleActive;
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