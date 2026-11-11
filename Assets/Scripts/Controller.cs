using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    [SerializeField] private GameObject _target;

    private PlaceObject _placeObject;
    private Camera _mainCamera;
    private SelectionManager _selection;
    void Start()
    {
        _mainCamera = Camera.main;  
        
        if(_target.TryGetComponent(out PlaceObject placeObject))
        {
            _placeObject = placeObject;
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} Get component failed");
        }
    }

    public void OnInteract(InputValue value)
    {
        if (!value.isPressed) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (_placeObject.IsPlacing)
        {
            _placeObject.TryPlace();
        }
        else
        {
            _selection.ClickSelect();
        }
    }


    public void OnRotateRight(InputValue value)
    {
        if (_placeObject != null)
        {
            _placeObject.TryRotateRight();
        }
    }

    public void OnRotateLeft(InputValue value)
    {
        if (_placeObject != null)
        {
            _placeObject.TryRotateLeft();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
