using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoom : MonoBehaviour
{
    public float zoomSpeed = 2f;
    public float keyPanSpeed = 2f;
    public float minHeight = 3f;
    public float maxHeight = 40f;
    public float groundY = 0f;
    public Vector2 mapCenter = Vector2.zero;
    public Vector2 mapHalfSize = new Vector2(24f, 24f);

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
        if (Time.timeScale == 0f) return;   // for pause menu

        var mouse = Mouse.current;
        var keyboard = Keyboard.current;

        if (mouse != null)
        {
            Vector2 mousePos = mouse.position.ReadValue();
            HandleMousePan(mouse, mousePos);
            HandleZoom(mouse, mousePos);
        }

        if (keyboard != null) HandleKeyPan(keyboard);
        ClampToBounds();
    }

    void HandleMousePan(Mouse mouse, Vector2 mousePos)
    {
        if (mouse.middleButton.wasPressedThisFrame)
            panning = GroundPoint(mousePos, out grabPoint);
        if (mouse.middleButton.wasReleasedThisFrame)
            panning = false;

        if (panning && mouse.middleButton.isPressed && GroundPoint(mousePos, out Vector3 current))
            transform.position += grabPoint - current;
    }

    void HandleKeyPan(Keyboard kb)
    {
        Vector2 input = Vector2.zero;
        if (kb.wKey.isPressed) input.y += 1f;
        if (kb.sKey.isPressed) input.y -= 1f;
        if (kb.dKey.isPressed) input.x += 1f;
        if (kb.aKey.isPressed) input.x -= 1f;
        if (input == Vector2.zero) return;

        Vector3 fwd = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
        if (fwd.sqrMagnitude < 0.001f) fwd = transform.up;
        fwd.Normalize();
        Vector3 right = Vector3.ProjectOnPlane(transform.right, Vector3.up).normalized;

        float scale = cam.orthographic ? cam.orthographicSize : Mathf.Abs(transform.position.y - groundY);

        Vector3 move = (right * input.x + fwd * input.y).normalized;
        transform.position += move * keyPanSpeed * scale * Time.deltaTime;
    }

    void HandleZoom(Mouse mouse, Vector2 mousePos)
    {
        float raw = mouse.scroll.ReadValue().y;
        if (raw == 0f) return;
        float dir = Mathf.Sign(raw);

        if (cam.orthographic)
        {
            if (!GroundPoint(mousePos, out Vector3 before)) return;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - dir * zoomSpeed, minHeight, maxHeight);
            if (GroundPoint(mousePos, out Vector3 after))
                transform.position += before - after;
        }
        else
        {
            Vector3 step = cam.ScreenPointToRay(mousePos).direction * dir * zoomSpeed;
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

    void ClampToBounds()
    {
        Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        if (!GroundPoint(screenCenter, out Vector3 look)) return;

        Vector3 clamped = new Vector3(
            Mathf.Clamp(look.x, mapCenter.x - mapHalfSize.x, mapCenter.x + mapHalfSize.x),
            look.y,
            Mathf.Clamp(look.z, mapCenter.y - mapHalfSize.y, mapCenter.y + mapHalfSize.y));

        transform.position += clamped - look;
    }
}