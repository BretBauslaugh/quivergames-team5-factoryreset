using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoom : MonoBehaviour
{
    public float zoomSpeed = 2f;
    public float minHeight = 3f;
    public float maxHeight = 40f;
    public float groundY = 0f;

    Camera cam;
    Plane ground;
    bool panning;
    Vector3 grabPoint;

    void Awake()
    {
        cam = GetComponent<Camera>();
        ground = new Plane(Vector3.up, new Vector3(0f, groundY, 0f));
    }

    bool GroundPoint(Vector2 screen, out Vector3 point)
    {
        Ray ray = cam.ScreenPointToRay(screen);
        if (ground.Raycast(ray, out float d)) { point = ray.GetPoint(d); return true; }
        point = default;
        return false;
    }

    void Update()
    {
        var mouse = Mouse.current;
        if (mouse == null) return;
        Vector2 mousePos = mouse.position.ReadValue();

        // camera panning
        if (mouse.middleButton.wasPressedThisFrame)
            panning = GroundPoint(mousePos, out grabPoint);
        if (mouse.middleButton.wasReleasedThisFrame)
            panning = false;

        if (panning && mouse.middleButton.isPressed && GroundPoint(mousePos, out Vector3 current))
            transform.position += grabPoint - current;

        // mouse zoom
        float raw = mouse.scroll.ReadValue().y;
        if (raw != 0f)
        {
            Vector3 step = cam.ScreenPointToRay(mousePos).direction * Mathf.Sign(raw) * zoomSpeed;

            float minY = groundY + minHeight, maxY = groundY + maxHeight;
            float newY = transform.position.y + step.y;

            if (newY < minY || newY > maxY)
            {
                float clampedY = Mathf.Clamp(newY, minY, maxY);
                step = Mathf.Abs(step.y) > 0.0001f ? step * ((clampedY - transform.position.y) / step.y) : Vector3.zero;
            }

            transform.position += step;
        }
    }
}