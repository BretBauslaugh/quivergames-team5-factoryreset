using FMOD.Studio;
using FMODUnity;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlaceObject : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private float _placeDelay = 0.1f;
    [SerializeField] private Material _previewMat;
    [SerializeField] private int _placeRotation;
    [SerializeField] LayerMask _groundMask;
    [SerializeField] private Grid _grid;
    [SerializeField] private BuildingGrid _buildingGrid;
    [SerializeField] private Resource _resource;
    

    [Header("FMOD")]
    [SerializeField] EventReference _placeItemSFX;
    [SerializeField] EventReference _hoverItemSFX;

    private GameObject _placeable;
    private GameObject _previewObject;
    private Structure _placeableData;

    private Camera _camera;
    private Vector3 _cursorPos;
    private Vector3 _cursorToGridPos;
    private Vector3 _currentHoverPosition; //this is to prevent a bug for the hover SFX
    private bool _canPlace = false;
    private bool _validSpot = true;
    public bool IsPlacing => _previewObject != null;

    public ShopButton shopButton;

    void Start()
    {
        _camera = Camera.main;    
    }

    public void SelectPlaceable(GameObject prefab){
        _placeable = prefab;
        _placeableData = prefab.GetComponent<BuildingInstance>().Data;

        if(_previewObject != null)
        {
            Destroy(_previewObject);
        }

        _previewObject = Instantiate(_placeable);
        foreach (var c in _previewObject.GetComponentsInChildren<Collider>()) c.enabled = false;
        MakePreviewMaterial(_previewObject, _previewMat);
        SetPreviewColor(_previewObject, Color.green);
        _validSpot = true;
        SetLayer(_previewObject, LayerMask.NameToLayer("Preview"));
        StartCoroutine(PlacementDelay(_placeDelay));
    }

    public void GrabShopPrefab()//update to check for cost
    {
        GameObject prefab = shopButton.modulePrefab;
        Structure data = prefab.GetComponent<BuildingInstance>().Data;
        
        if (_resource.TryGetMissing(data.Cost , out var missing))
        {
            ErrorText.Show($"Not enough {missing.DisplayName}");
            CancelPlacement();
            return;
        }

        SelectPlaceable(prefab);

    }

    public void TryPlace()
    {
        if (_canPlace && _placeable != null)
        {
            //_resource.LogAll();
            Vector3Int cell = _buildingGrid.ToCell(_cursorToGridPos);
            if (!_buildingGrid.IsFree(cell)) return;

            if (!_resource.CanAfford(_placeableData.Cost))
            {
                Debug.Log("Not enough resource");
                return;
            }
            
            _resource.Spend(_placeableData.Cost);

            var placed = Instantiate(_placeable, _cursorToGridPos.Flatten(), _previewObject.transform.rotation);
            placed.GetComponent<BuildingInstance>().Init(_buildingGrid, cell);
            SetLayer(placed, LayerMask.NameToLayer("Placeable"));
            _canPlace = false;
            PlaySFX(_placeItemSFX);
            Destroy(_previewObject);
            _previewObject = null;
        }
    }

    private IEnumerator PlacementDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _canPlace = true;
    }

    void Update()
    {
        if (Mouse.current == null) return;
        Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (_previewObject == null) return;

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            _cursorPos = hit.point;

            Vector3Int cellPos = _grid.WorldToCell(_cursorPos);
            _cursorToGridPos = _grid.GetCellCenterWorld(cellPos);

            if (_previewObject != null)
            {
                _previewObject.transform.position = _cursorToGridPos.Flatten();
                if (_previewObject.transform.position != _currentHoverPosition) //this is to prevent the hover SFX from spamming when moving an item is idle
                {
                    Play3DSFX(_hoverItemSFX);
                    _currentHoverPosition = _previewObject.transform.position;
                }

                Vector3Int cell = _buildingGrid.ToCell(_cursorToGridPos);
                bool valid = _buildingGrid.IsFree(cell) && _resource.CanAfford(_placeableData.Cost);

                if (valid != _validSpot)
                {
                    _validSpot = valid;
                    SetPreviewColor(_previewObject, valid ? Color.green : Color.red);
                }
            }

        }

        
    }

    private void SetLayer(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;
        foreach (Transform child in obj.transform)
        {
            SetLayer(child.gameObject, newLayer);
        }
    }

    private void MakePreviewMaterial(GameObject obj, Material mat)
    {
        foreach (Renderer renderer in obj.GetComponentsInChildren<Renderer>())
        {
            renderer.material = mat;
        }
    }

    private void SetPreviewColor(GameObject obj, Color color)
    {
        foreach(Renderer r in obj.GetComponentsInChildren<Renderer>())
        {
            Color c = color;
            c.a = r.material.color.a;
            r.material.color = c;
        }
    }

    public void TryRotateRight()
    {
        if (_canPlace && _previewObject != null)
        {
            _previewObject.transform.Rotate(0f, _placeRotation, 0f);
        }
    }

    public void TryRotateLeft()
    {
        if (_canPlace && _previewObject != null)
        {
            _previewObject.transform.Rotate(0f, -_placeRotation, 0f);
        }
    }

    public void CancelPlacement()
    {
        _canPlace = false;
        _placeable = null;
        if (_previewObject != null)
        {
            Destroy(_previewObject);
            _previewObject = null;
        }
    }

    // FMOD
    public void Play3DSFX(EventReference SFX)
    {
        EventInstance instance = RuntimeManager.CreateInstance(SFX);
        instance.start();
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(_previewObject.transform.position));
        instance.release();
    }

    public void PlaySFX(EventReference SFX)
    {
        EventInstance instance = RuntimeManager.CreateInstance(SFX);
        instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position)); //added to prevent warnings
        RuntimeManager.PlayOneShot(SFX, transform.position);
    }
}
