using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public float panSpeed = 20f;
    public float zoomSpeed = 0.1f;
    public float minZoom = 5f;
    public float maxZoom = 100f;

    private Vector2 lastPanPosition;  // Use Vector2 for screen positions
    private int panFingerId;
    private bool isPanning;

    void Update()
    {
        if (Application.isMobilePlatform)
        {
            HandleTouch();
        }
        else
        {
            HandleMouse();
        }
    }

    void HandleMouse()
    {
        if (Input.GetMouseButtonDown(2))
        {
            lastPanPosition = Input.mousePosition;
            isPanning = true;
        }
        else if (Input.GetMouseButton(2))
        {
            Vector2 currentMousePos = Input.mousePosition;
            Vector2 delta = currentMousePos - lastPanPosition;
            PanCamera(delta);
            lastPanPosition = currentMousePos;
        }
        else if (Input.GetMouseButtonUp(2))
        {
            isPanning = false;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        ZoomCamera(scroll * 1000f);
    }

    void HandleTouch()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                lastPanPosition = touch.position;
                panFingerId = touch.fingerId;
                isPanning = true;
            }
            else if (touch.phase == TouchPhase.Moved && touch.fingerId == panFingerId)
            {
                Vector2 delta = touch.position - lastPanPosition;
                PanCamera(delta);
                lastPanPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isPanning = false;
            }
        }
        else if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            Vector2 prevPos0 = touch0.position - touch0.deltaPosition;
            Vector2 prevPos1 = touch1.position - touch1.deltaPosition;

            float prevMag = (prevPos0 - prevPos1).magnitude;
            float currentMag = (touch0.position - touch1.position).magnitude;

            float difference = currentMag - prevMag;
            ZoomCamera(difference * zoomSpeed);
        }
    }

    void PanCamera(Vector2 delta)
    {
        Vector3 move = new Vector3(-delta.x * panSpeed * Time.deltaTime, -delta.y * panSpeed * Time.deltaTime, 0);
        transform.Translate(move, Space.World);
    }

    void ZoomCamera(float increment)
    {
        Camera cam = Camera.main;
        if (cam.orthographic)
        {
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - increment, minZoom, maxZoom);
        }
        else
        {
            cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - increment, minZoom, maxZoom);
        }
    }
}