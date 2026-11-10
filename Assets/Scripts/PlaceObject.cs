using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlaceObject : MonoBehaviour
{
    [SerializeField] private float _placeDelay = 0.1f;
    [SerializeField] private Material _previewMat;
    [SerializeField] private int _placeRotation;
    [SerializeField] private Grid _grid;
    [SerializeField] private Resource _resource;

    private GameObject _placeable;
    private GameObject _previewObject;
    private Camera _camera;
    private Vector3 _cursorPos;
    private Vector3 _cursorToGridPos;
    private bool _canPlace = false;

    void Start()
    {
        _camera = Camera.main;    
    }

    public void SelectPlaceable(GameObject prefab){
        _placeable = prefab;
        if(_previewObject != null)
        {
            Destroy(_previewObject);
        }

        _previewObject = Instantiate(_placeable);
        foreach (var c in _previewObject.GetComponents<Collider>()) c.enabled = false;
        MakePreviewMaterial(_previewObject, _previewMat);
        SetLayer(_previewObject, LayerMask.NameToLayer("Preview"));
        StartCoroutine(PlacementDelay(_placeDelay));
    }

    public void TryPlace()
    {
        if (_canPlace && _placeable != null)
        {
            
            Instantiate(_placeable, _cursorToGridPos.Flatten(), _previewObject.transform.rotation);
            foreach (var c in _previewObject.GetComponents<Collider>()) c.enabled = true;
            _canPlace = false;

            if (_previewObject != null)
            {
                Destroy(_previewObject);
            }

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

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            _cursorPos = hit.point;

            Vector3Int cellPos = _grid.WorldToCell(_cursorPos);
            _cursorToGridPos = _grid.GetCellCenterWorld(cellPos);

            if (_previewObject != null)
            {
                _previewObject.transform.position = _cursorToGridPos.Flatten();
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
}
