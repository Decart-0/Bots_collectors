using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(InputScheme))]
public class InstallerFlag : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _baseLayer;
    [SerializeField] private Flag _flagPrefab;
    [SerializeField] private int _clicksRequired = 1;
    [SerializeField] private int _quantityMax = 2;
    [SerializeField] private int _requiredUnitsNewBase = 1;

    private ObjectPool<Flag> _flagPool;
    private Base _selectedBase;
    private InputScheme _inputScheme;
    private Camera _mainCamera;

    private int _clickCounter;

    private void Awake()
    {
        _inputScheme = GetComponent<InputScheme>();
        _mainCamera = Camera.main;

        InitializePool();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown((int)_inputScheme.SpawnBase))
        {
            HandleMouseClick();
        }
    }

    private void InitializePool()
    {
        _flagPool = new ObjectPool<Flag>
        (
            createFunc: CreateFlag,
            actionOnGet: OnGetFlag,
            actionOnRelease: OnReleaseFlag,
            actionOnDestroy: OnDestroyFlag,
            collectionCheck: true,
            defaultCapacity: _quantityMax,
            maxSize: _quantityMax
        );
    }

    private void HandleMouseClick()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (_clickCounter < _clicksRequired)
        {
            SelectBase(ray);
        }
        else
        {
            HandleFlagOperation(ray);
        }
    }

    private void SelectBase(Ray ray)
    {
        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, _baseLayer))
        {
            if (hit.collider.TryGetComponent(out Base baseUnit))
            {
                if (baseUnit.NumberUnits > _requiredUnitsNewBase)
                {
                    _selectedBase = baseUnit;
                    _clickCounter++;
                }
                else
                {
                    Debug.Log($"У базы {baseUnit.NumberUnits} унит");
                }
            }
        }
    }

    private void HandleFlagOperation(Ray ray)
    {
        if (_selectedBase == null) return;

        if (Physics.Raycast(ray, out var hit, Mathf.Infinity))
        {
            if (IsBase(hit.collider.gameObject))
            {
                Debug.Log("Нельзя ставить флаг на базу!");
                ResetSelection();

                return;
            }

            if (_selectedBase.IsSetFlag)
            {
                RearrangeFlag(hit.point);
            }
            else
            {
                SpawnFlag(hit.point);
            }

            ResetSelection();
        }
    }

    private void SpawnFlag(Vector3 position)
    {
        if (_selectedBase == null) return;

        Flag flag = _flagPool.Get();
        flag.transform.position = position;

        flag.ReturnedToPool += ReturnFlagToPool;

        _selectedBase.AssignFlag(flag);
    }

    private void RearrangeFlag(Vector3 newPosition)
    {
        _selectedBase.RearrangeFlag(newPosition);
    }

    private void ReturnFlagToPool(Flag flag)
    {
        flag.ReturnedToPool -= ReturnFlagToPool;
        _flagPool.Release(flag);
    }

    private void ResetSelection()
    {
        _clickCounter = 0;
        _selectedBase = null;
    }

    private bool IsBase(GameObject obj)
    {
        return _baseLayer == (_baseLayer | (1 << obj.layer));
    }

    private Flag CreateFlag()
    {
        Flag flag = Instantiate(_flagPrefab);
        flag.gameObject.SetActive(false);

        return flag;
    }

    private void OnGetFlag(Flag flag)
    {
        flag.gameObject.SetActive(true);
    }

    private void OnReleaseFlag(Flag flag)
    {
        flag.gameObject.SetActive(false);
    }

    private void OnDestroyFlag(Flag flag)
    {
        Destroy(flag.gameObject);
    }
}